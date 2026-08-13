# -------------------------------------
# Stage 01: Shared build
# -------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /app
COPY . .

# Executa diretamente da raiz (/app)
RUN dotnet restore CleanArchitectureWithVerticalSliceArchitectureTemplate.slnx
RUN dotnet build CleanArchitectureWithVerticalSliceArchitectureTemplate.slnx -c Release

# -------------------------------------
# Publish
# -------------------------------------
FROM build AS publish-api
RUN dotnet publish CleanArchWithVerticalSliceArchTemplate.WebApi/CleanArchWithVerticalSliceArchTemplate.WebApi.csproj \
    -c Release \
    -o /app/publish/api \
    --no-build

# -------------------------------------
# Runtime
# -------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS vertical-slice-clean-architecture
WORKDIR /app
COPY --from=publish-api /app/publish/api .
EXPOSE 8080

# Certifique-se de que o nome do .dll condiz com o Assembly Name da sua aplicação
ENTRYPOINT [ "dotnet", "CleanArchWithVerticalSliceArchTemplate.WebApi.dll" ]