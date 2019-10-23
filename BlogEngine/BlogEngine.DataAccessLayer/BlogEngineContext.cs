using BlogEngine.BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogEngine.DataAccessLayer
{
    public partial class BlogEngineContext : DbContext
    {
        public BlogEngineContext()
        {
        }

        public BlogEngineContext(DbContextOptions<BlogEngineContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<Role> Role { get; set; }
    }
}
