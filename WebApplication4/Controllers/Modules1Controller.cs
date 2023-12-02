using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClassLibrary1;
using WebApplication4.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.Exchange.WebServices.Data;

namespace WebApplication4.Controllers
{
    public class Modules1Controller : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _userManager;

        public Modules1Controller(ApplicationDbContext context, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Modules1
        public async Task<IActionResult> Index()
        {
            //Get current users list of modules
            var currentUser = await _userManager.GetUserAsync(User);

            if(currentUser != null)
            {
                //Filter modules
                var Table1 = await _context.Table1.Where(t => t.UserId == currentUser.Id).ToListAsync();
                return View(Table1);
            }
            else
            {
                return NotFound(); // Or handle accordingly if user not found
            }
        }

        // GET: Table1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Table1 == null)
            {
                return NotFound();
            }

            var table1 = await _context.Table1
                .FirstOrDefaultAsync(m => m.Id == id);
            if (table1 == null)
            {
                return NotFound();
            }

            return View(table1);
        }


        // GET: Modules1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Modules1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Code,Credits,ClassHoursPerWeek,StartDate,NumOfWeeks,HoursWorked,EndDate")] Table1 table1)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);

                if (currentUser != null)
                {
                    table1.UserId= currentUser.Id; //Set UserId to currently signed-in user's Id

                    _context.Add(table1);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                
            }
            return View(table1);
        }

        // GET: Table1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Table1 == null)
            {
                return NotFound();
            }

            var table1 = await _context.Table1.FindAsync(id);
            if (table1 == null)
            {
                return NotFound();
            }
            return View(table1);
        }

        // POST: Table1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Code,Credits,ClassHoursPerWeek,StartDate,NumOfWeeks,HoursWorked,EndDate,SelfStudyHours,RemainingHours,UserId")] Table1 table1)
        {
            if (id != table1.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(table1);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Table1Exists(table1.Id))
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
            return View(table1);
        }

        // GET: Table1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Table1 == null)
            {
                return NotFound();
            }

            var table1 = await _context.Table1
                .FirstOrDefaultAsync(m => m.Id == id);
            if (table1 == null)
            {
                return NotFound();
            }

            return View(table1);
        }

        // POST: Table1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Table1 == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Table1'  is null.");
            }
            var table1 = await _context.Table1.FindAsync(id);
            if (table1 != null)
            {
                _context.Table1.Remove(table1);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool Table1Exists(int id)
        {
            return (_context.Table1?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
