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
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NPOI.SS.Formula.Functions;
using System.Reflection;
using System.Globalization;

namespace WebApplication4.Controllers
{
    public class SelfStudyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _userManager;

        public SelfStudyController(ApplicationDbContext context, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: SelfStudy
        public async Task<IActionResult> Index() { 
            //Get current users list of modules
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser != null)
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

        public IActionResult CalculateSelfStudyHours()
        {
            //Retrieve information from database Table1
            Table1 t = _context.Table1.FirstOrDefault();

            //Convert string properties to integers
            int Credits = int.Parse(t.Credits.ToString());
            int NumOfWeeks = int.Parse(t.NumOfWeeks.ToString());
            int ClassHoursPerWeek = int.Parse(t.ClassHoursPerWeek.ToString());

            int selfStudyHoursPerWeek = (Credits * 10) / NumOfWeeks - ClassHoursPerWeek;
            ViewBag.SelfStudyHours = selfStudyHoursPerWeek.ToString();
            return View();
        }
        // GET: SelfStudy/Create
        public IActionResult Create()
        {
            return View();
        }
       
        // POST: SelfStudy/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,NumOfWeeks,Credits")] Table1 table1)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);

                if (currentUser != null)
                {
                    table1.UserId = currentUser.Id; //Set UserId to currently signed-in user's Id

                    _context.Add(table1);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
            }
            
            return View(table1);
        }
       

        // GET: SelfStudy/Edit/5
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

        // POST: SelfStudy/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,NumOfWeeks")] Table1 table1)
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

        // GET: SelfStudy/Delete/5
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

        // POST: SelfStudy/Delete/5
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
