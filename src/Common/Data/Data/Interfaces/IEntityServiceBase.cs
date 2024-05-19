namespace Data.Interfaces
{
    public interface IEntityServiceBase<TEntity> where TEntity : class
    {
        Task<TEntity> GetById(object id, CancellationToken cancellationToken);
    }
}
