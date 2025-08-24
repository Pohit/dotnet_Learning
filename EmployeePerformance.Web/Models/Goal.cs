using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeePerformance.Web.Models;

public class Goal
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    // Owner
    [Required]
    public string UserId { get; set; } = string.Empty;

    [ForeignKey(nameof(UserId))]
    public ApplicationUser? User { get; set; }

    // Status
    [Required, MaxLength(30)]
    public string Status { get; set; } = "Open"; // Open, InProgress, Completed, Archived

    public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}

