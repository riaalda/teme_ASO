using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;

namespace TodoApp.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly TodoDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;

    public AdminController(TodoDbContext db, UserManager<IdentityUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var users = _userManager.Users.ToList();
        var totalTodos = await _db.TodoItems.CountAsync();
        var completedTodos = await _db.TodoItems.CountAsync(t => t.IsComplete);

        ViewBag.Users = users;
        ViewBag.TotalTodos = totalTodos;
        ViewBag.CompletedTodos = completedTodos;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        // Delete their todos first
        var todos = _db.TodoItems.Where(t => t.UserId == userId);
        _db.TodoItems.RemoveRange(todos);
        await _db.SaveChangesAsync();

        await _userManager.DeleteAsync(user);
        TempData["SuccessMessage"] = $"Utilizatorul {user.UserName} a fost sters.";
        return RedirectToAction(nameof(Index));
    }
}