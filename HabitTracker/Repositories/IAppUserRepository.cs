using HabitTracker.Entities;

namespace HabitTracker.Repositories;

public interface IAppUserRepository : IRepository<AppUser>
{
    Task<AppUser?> GetByIdWithHabitsAsync(string id);
}