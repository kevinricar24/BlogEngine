using BlogEngine.BusinessLogic.Models;
using BlogEngine.DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BlogEngine.Web.Controllers
{
    public class CommentsController : Controller
    {
        private readonly IUnitOfWork uow;

        public CommentsController(IUnitOfWork unityOfWork)
        {
            uow = unityOfWork;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int postId, string commentAuthor, string commentEmail, string commentBody)
        {
            if (ModelState.IsValid)
            {
                DateTime currentDateTime = DateTime.Now;
                Comment comment = new Comment()
                {
                    PostId = postId,
                    Body = commentBody,
                    AuthorName = commentAuthor,
                    AuthorEmail = commentEmail,
                    CreationDate = currentDateTime,
                    LastUpdated = currentDateTime
                };
                uow.CommentRepository.Insert(comment);
                await uow.SaveAsync();
            }
            return RedirectToAction("Details", "Post", new { id = postId });
        }
    }
}
