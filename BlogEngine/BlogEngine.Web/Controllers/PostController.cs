using BlogEngine.DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BlogEngine.Web.Controllers
{
    public class PostController : Controller
    {
        private readonly BlogEngineContext _context;

        public PostController(BlogEngineContext context)
        {
            _context = context;
        }

        // GET: Post
        public async Task<IActionResult> Index()
        {
            var PostsAsync = await _context.Post.Where(x => x.IsPublished == true).ToListAsync();
            foreach (var item in PostsAsync)
            {
                item.Author = _context.Person.Where(x => x.Id == item.AuthorId).FirstOrDefault();
                item.Approver = _context.Person.Where(x => x.Id == item.ApproverId).FirstOrDefault();
            }
            return View(PostsAsync);
        }

        // GET: Post/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post.FirstOrDefaultAsync(m => m.Id == id);
            post.Comment = _context.Comment.Where(x => x.PostId == id).ToList();
            post.Author = _context.Person.Where(x => x.Id == post.AuthorId).FirstOrDefault();
            post.Approver = _context.Person.Where(x => x.Id == post.ApproverId).FirstOrDefault();
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }
    }
}
