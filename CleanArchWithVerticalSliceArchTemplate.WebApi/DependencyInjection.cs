using CleanArchWithVerticalSliceArchTemplate.Application.Abstraction;
using CleanArchWithVerticalSliceArchTemplate.Application.Abstraction.Data;
using CleanArchWithVerticalSliceArchTemplate.Infrastructure.Database;
using CleanArchWithVerticalSliceArchTemplate.Infrastructure.Interceptors;
using CleanArchWithVerticalSliceArchTemplate.Infrastructure.Repository;
using CleanArchWithVerticalSliceArchTemplate.WebApi.Shared.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

namespace CleanArchWithVerticalSliceArchTemplate.WebApi
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Provides extension methods to configure dependency injection for an Authentication API.
        /// </summary>
        /// <remarks>
        /// This static class defines methods for setting up services required by the Authentication API, including API services, authentication, authorization, database
        /// services and CORS policies.
        /// </remarks>
        extension(WebApplicationBuilder builder)
        {
            /// <summary>
            /// Configures and registers the required API services for the application.
            /// </summary>
            /// <remarks>
            /// This method sets up essential services such as controllers, API endpoint exploration,
            /// OpenAPI/Swagger document generation, and dependency injection for repositories, unit of work, and other application-specific services.
            /// Additionally, it registers an audit interceptor to handle database operation tracking and scans for handler interfaces to allow dynamic dependency
            /// injection.
            /// </remarks>
            /// <returns>The modified <see cref="WebApplicationBuilder"/> instance, enabling further configuration.</returns>
            public WebApplicationBuilder AddApiServices()
            {
                builder.Services.AddSingleton<AuditInterceptor>();
                builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddOpenApi((options) =>
                {
                    options.AddDocumentTransformer(async (document, _, cancellationToken) =>
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        document.Components ??= new OpenApiComponents();
                        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

                        OpenApiSecurityRequirement requirement = new()
                        {
                            [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                        };

                        document.Security = [requirement];

                        await Task.CompletedTask;
                    });
                });
                builder.Services.Scan(scan => scan
                    .FromAssemblies(typeof(IApiEndpoint).Assembly)
                    .AddClasses(classes => classes.AssignableTo(typeof(IHandler<,>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

                return builder;
            }

            /// <summary>
            /// Configures authentication services for the application.
            /// </summary>
            /// <remarks>
            /// This method sets up JWT Bearer authentication for the application.
            /// It configures the authority for token validation using the Identity Server URL specified in the application's configuration.
            /// Additionally, it customizes token validation parameters, such as disabling audience validation and setting the claim type for roles.
            /// </remarks>
            /// <returns>The same <see cref="WebApplicationBuilder"/> instance, allowing for method chaining.</returns>
            public WebApplicationBuilder ConfigureAuthentication()
            {
                builder.Services
                    .AddAuthentication("Bearer")
                    .AddJwtBearer((options) =>
                    {
                        options.Authority = builder.Configuration["ServicesUrls:IdentityServer"];
                        options.MapInboundClaims = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateAudience = false,
                            RoleClaimType = "role"
                        };
                        options.Events = new JwtBearerEvents
                        {
                            OnAuthenticationFailed = (context) =>
                            {
                                Console.WriteLine($">>> JWT FAILED: {context.Exception.Message}");
                                return Task.CompletedTask;
                            },
                            OnTokenValidated = (context) =>
                            {
                                Console.WriteLine($">>> JWT VALID, claims: {
                                    string.Join(", ", context.Principal!.Claims.Select(ctx => $"{ctx.Type}={ctx.Value}"))
                                }");
                                return Task.CompletedTask;
                            }
                        };
                    });

                return builder;
            }

            /// <summary>
            /// Configures the authorization policies and services for the application.
            /// </summary>
            /// <remarks>
            /// This method sets up the default authorization policy to require authenticated users and defines a custom policy named "ApiScope".
            /// The "ApiScope" policy ensures that requests include a specific claim with the key "scope" and the value "geek_shopping", making it suitable for securing API
            /// endpoints with granular access control.
            /// </remarks>
            /// <returns>The same <see cref="WebApplicationBuilder"/> instance, allowing for method chaining.</returns>
            public WebApplicationBuilder ConfigureAuthorization()
            {
                builder.Services.AddAuthorization((options) =>
                {
                    options.DefaultPolicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();

                    options.AddPolicy(
                        name: "ApiScope",
                        configurePolicy: (policy) =>
                        {
                            policy.RequireAuthenticatedUser();
                            policy.RequireClaim("scope", "geek_shopping");
                        }
                    );
                });

                return builder;
            }

            /// <summary>
            /// Configures and registers the database services for the application.
            /// </summary>
            /// <remarks>
            /// This method sets up the application's database context with PostgreSQL as the database provider. It constructs the connection string dynamically based on
            /// the environment, ensuring compatibility for both local and Dockerized deployments. Additionally, it registers the necessary database context and interceptors,
            /// such as the <see cref="AuditInterceptor"/>, to enable auditing during database operations.
            /// </remarks>
            /// <returns>The same <see cref="WebApplicationBuilder"/> instance, allowing for method chaining.</returns>
            public WebApplicationBuilder AddDatabaseServices()
            {
                string? baseConnection = builder.Configuration.GetConnectionString("DefaultPostgresConnection");
                bool isDocker = builder.Configuration.GetValue<bool>("DOTNET_RUNNING_IN_CONTAINER");
                string host = (isDocker) ? "postgres" : "localhost";
                string finalConnectionString = $"Host={host};Port={5432};{baseConnection}";

                builder.Services.AddDbContext<AppDbContext>((sp, options) =>
                {
                    options.AddInterceptors(sp.GetRequiredService<AuditInterceptor>());
                    options.UseNpgsql(
                        finalConnectionString,
                        (npgsqlOptions) => npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName)
                    );
                });

                return builder;
            }

            /// <summary>
            /// Configures the application's CORS (Cross-Origin Resource Sharing) policy to allow unrestricted access from any origin, enabling development and testing scenarios.
            /// </summary>
            /// <remarks>
            /// This method sets up a default CORS policy named "DefaultCors" within the application's service collection.
            /// The policy permits requests from any origin, with any header, and any HTTP method.
            /// Use this policy to facilitate communication between client and server during development where cross-origin requests are necessary.
            /// Ensure proper configuration in production environments to restrict origins and secure communication.
            /// </remarks>
            public void AddCorsPolicy()
            {
                builder.Services.AddCors((options) =>
                {
                    options.AddPolicy("DefaultCors", (policy) =>
                    {
                        policy
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
                });
            }
        }

        /// <summary>
        /// Configures the application's middleware pipeline and API-related features.
        /// </summary>
        /// <remarks>
        /// This method sets up request localization with support for multiple cultures, configures middlewares such as HTTPS redirection, routing, CORS, authentication, and authorization.
        /// Additionally, it registers API controllers and maps OpenAPI documentation and scalar API references when in a development environment.
        /// </remarks>
        /// <param name="application">The <see cref="WebApplication"/> instance used to configure the application's middleware pipeline.</param>
        /// <returns>The same <see cref="WebApplication"/> instance, enabling method chaining for further configurations.</returns>
        public static WebApplication UseApi(this WebApplication application)
        {
            string[] supportedLanguages = ["en-US", "pt-BR"];

            RequestLocalizationOptions localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture(supportedLanguages[0])
                .AddSupportedCultures(supportedLanguages)
                .AddSupportedUICultures(supportedLanguages);

            if(application.Environment.IsDevelopment())
            {
                application.MapOpenApi();
                application.MapScalarApiReference((options) =>
                {
                    options.Authentication = new ScalarAuthenticationOptions
                    {
                        PreferredSecuritySchemes = ["Bearer"]
                    };
                    options.DarkMode = true;
                    options.DefaultHttpClient = new KeyValuePair<ScalarTarget, ScalarClient>(ScalarTarget.Shell, ScalarClient.Httpie);
                    options.DefaultOpenAllTags = false;
                    options.DocumentDownloadType = DocumentDownloadType.Json;
                    options.EnabledClients = [ScalarClient.Httpie];
                    options.ExpandAllResponses = false;
                    options.HideClientButton = false;
                    options.HideDarkModeToggle = false;
                    options.HideModels = true;
                    options.HideSearch = true;
                    options.HideTestRequestButton = false;
                    options.Layout = ScalarLayout.Modern;
                    options.OperationSorter = OperationSorter.Method;
                    options.SchemaPropertyOrder = PropertyOrder.Alpha;
                    options.ShowDeveloperTools = DeveloperToolsVisibility.Localhost; // or always or never.
                    options.ShowOperationId = false;
                    options.ShowSidebar = true;
                    options.SortTagsAlphabetically(); // The same as `options.TagSorter = TagSorter.Alpha`.
                    options.Telemetry = true;
                    options.Theme = ScalarTheme.BluePlanet;
                    options.Title = "AuthenticationAPI";
                });
            }

            application.UseRequestLocalization(localizationOptions);
            application.UseHttpsRedirection();
            application.UseRouting();
            application.UseCors("DefaultCors");
            application.UseAuthentication();
            application.UseAuthorization();
            application.UseMiddlewares();
            application.MapControllers();
            application.MapEndpoints(typeof(IApiEndpoint).Assembly);

            return application;
        }
    }
}