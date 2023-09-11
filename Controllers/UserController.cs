using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using LogReg.Models;

namespace LogReg.Controllers;

public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private MyContext _context;
    public UserController(ILogger<UserController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("")]
    public ViewResult Index()
    {
        return View();
    }

    [SessionCheck]
    [HttpGet("users/success")]
    public ViewResult Success()
    {
        return View("Success");
    }

    [HttpPost("user/create")]
    public IActionResult RegisterUser(User newUser)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        }
        PasswordHasher<User> hasher = new();
        newUser.Password = hasher.HashPassword(newUser, newUser.Password);
        _context.Add(newUser);
        _context.SaveChanges();

        HttpContext.Session.SetInt32("UUID", newUser.UserId);
        return RedirectToAction("Success");
    }

    [HttpPost("users/login")]
    public IActionResult Login(LoginUser userSubmission)
    {
        if (ModelState.IsValid)
        {
            return View("Success");
        }
        User? userInDb = _context.Users.FirstOrDefault(u => u.Email == userSubmission.LogEmail);
        if (userInDb == null)
        {
            ModelState.AddModelError("LogPassword", "Invalid Credentials");
            return View("Index");
        }
        PasswordHasher<LoginUser> hasher = new();
        PasswordVerificationResult pwCompareResult = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LogPassword);                                    // Result can be compared to 0 for failure        
        if (pwCompareResult == 0)
        {
            ModelState.AddModelError("LogPassword", "Inalid Credentials");
            return View("Index");
        }
        HttpContext.Session.SetInt32("UUID", userInDb.UserId);
        return RedirectToAction("Success");
    }

    [HttpPost("users/logout")]
    public IActionResult LogOut()
    {
        HttpContext.Session.Remove("UUID");
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
