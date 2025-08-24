using System.Security.Claims;
using EmployeePerformance.Web.Data;
using EmployeePerformance.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeePerformance.Web.Controllers;

[Authorize]
public class GoalsController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public GoalsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var goals = await _db.Goals.Where(g => g.UserId == userId)
            .Include(g => g.Feedbacks)
            .OrderByDescending(g => g.Id)
            .ToListAsync();
        return View(goals);
    }

    public IActionResult Create()
    {
        return View(new Goal { DueDate = DateTime.UtcNow.AddMonths(1) });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Goal goal)
    {
        if (!ModelState.IsValid) return View(goal);
        goal.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        _db.Goals.Add(goal);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var goal = await _db.Goals.Include(g => g.Feedbacks).ThenInclude(f => f.SubmittedBy)
            .FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId);
        if (goal == null) return NotFound();
        return View(goal);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var goal = await _db.Goals.FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId);
        if (goal == null) return NotFound();
        return View(goal);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Goal model)
    {
        if (id != model.Id) return BadRequest();
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var goal = await _db.Goals.FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId);
        if (goal == null) return NotFound();
        if (!ModelState.IsValid) return View(model);
        goal.Title = model.Title;
        goal.Description = model.Description;
        goal.DueDate = model.DueDate;
        goal.Status = model.Status;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}

