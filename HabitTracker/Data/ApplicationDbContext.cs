using HabitTracker.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace HabitTracker.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Habit> Habits => Set<Habit>();
    public DbSet<HabitLog> HabitLogs => Set<HabitLog>();
    public DbSet<Goal> Goals => Set<Goal>();
    public DbSet<PredictionModel> PredictionModels => Set<PredictionModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Habit>()
            .HasOne(h => h.User)
            .WithMany(u => u.Habits)
            .HasForeignKey(h => h.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<HabitLog>()
            .HasOne(l => l.Habit)
            .WithMany(h => h.Logs)
            .HasForeignKey(l => l.HabitId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<HabitLog>()
            .HasOne(l => l.User)
            .WithMany(u => u.HabitLogs)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Goal>()
            .HasOne(g => g.Habit)
            .WithMany(h => h.Goals)
            .HasForeignKey(g => g.HabitId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PredictionModel>()
            .HasOne(p => p.Habit)
            .WithMany(h => h.Predictions)
            .HasForeignKey(p => p.HabitId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}