using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeePerformance.Web.Models;

public class ApplicationUser : IdentityUser
{
    [MaxLength(100)]
    public string? FullName { get; set; }

    // Self-referencing manager relationship
    public string? ManagerId { get; set; }

    [ForeignKey(nameof(ManagerId))]
    public ApplicationUser? Manager { get; set; }

    public ICollection<ApplicationUser>? DirectReports { get; set; }

    public ICollection<Goal> Goals { get; set; } = new List<Goal>();
    public ICollection<KpiMetric> KpiMetrics { get; set; } = new List<KpiMetric>();
}

