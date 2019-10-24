using BlogEngine.BusinessLogic.Models;
using BlogEngine.DataAccessLayer;
using BlogEngine.Web.Actions;
using BlogEngine.Web.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogEngine.Web.Controllers
{
    public class PostController : Controller
    {
        private readonly IUnitOfWork uow;
        private PostActions _postActions;

        public PostController(IUnitOfWork unityOfWork)
        {
            uow = unityOfWork;
            _postActions = new PostActions(uow);
        }

        public async Task<IActionResult> Index()
        {
            return View(await _postActions.GetPostsAsync((int)Roles.None));
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IEnumerable<Post> posts = await _postActions.GetPostsAsync((int)Roles.None, id);
            var post = posts.FirstOrDefault();
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }
    }
}
