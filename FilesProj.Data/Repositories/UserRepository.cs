
using FilesProj.Core.Entities;
using FilesProj.Core.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = FilesProj.Core.Entities.File;

namespace FilesProj.Data.Repositories
{
    public class UserRepository(DataContext context) : IUserRepository
    {
        private readonly DbSet<User> _users = context.UsersList;

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _users.ToListAsync();
            //return await _users.Include(u=>u.Files).ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _users.Include("Files").Include("Folders").FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<User> GetByEmailAsync(string email)
        {
            return await _users.Where(u => u.Email.Equals(email)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<File>> GetFilesAsync(int id)
        {
            return await _users
                .Where(u => u.Id == id)
                .SelectMany(u => u.Files)
                .Where(f => !f.IsDeleted)
                .ToListAsync();
        }
        public async Task<IEnumerable<Folder>> GetFoldersAsync(int id)
        {
            return await _users
                .Where(u => u.Id == id)
                .SelectMany(u => u.Folders)
                .Where(f => !f.IsDeleted)
                .ToListAsync();
        }

        public async Task<User> AddAsync(User user)
        {
            var u = await _users.AddAsync(user);
            return u.Entity;
        }

        public async Task<User> UpdateAsync(int id, User user)
        {
            var u = await _users.FindAsync(id);

            u.Name = user.Name;
            u.Email = user.Email;
            u.Password = user.Password;

            u.UpdatedAt = DateTime.Now;

            //_users.Update(u);
            return u;
        }
        public async Task<User> AddFolderAsync(int id, Folder folder)
        {
            var u = await _users.FindAsync(id);

            u.Folders.Add(folder);

            u.UpdatedAt = DateTime.Now;

            //_users.Update(u);
            return u;
        }
        public async Task<User> AddFileAsync(int id, File file)
        {
            var u = await _users.FindAsync(id);

            u.Files.Add(file);

            u.UpdatedAt = DateTime.Now;

            //_users.Update(u);
            return u;
        }

        public async Task<User> DeleteAsync(int id)
        {
            var u = await _users.FindAsync(id);
            await Task.Run(() => _users.Remove(u));
            return u;
        }

    }
}
