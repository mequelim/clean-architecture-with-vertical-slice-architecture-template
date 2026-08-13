using CleanArchWithVerticalSliceArchTemplate.Application.Abstraction.Data;

namespace CleanArchWithVerticalSliceArchTemplate.Infrastructure.Repository
{
    using CleanArchWithVerticalSliceArchTemplate.Infrastructure.Database;

    public class UnitOfWork(AppDbContext dataContext) : IUnitOfWork
    {
        public int Commit() => dataContext.SaveChanges();

        public async Task<int> CommitAsync(CancellationToken cancellationToken) => await dataContext.SaveChangesAsync(cancellationToken);

        public void Rollback() => dataContext.Dispose();

        public async Task RollbackAsync() => await dataContext.DisposeAsync();
    }
}