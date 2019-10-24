using BlogEngine.BusinessLogic.Models;
using BlogEngine.DataAccessLayer.Repositories;
using System.Threading.Tasks;

namespace BlogEngine.DataAccessLayer
{
    public interface IUnitOfWork
    {
        GenericRepository<Post> PostRepository { get; }
        GenericRepository<Comment> CommentRepository { get; }
        GenericRepository<Role> RoleRepository { get; }
        GenericRepository<Person> PersonRepository { get; }
        void Save();
        Task SaveAsync();
    }
}
