using EmployeePerformance.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeePerformance.Web.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Goal> Goals => Set<Goal>();
    public DbSet<Feedback> Feedbacks => Set<Feedback>();
    public DbSet<KpiMetric> KpiMetrics => Set<KpiMetric>();
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>()
            .HasOne(u => u.Manager)
            .WithMany(u => u!.DirectReports)
            .HasForeignKey(u => u.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Goal>()
            .HasOne(g => g.User)
            .WithMany(u => u.Goals)
            .HasForeignKey(g => g.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<KpiMetric>()
            .HasOne(k => k.User)
            .WithMany(u => u.KpiMetrics)
            .HasForeignKey(k => k.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<KpiMetric>()
            .Property(k => k.TargetValue)
            .HasPrecision(18, 2);

        builder.Entity<KpiMetric>()
            .Property(k => k.ActualValue)
            .HasPrecision(18, 2);
    }
}
