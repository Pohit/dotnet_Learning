using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeePerformance.Web.Models;

public class Review
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string EmployeeUserId { get; set; } = string.Empty;

    [ForeignKey(nameof(EmployeeUserId))]
    public ApplicationUser? Employee { get; set; }

    public string? ReviewerUserId { get; set; }

    [ForeignKey(nameof(ReviewerUserId))]
    public ApplicationUser? Reviewer { get; set; }

    [Range(1, 5)]
    public int? Rating { get; set; }

    [MaxLength(3000)]
    public string? Summary { get; set; }

    public DateTime PeriodStartUtc { get; set; }
    public DateTime PeriodEndUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}

