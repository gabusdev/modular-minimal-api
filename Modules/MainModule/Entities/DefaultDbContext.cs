using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Modules.MainModule.Entities
{
    public partial class DefaultDbContext : DbContext
    {
        public DefaultDbContext()
        {
        }

        public DefaultDbContext(DbContextOptions<DefaultDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Todo> Todos { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        // public virtual DbSet<UserRol> UserRols { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Todo>(entity =>
            {
                entity.ToTable("TodoItem");

                entity.HasIndex(e => e.UserId, "Fk_User");

                entity.Property(e => e.Id)
                    .HasMaxLength(40)
                    .HasColumnName("id");

                entity.Property(e => e.IsDone).HasColumnName("isDone");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.UserId)
                    .HasMaxLength(40)
                    .HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Todos)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("Fk_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => new { e.Username, e.Mail }, "Unique_Username_Mail")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasMaxLength(40)
                    .HasColumnName("id");

                entity.Property(e => e.Mail)
                    .HasMaxLength(30)
                    .HasColumnName("mail");

                entity.Property(e => e.Pass)
                    .HasMaxLength(500)
                    .HasColumnName("pass");

                entity.Property(e => e.Username)
                    .HasMaxLength(20)
                    .HasColumnName("username");

                entity.HasMany(u => u.Roles)
                    .WithMany(r => r.Users);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
