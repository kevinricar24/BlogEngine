using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogEngine.BusinessLogic.Models;
using BlogEngine.DataAccessLayer;

namespace BlogEngine.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly BlogEngineContext _context;

        public LoginController(BlogEngineContext context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult Index()
        {
            return View();
        }

        // GET: Login/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .Include(p => p.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: Login/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Role, "Id", "Id");
            return View();
        }

        // POST: Login/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("UserName,Pass")] Person person)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(person.UserName) && !string.IsNullOrEmpty(person.Pass))
                {
                    var persons = await _context.Person.ToListAsync();
                    foreach (var item in persons)
                    {
                        if ((item.UserName == person.UserName || item.Email == person.UserName) && item.Pass == person.Pass)
                        {
                            if (item.Role.Name == "User Writer")
                            {
                                //Go To Writer Panel
                                return RedirectToAction(nameof(Index));
                            }
                            else if (item.Role.Name == "User Editor")
                            {
                                //Go To Editor Panel
                                return RedirectToAction(nameof(Index));
                            }
                        }
                    }
                }
            }
            return View();
        }

        // GET: Login/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Role, "Id", "Id", person.RoleId);
            return View(person);
        }

        // POST: Login/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,RoleId,Name,UserName,Email,Pass,CreationDate,LastUpdated")] Person person)
        {
            if (id != person.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.Id))
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
            ViewData["RoleId"] = new SelectList(_context.Role, "Id", "Id", person.RoleId);
            return View(person);
        }

        // GET: Login/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .Include(p => p.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: Login/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var person = await _context.Person.FindAsync(id);
            _context.Person.Remove(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(long id)
        {
            return _context.Person.Any(e => e.Id == id);
        }
    }
}
