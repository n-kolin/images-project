using FilesProj.Core.DTOs;
using FilesProj.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = FilesProj.Core.Entities.File;

namespace FilesProj.Core.IRepositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();

        Task<User> GetByIdAsync(int id);
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<File>> GetFilesAsync(int id);
        Task<IEnumerable<Folder>> GetFoldersAsync(int id);

        Task<User> AddAsync(User user);

        Task<User> UpdateAsync(int id, User user);
        Task<User> AddFolderAsync(int id, Folder folder);
        Task<User> AddFileAsync(int id, File file);

        Task<User> DeleteAsync(int id);
    }
}
