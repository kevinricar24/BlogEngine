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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.AuthorEmail)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.AuthorName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdated).HasColumnType("datetime");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Comment__PostId__2C3393D0");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastUpdated).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Pass)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserName).HasMaxLength(50);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Person)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Person__RoleId__25869641");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.ApprovalDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.ImageUrl).HasColumnName("ImageURL");

                entity.Property(e => e.LastUpdated).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Approver)
                    .WithMany(p => p.PostApprover)
                    .HasForeignKey(d => d.ApproverId)
                    .HasConstraintName("FK__Post__ApproverId__29572725");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.PostAuthor)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Post__AuthorId__286302EC");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdated).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
