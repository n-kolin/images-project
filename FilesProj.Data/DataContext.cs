using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilesProj.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using File = FilesProj.Core.Entities.File;
using MySql.EntityFrameworkCore.Extensions;
using static System.Net.Mime.MediaTypeNames;
using FilesProj.Core.Entities;

namespace FilesProj.Data
{
    public class DataContext:DbContext
    {
        public DbSet<User> UsersList { get; set; }
        public DbSet<Folder> FoldersList { get; set; }
        public DbSet<File> FilesList { get; set; }
        public DbSet<Frame> FramesList { get; set; }

        public DbSet<Role> RolesList { get; set; }
        public DbSet<Permission> permissionsList { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Role>()
            //    .HasMany(r => r.Permissions)
            //    .WithMany(p => p.Roles)
            //    .UsingEntity(j => j.ToTable("RolePermissions"));

            //// הגדרת קשרים מרובי-לרבים עבור תמונות ותגיות
            //modelBuilder.Entity<File>()
            //    .HasMany(i => i.Tags)
            //    .WithMany(t => t.Files)
            //    .UsingEntity(j => j.ToTable("ImageTags"));

            //// הגדרת קשרים מרובי-לרבים עבור מסגרות ותגיות
            //modelBuilder.Entity<Frame>()
            //    .HasMany(f => f.Tags)
            //    .WithMany(t => t.Frames);
            // הגדרה של קשרים בין טבלאות
            modelBuilder.Entity<User>()
                .HasMany(u => u.Files)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.CreatedBy)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Folders)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.CreatedBy).OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Folder>()
                .HasMany(f => f.SubFolders)
                .WithOne(f => f.Parent)
                .HasForeignKey(f => f.ParentId)
                  .OnDelete(DeleteBehavior.Restrict); ;

            modelBuilder.Entity<Folder>()
                .HasMany(f => f.Files)
                .WithOne(f => f.Folder)
                .HasForeignKey(f => f.FolderId)
                  .OnDelete(DeleteBehavior.Restrict); ;

            modelBuilder.Entity<File>()
                .HasOne(f => f.Folder)
                .WithMany(f => f.Files)
                .HasForeignKey(f => f.FolderId)
                  .OnDelete(DeleteBehavior.Restrict); ;

            modelBuilder.Entity<File>()
                .HasMany(f => f.Tags)
                .WithMany(t => t.Files)
                ;

            modelBuilder.Entity<Frame>()
                .HasMany(f => f.Tags)
                .WithMany(t => t.Frames);

            modelBuilder.Entity<Role>()
                .HasMany(r => r.Permissions)
                .WithMany(p => p.Roles);

            modelBuilder.Entity<FramedImg>()
                .HasOne(fi => fi.Frame)
                .WithMany()
                .HasForeignKey(fi => fi.FrameId);

            modelBuilder.Entity<FramedImg>()
                .HasOne(fi => fi.Iamge)
                .WithMany()
                .HasForeignKey(fi => fi.ImageId);


        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                
                var connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");    
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            }
        }

    }
}
