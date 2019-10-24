using BlogEngine.BusinessLogic.Models;
using BlogEngine.DataAccessLayer;
using BlogEngine.Web.Actions;
using BlogEngine.Web.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogEngine.Web.Controllers
{
    public class EditorController : Controller
    {
        private readonly BlogEngineContext _context;
        private PostActions _postActions;

        public EditorController(BlogEngineContext context)
        {
            _context = context;
            _postActions = new PostActions(_context);
        }

        public async Task<IActionResult> Index()
        {
            return View(await _postActions.GetPostsAsync((int)Roles.Editor));
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<Post> posts = await _postActions.GetPostsAsync((int)Roles.Editor, id);
            var post = posts.FirstOrDefault();
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, string command)
        {
            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                DateTime currentDateTime = DateTime.Now;
                post.PendingToApprove = false;
                post.LastUpdated = currentDateTime;

                if (command.Equals("Approve"))
                {
                    post.Approver = _context.Person.Where(x => x.RoleId == (int)Roles.Editor).FirstOrDefault();
                    post.ApproverId = post.Approver.Id;
                    post.ApprovalDateTime = currentDateTime;
                    post.IsPublished = true;
                }

                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
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
            return View(post);
        }

        private bool PostExists(long id)
        {
            return _context.Post.Any(e => e.Id == id);
        }
    }
}
