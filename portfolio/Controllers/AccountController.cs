using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using portfolio.Models;

public class AccountController : Controller
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    // Show the login form (GET request)
    public IActionResult Login()
    {
        return View();
    }

    // Handle form submission (POST request)
    [HttpPost]
    public IActionResult Authenticate(string username, string password)
    {
        // Check credentials with your user service
        var user = _userService.Authenticate(username, password);

        if (user == null)
        {
            // If authentication fails, return to login page with error message
            ModelState.AddModelError("", "Invalid username or password");
            return View("Login");
        }

        // Authentication successful, create claims and sign in
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        // Sign the user in with a cookie
        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        // Redirect to a secure page after successful login
        return RedirectToAction("", "Admin");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        // Sign out the user by clearing the authentication cookie
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        // After logout, redirect the user to the login page or home page
        return RedirectToAction("Login", "Account"); // Or to any page you prefer
    }
}
