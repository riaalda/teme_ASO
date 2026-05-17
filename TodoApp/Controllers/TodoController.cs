using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Controllers;

[Authorize]
public class TodoController : Controller
{
    private readonly TodoDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;

    public TodoController(TodoDbContext db, UserManager<IdentityUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        var todos = await _db.TodoItems
            .Where(t => t.UserId == userId)
            .ToListAsync();
        return View(todos);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string title, string? description, DateTime? dueDate)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            TempData["ErrorMessage"] = "Titlul nu poate fi gol.";
            return RedirectToAction(nameof(Index));
        }

        var userId = _userManager.GetUserId(User);
        _db.TodoItems.Add(new TodoItem
        {
            Title = title.Trim(),
            Description = description,
            DueDate = dueDate,
            UserId = userId!
        });
        await _db.SaveChangesAsync();
        TempData["SuccessMessage"] = "Task adaugat!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Toggle(int id)
    {
        var userId = _userManager.GetUserId(User);
        var todo = await _db.TodoItems.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if (todo == null) return NotFound();

        todo.IsComplete = !todo.IsComplete;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = _userManager.GetUserId(User);
        var todo = await _db.TodoItems.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if (todo == null) return NotFound();

        _db.TodoItems.Remove(todo);
        await _db.SaveChangesAsync();
        TempData["SuccessMessage"] = "Task sters.";
        return RedirectToAction(nameof(Index));
    }
}