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
        private readonly IUnitOfWork uow;
        private PostActions _postActions;

        public EditorController(IUnitOfWork unityOfWork)
        {
            uow = unityOfWork;
            _postActions = new PostActions(uow);
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

            IEnumerable<Post> posts = await _postActions.GetPostsAsync((int)Roles.Editor, id);
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
            var post = await uow.PostRepository.GetByIdAsync(id);
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
                    post.Approver = uow.PersonRepository.Get(x => x.RoleId == (int)Roles.Editor).FirstOrDefault();
                    post.ApproverId = post.Approver.Id;
                    post.ApprovalDateTime = currentDateTime;
                    post.IsPublished = true;
                }

                try
                {
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

        private bool PostExists(long id)
        {
            var exist = uow.PostRepository.GetById(id);
            return exist == null ? false : true;
        }
    }
}
