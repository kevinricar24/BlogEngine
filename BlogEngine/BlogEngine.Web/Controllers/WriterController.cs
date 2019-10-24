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
    public class WriterController : Controller
    {
        private readonly IUnitOfWork uow;
        private PostActions _postActions;

        public WriterController(IUnitOfWork unityOfWork)
        {
            uow = unityOfWork;
            _postActions = new PostActions(uow);
        }

        public async Task<IActionResult> Index()
        {
            return View(await _postActions.GetPostsAsync((int)Roles.Writer));
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IEnumerable<Post> posts = await _postActions.GetPostsAsync((int)Roles.Writer, id);
            var post = posts.FirstOrDefault();
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string command, [Bind("Id,Title,Body,ImageUrl")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.Author = uow.PersonRepository.Get(x => x.RoleId == (int)Roles.Writer).FirstOrDefault();
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
                uow.PostRepository.Insert(post);
                await uow.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IEnumerable<Post> posts = await _postActions.GetPostsAsync((int)Roles.Writer, id);
            var post = posts.FirstOrDefault();
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

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

                    uow.PostRepository.Update(post);
                    await uow.SaveAsync();
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

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await uow.PostRepository.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var post = await uow.PostRepository.GetByIdAsync(id);
            uow.PostRepository.Delete(post);
            await uow.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(long id)
        {
            var exist = uow.PostRepository.GetById(id);
            return exist == null ? false : true;
        }
    }
}
