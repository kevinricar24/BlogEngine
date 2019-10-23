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
        private readonly BlogEngineContext _context;
        private PostActions _postActions;

        public PostController(BlogEngineContext context)
        {
            _context = context;
            _postActions = new PostActions(_context);
        }

        // GET: Post
        public async Task<IActionResult> Index()
        {
            return View(await _postActions.GetPostsAsync((int)Roles.None));
        }

        // GET: Post/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<Post> posts = await _postActions.GetPostsAsync((int)Roles.None, id);
            var post = posts.FirstOrDefault();
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }
    }
}
