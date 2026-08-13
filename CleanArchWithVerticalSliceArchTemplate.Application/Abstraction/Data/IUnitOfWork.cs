namespace CleanArchWithVerticalSliceArchTemplate.Application.Abstraction.Data
{
    public interface IUnitOfWork
    {
        int Commit();
        void Rollback();
        Task<int> CommitAsync(CancellationToken cancellationToken);
        Task RollbackAsync();
    }
}