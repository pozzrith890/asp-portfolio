using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using portfolio.Data;
using portfolio.Models;

namespace portfolio.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly MyDbContext _context;
        private readonly IWebHostEnvironment environment;


        public HomeController(ILogger<HomeController> logger,MyDbContext context, IWebHostEnvironment environment)
        {
            _logger = logger;
            _context = context;
            this.environment = environment;

        }
        public IActionResult Index()
        {
            ViewData["teamWork"] = _context.Members.Count();
            ViewData["project"] = _context.Project.Count();
            ViewData["happyClient"] = _context.Comment.Count();
            return View();
        }

        public async Task<IActionResult> Create(Comment comment)
        {
            if (ModelState.IsValid)
            {
                if (comment.img == null)
                {
                    ModelState.AddModelError("img", "please choose profile");
                    return RedirectToAction("Index", "Home", new { comment = comment });
                }

                string photoName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(comment.img.FileName);
                string photoFullPath = Path.Combine(environment.WebRootPath, "comment_photo", photoName);
                await using (var stream = System.IO.File.Create(photoFullPath))
                {
                    await comment.img.CopyToAsync(stream);
                }

                comment.photo = photoName;

                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(comment);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
