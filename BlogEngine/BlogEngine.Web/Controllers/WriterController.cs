using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogEngine.BusinessLogic.Models;
using BlogEngine.DataAccessLayer;
using BlogEngine.Web.Actions;
using BlogEngine.Web.Enums;

namespace BlogEngine.Web.Controllers
{
    public class WriterController : Controller
    {
        private readonly BlogEngineContext _context;
        private PostActions _postActions;

        public WriterController(BlogEngineContext context)
        {
            _context = context;
            _postActions = new PostActions(_context);
        }

        // GET: Writer
        public async Task<IActionResult> Index()
        {
            return View(await _postActions.GetPostsAsync((int)Roles.Writer));
        }

        // GET: Writer/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<Post> posts = await _postActions.GetPostsAsync((int)Roles.Writer, id);
            var post = posts.FirstOrDefault();
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Writer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Writer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string command, [Bind("Id,Title,Body,ImageUrl")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.Author = _context.Person.Where(x => x.RoleId == (int)Roles.Writer).FirstOrDefault();
                post.AuthorId = post.Author.Id;
                DateTime currentDateTime = DateTime.Now;
                post.CreationDate = currentDateTime;
                post.LastUpdated = currentDateTime;

                if (command.Equals("Publish"))
                {
                    post.PendingToApprove = true;
                }
                else if(command.Equals("Draft"))
                {
                    post.PendingToApprove = false;
                }
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Writer/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<Post> posts = await _postActions.GetPostsAsync((int)Roles.Writer, id);
            var post = posts.FirstOrDefault();
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Writer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, string command, [Bind("Id,Title,Body,ImageUrl,AuthorId,PendingToApprove,ApproverId,ApprovalDateTime,IsPublished,CreationDate")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(command.Equals("Publish"))
                    {
                        post.PendingToApprove = true;
                        post.IsPublished = false;
                    }
                    post.LastUpdated = DateTime.Now;
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

        // GET: Writer/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Writer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var post = await _context.Post.FindAsync(id);
            _context.Post.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(long id)
        {
            return _context.Post.Any(e => e.Id == id);
        }
    }
}
