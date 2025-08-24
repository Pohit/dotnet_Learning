using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeePerformance.Web.Models;

public class Feedback
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int GoalId { get; set; }

    [ForeignKey(nameof(GoalId))]
    public Goal? Goal { get; set; }

    [Required]
    public string SubmittedByUserId { get; set; } = string.Empty;

    [ForeignKey(nameof(SubmittedByUserId))]
    public ApplicationUser? SubmittedBy { get; set; }

    [Required, MaxLength(2000)]
    public string Comments { get; set; } = string.Empty;

    public DateTime SubmittedAtUtc { get; set; } = DateTime.UtcNow;
}

