using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CQUCloudWeb.Data;
using CQUCloudWeb.Models;
using Microsoft.AspNetCore.Authorization;
using CQUCloudWeb.Scripts;

namespace CQUCloudWeb
{
    [Authorize]
    public class AppsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Apps
        public async Task<IActionResult> Index()
        {
            string userID = _context.Users.First(u=>u.UserName== User.Identity.Name).Id;
            var applicationDbContext = _context.App.Where(u =>u.UserNameID== userID);
            List<App> apps = await applicationDbContext.ToListAsync();
            foreach (App app in apps)
            {
                app.Algorithm = await _context.Algorithm.FirstOrDefaultAsync(a=>a.AlgorithmId ==app.AlgorithmRefId);
            }
            return View(apps);
        }
        

        // GET: Apps/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var app = await _context.App
                 
                .FirstOrDefaultAsync(m => m.AppId == id);
            if (app == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(app.AlgorithmRefId))
            {
                app.Algorithm = _context.Algorithm.First(a=>a.AlgorithmId==app.AlgorithmRefId);
            }
            return View(app);
        }

        // GET: Apps/Create
        public async Task<IActionResult> Create(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string userID = _context.Users.First(u => u.UserName == User.Identity.Name).Id;
            int accountAppNum = _context.App.Where(a => a.UserNameID == userID).Count();

            if (AppSettingUtility.MaxAccountAppNum <= accountAppNum)
            {
                ViewData["Creatable"] = false;
                ViewData["ErrorMsg"] = "您创建的应用数量已达到上限。";
            }
            else
            {
                ViewData["Creatable"] = true;
                var algorithm = await _context.Algorithm
    .FirstOrDefaultAsync(m => m.AlgorithmId == id);
                if (algorithm == null)
                {
                    return NotFound();
                }
                ViewData["AlgorithmId"] = algorithm.AlgorithmId;
                ViewData["AlgorithmName"] = algorithm.AlgorithmName;
                ViewData["AlgorithmRefId"] = new SelectList(_context.Algorithm.Where(a=>a.AlgorithmId== algorithm.AlgorithmId), "AlgorithmId", "AlgorithmId");
            }


            return View();
        }
        public IActionResult CreateAPI()
        {
            string userID = _context.Users.First(u => u.UserName == User.Identity.Name).Id;
            int accountAppNum = _context.App.Where(a => a.UserNameID == userID).Count();

            if (AppSettingUtility.MaxAccountAppNum <= accountAppNum)
            {
                ViewData["Creatable"] = false;
                ViewData["ErrorMsg"] = "您创建的应用数量已达到上限。";
            }
            else
            {
                ViewData["Creatable"] = true;


            }
            return View();
        }
        // POST: Apps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppId,AppName,UserNameID,AlgorithmRefId,APIKey,CreateTime,LastCallTime,MonthCallCount,State,AIFunction")] App app)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(app.AppName))
                {
                    return NotFound();
                }
                if (app.Algorithm !=null||!string.IsNullOrEmpty(app.AlgorithmRefId))
                {
                    app.AIFunction = AIFunction.Custom;
                }
                string userID = _context.Users.First(u => u.UserName == User.Identity.Name).Id;
                app.UserNameID = userID;
                app.CreateTime = DateTime.Now;
                app.LastCallTime = DateTime.Now;
                app.MonthCallCount = 0;
                app.State = true;
                string rand;
                for (rand = RandomStringBuilder.Create(36);_context.App.Any(a=>a.APIKey== RandomStringBuilder.Create(36));)
                {
                    rand = RandomStringBuilder.Create(36);
                }
                app.APIKey = rand;
                _context.Add(app);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlgorithmId"] = app.AlgorithmRefId;
            var algorithm = await _context.Algorithm
                .FirstOrDefaultAsync(m => m.AlgorithmId == app.AlgorithmRefId);
            ViewData["AlgorithmName"] = algorithm.AlgorithmName;
            ViewData["AlgorithmRefId"] = new SelectList(_context.Algorithm.Where(a => a.AlgorithmId == algorithm.AlgorithmId), "AlgorithmId", "AlgorithmId");
            return View(app);
        }


        // GET: Apps/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string userID = _context.Users.First(u => u.UserName == User.Identity.Name).Id;

            var app = await _context.App
                 
                .FirstOrDefaultAsync(m => m.AppId == id);
            if (app == null || app.UserNameID != userID)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(app.AlgorithmRefId))
            {
                app.Algorithm = _context.Algorithm.First(a => a.AlgorithmId == app.AlgorithmRefId);
            }
            return View(app);
        }

        // POST: Apps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var app = await _context.App.FindAsync(id);
            _context.App.Remove(app);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppExists(string id)
        {
            return _context.App.Any(e => e.AppId == id);
        }
    }
}
