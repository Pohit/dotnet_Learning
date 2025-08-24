using System.Security.Claims;
using EmployeePerformance.Web.Data;
using EmployeePerformance.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeePerformance.Web.Controllers;

[Authorize]
public class FeedbackController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailSender _emailSender;

    public FeedbackController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
    {
        _db = db;
        _userManager = userManager;
        _emailSender = emailSender;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int goalId, string comments)
    {
        var goal = await _db.Goals.Include(g => g.User).FirstOrDefaultAsync(g => g.Id == goalId);
        if (goal == null) return NotFound();

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var feedback = new Feedback
        {
            GoalId = goalId,
            SubmittedByUserId = currentUserId,
            Comments = comments
        };
        _db.Feedbacks.Add(feedback);
        await _db.SaveChangesAsync();

        // Notify manager by email if available
        if (goal.User?.ManagerId is not null)
        {
            var manager = await _userManager.FindByIdAsync(goal.User.ManagerId);
            if (manager?.Email is not null)
            {
                await _emailSender.SendEmailAsync(manager.Email, "New Feedback Submitted", $"A new feedback was submitted for {goal.Title}.");
            }
        }
        return RedirectToAction("Details", "Goals", new { id = goalId });
    }
}

