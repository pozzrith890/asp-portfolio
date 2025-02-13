using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using portfolio.Data;
using portfolio.Models;

namespace portfolio.Controllers
{

    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProjectsController(MyDbContext context,IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            return View(await _context.Project.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .FirstOrDefaultAsync(m => m.id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {
            if (ModelState.IsValid)
            {
                if (project.imgs == null || project.imgs.Length == 0)
                {
                    ModelState.AddModelError("img", "Please upload a Photo.");
                    return View(project);
                }

                try
                {
                    string photoNames = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(project.imgs.FileName);
                    string photoFullPaths = Path.Combine(_environment.WebRootPath, "project_photo", photoNames);

                    await using (var stream = System.IO.File.Create(photoFullPaths))
                    {
                        await project.imgs.CopyToAsync(stream);
                    }

                    project.photo = photoNames;
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that might occur during file upload
                    ModelState.AddModelError("img", $"An error occurred: {ex.Message}");
                    return View(project);
                }

                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Project project)
        {
            if (id != project.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    if (project.imgs == null || project.imgs.Length == 0)
                    {
                        ModelState.AddModelError("img", "Please upload a Photo.");
                        return View(project);
                    }

                    string photoNames = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(project.imgs.FileName);
                    string photoFullPaths = Path.Combine(_environment.WebRootPath, "project_photo", photoNames);

                    await using (var stream = System.IO.File.Create(photoFullPaths))
                    {
                        await project.imgs.CopyToAsync(stream);
                    }

                    project.photo = photoNames;

                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .FirstOrDefaultAsync(m => m.id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Project.FindAsync(id);
            if (project != null)
            {
                _context.Project.Remove(project);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.id == id);
        }
    }
}
