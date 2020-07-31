using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CQUCloudWeb.Data;
using CQUCloudWeb.Models;
using CQUCloudWeb.Scripts;
using Microsoft.AspNetCore.Authorization;
namespace CQUCloudWeb
{
    [Authorize]
    public class AlgorithmsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AlgorithmsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Algorithms
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Algorithm;
            List<Algorithm> algorithms = await applicationDbContext.OrderByDescending(a => a.MonthCallCount).ToListAsync();
            foreach (Algorithm algorithm in algorithms)
            {
                algorithm.UserNameID = _context.Users.First(u => u.Id == algorithm.UserNameID).UserName;
            }
            return View(algorithms);
        }
        
        public async Task<IActionResult> Search(string keyword)
        {
            var applicationDbContext = _context.Algorithm.Where(a => a.AlgorithmName.Contains(keyword));
            List<Algorithm> algorithms = await applicationDbContext.OrderByDescending(a => a.MonthCallCount).ToListAsync();
            foreach (Algorithm algorithm in algorithms)
            {
                algorithm.UserNameID = _context.Users.First(u=>u.Id==algorithm.UserNameID).UserName;
            }
            return View(algorithms);
        }
        public async Task<IActionResult> Mine()
        {
            string userID = _context.Users.First(u => u.UserName == User.Identity.Name).Id;
            var applicationDbContext = _context.Algorithm.Where(a => a.UserNameID ==userID);
            List<Algorithm> algorithms = await applicationDbContext.OrderByDescending(a => a.MonthCallCount).ToListAsync();
            foreach (Algorithm algorithm in algorithms)
            {
                algorithm.UserNameID = _context.Users.First(u => u.Id == algorithm.UserNameID).UserName;
            }
            return View(algorithms);
        }
        // GET: Algorithms/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var algorithm = await _context.Algorithm
                .FirstOrDefaultAsync(m => m.AlgorithmId == id);
            if (algorithm == null)
            {
                return NotFound();
            }
            algorithm.CodeFile = new byte[0];
            algorithm.UserNameID = _context.Users.First(u => u.Id == algorithm.UserNameID).UserName;

            return View(algorithm);
        }

        // GET: Algorithms/Create
        public IActionResult Create()
        {
            string userID = _context.Users.First(u => u.UserName == User.Identity.Name).Id;
            int algorithmCount = _context.Algorithm.Count();
            int accountAlgorithmCount = _context.Algorithm.Where(a => a.UserNameID == userID).Count();
            
            if (AppSettingUtility.MaxAlgorithmNum<=algorithmCount)
            {
                ViewData["Creatable"] = false;
                ViewData["ErrorMsg"] = "算法数量已达到服务器上限。";
            }
            else if(AppSettingUtility.MaxAccountAlgorithmNum <= accountAlgorithmCount)
            {
                ViewData["Creatable"] = false;
                ViewData["ErrorMsg"] = "您上传的算法数量已达到上限。";
            }
            else
            {
                ViewData["Creatable"] = true;  
            }

            return View();
        }

        // POST: Algorithms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlgorithmId,AlgorithmName,Description,UserNameID,CreateTime,LastCallTime,MonthCallCount,State,DisplayType,EnvironmentType,CodeFile,ContainerID")] Algorithm algorithm)
        {

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(algorithm.AlgorithmName))
                {
                    return NotFound();
                }
                string userID = _context.Users.First(u => u.UserName == User.Identity.Name).Id;
                algorithm.UserNameID = userID;
                algorithm.CreateTime = DateTime.Now;
                algorithm.LastCallTime = DateTime.Now;
                algorithm.MonthCallCount = 0;
                algorithm.State = false;
                _context.Add(algorithm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(algorithm);
        }

        // GET: Algorithms/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string userID = _context.Users.First(u => u.UserName == User.Identity.Name).Id;
            var algorithm = await _context.Algorithm.FindAsync(id);
            if (algorithm == null||userID!=algorithm.UserNameID)
            {
                return NotFound();
            }
            algorithm.UserNameID = _context.Users.First(u => u.Id == algorithm.UserNameID).UserName;
            return View(algorithm);
        }

        // POST: Algorithms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("AlgorithmId,AlgorithmName,Description,UserNameID,CreateTime,LastCallTime,MonthCallCount,State,DisplayType,EnvironmentType,CodeFile,ContainerID")] Algorithm algorithm)
        {
            if (id != algorithm.AlgorithmId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string userID = _context.Users.First(u => u.UserName == User.Identity.Name).Id;
                if (userID != algorithm.UserNameID)
                {
                    return NotFound();
                }
                try
                {
                    _context.Update(algorithm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlgorithmExists(algorithm.AlgorithmId))
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
            return View(algorithm);
        }

        // GET: Algorithms/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string userID = _context.Users.First(u => u.UserName == User.Identity.Name).Id;
            var algorithm = await _context.Algorithm
                .FirstOrDefaultAsync(m => m.AlgorithmId == id);
            if (algorithm == null||userID!=algorithm.UserNameID)
            {
                return NotFound();
            }

            return View(algorithm);
        }

        // POST: Algorithms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var algorithm = await _context.Algorithm.FindAsync(id);
            List<App> apps = await _context.App.Where(a => a.AlgorithmRefId == id).ToListAsync();
            foreach (App app in apps)
            {
                _context.App.Remove(app);
            }
            await _context.SaveChangesAsync();
            _context.Algorithm.Remove(algorithm);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlgorithmExists(string id)
        {
            return _context.Algorithm.Any(e => e.AlgorithmId == id);
        }
    }
}
