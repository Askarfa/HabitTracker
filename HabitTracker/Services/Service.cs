using HabitTracker.Repositories;
using System.Linq.Expressions;

namespace HabitTracker.Services;

public class Service<T> : IService<T> where T : class
{
    protected readonly IRepository<T> _repository;

    public Service(IRepository<T> repository)
    {
        _repository = repository;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Invalid ID", nameof(id));

        return await _repository.GetByIdAsync(id);
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));

        return await _repository.FindAsync(predicate);
    }

    public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));

        return await _repository.FirstOrDefaultAsync(predicate);
    }

    public virtual async Task<T> CreateAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        return await _repository.AddAsync(entity);
    }

    public virtual async Task UpdateAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        await _repository.UpdateAsync(entity);
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Invalid ID", nameof(id));

        var entity = await _repository.GetByIdAsync(id);
        if (entity != null)
        {
            await _repository.DeleteAsync(entity);
        }
    }

    public virtual async Task<bool> ExistsAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Invalid ID", nameof(id));

        return await _repository.GetByIdAsync(id) != null;
    }
}