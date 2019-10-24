using BlogEngine.BusinessLogic.Models;
using BlogEngine.DataAccessLayer;
using BlogEngine.Web.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BlogEngine.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUnitOfWork uow;

        public LoginController(IUnitOfWork unityOfWork)
        {
            uow = unityOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("UserName,Pass")] Person person)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(person.UserName) && !string.IsNullOrEmpty(person.Pass))
                {
                    var persons = await uow.PersonRepository.GetAsync();
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
            return RedirectToAction("Index", "Login");
        }
    }
}
