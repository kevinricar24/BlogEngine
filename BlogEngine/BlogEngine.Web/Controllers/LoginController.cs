using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogEngine.BusinessLogic.Models;
using BlogEngine.DataAccessLayer;
using BlogEngine.Web.Enums;

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
                            if (item.RoleId == (int)Roles.Writer)
                            {
                                return RedirectToAction("Index", "Writer");
                            }
                            else if (item.RoleId == (int)Roles.Editor)
                            {
                                return RedirectToAction("Index", "Editor");
                            }
                        }
                    }
                }
            }
            return View();
        }
    }
}
