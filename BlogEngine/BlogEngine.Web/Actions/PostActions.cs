using BlogEngine.BusinessLogic.Models;
using BlogEngine.DataAccessLayer;
using BlogEngine.Web.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogEngine.Web.Actions
{
    public class PostActions
    {
        private readonly BlogEngineContext _context;

        public PostActions(BlogEngineContext context)
        {
            _context = context;
        }

        public async Task<List<Post>> GetPostsAsync(int role, long? id = null)
        {
            List<Post> Posts = null;

            if(role == (int)Roles.Writer)
            {
                if (id != null)
                {
                    Posts = await _context.Post.Where(x => x.Id == id).ToListAsync();
                }
                else
                {
                    Posts = await _context.Post.ToListAsync();
                }
            }
            else if (role == (int)Roles.Editor)
            {
                if (id != null)
                {
                    Posts = await _context.Post.Where(x => x.PendingToApprove == true && x.Id == id).ToListAsync();
                }
                else
                {
                    Posts = await _context.Post.Where(x => x.PendingToApprove == true).ToListAsync();
                }
            }
            else if (role == (int)Roles.None)
            {
                if (id != null)
                {
                    Posts = await _context.Post.Where(x => x.IsPublished == true && x.Id == id).ToListAsync();
                }
                else
                {
                    Posts = await _context.Post.Where(x => x.IsPublished == true).ToListAsync();
                }
            }

            foreach (var item in Posts)
            {
                item.Author = _context.Person.Where(x => x.Id == item.AuthorId).FirstOrDefault();
                item.Approver = _context.Person.Where(x => x.Id == item.ApproverId).FirstOrDefault();
            }

            return Posts;
        }

    }
}
