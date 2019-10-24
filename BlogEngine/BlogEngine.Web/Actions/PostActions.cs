using BlogEngine.BusinessLogic.Models;
using BlogEngine.DataAccessLayer;
using BlogEngine.Web.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogEngine.Web.Actions
{
    public class PostActions
    {
        private readonly IUnitOfWork uow;

        public PostActions(IUnitOfWork unityOfWork)
        {
            uow = unityOfWork;
        }

        public async Task<IEnumerable<Post>> GetPostsAsync(int role, long? id = null)
        {
            IEnumerable<Post> Posts = null;
            string includes = "Comment,Author,Approver";

            if (role == (int)Roles.Writer)
            {
                if (id != null)
                {
                    Posts = await uow.PostRepository.GetAsync(x => x.Id == id, includeProperties: includes);
                }
                else
                {
                    Posts = await uow.PostRepository.GetAsync(includeProperties: includes);
                }
            }
            else if (role == (int)Roles.Editor)
            {
                if (id != null)
                {
                    Posts = await uow.PostRepository.GetAsync(x => x.PendingToApprove == true && x.Id == id, includeProperties: includes);
                }
                else
                {
                    Posts = await uow.PostRepository.GetAsync(x => x.PendingToApprove == true, includeProperties: includes);
                }
            }
            else if (role == (int)Roles.None)
            {
                if (id != null)
                {
                    Posts = await uow.PostRepository.GetAsync(x => x.IsPublished == true && x.Id == id, includeProperties: includes);
                }
                else
                {
                    Posts = await uow.PostRepository.GetAsync(x => x.IsPublished == true, includeProperties: includes);
                }
            }

            return Posts;
        }

    }
}
