using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using portfolio.Data;
using portfolio.Models;

namespace portfolio.Controllers
{

    public class MyViewModel
    {
        public List<Member> Members { get; set; }
        public List<Comment> Comment { get; set; }
        public List<Service> Services { get; set; }
        public List<Skill> Skills { get; set; }
        public List<Project> Projects { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Setting> Settings { get; set; }
    }


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

        [HttpGet("DownloadFile")]
        public IActionResult DownloadFile(string fileName , string name)
        {
            // Ensure the fileName is not null or empty
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("File name is required.");
            }

            // Set the path to the file inside the wwwroot folder
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "members_cv", fileName);

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(); // If the file does not exist, return 404
            }

            // Return the file for download
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/octet-stream", name + ".pdf"); // Generic MIME type
        }


        // Use async version of Index
        public async Task<IActionResult> Index()
        {
            var members = await _context.Members.ToListAsync();
            var comment = await _context.Comment.ToListAsync();
            var services = await _context.Services.ToListAsync();
            var skills = await _context.Skill.ToListAsync();
            var projects = await _context.Project.ToListAsync();
            var comments = await _context.Comment.ToListAsync();
            var settings = await _context.Settings.ToListAsync();
            var viewModels = new MyViewModel
            {
                Members = members,
                Comment = comment,
                Services = services,
                Skills = skills,
                Projects = projects,
                Comments = comments,
                Settings = settings,
            };

            //return Json(viewModels.Comments);

            ViewData["teamWork"] = _context.Members.Count();
            ViewData["project"] = _context.Project.Count();
            ViewData["happyClient"] = _context.Comment.Count();

            return View(viewModels); // Pass the ViewModel to the view
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
                //return View("../Home/Index",viewModel);
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
