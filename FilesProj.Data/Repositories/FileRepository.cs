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
    public class FileRepository(DataContext context):IFileRepository
    {
        private readonly DbSet<File> _files = context.FilesList;

        public async Task<IEnumerable<File>> GetAllAsync()
        {
            return await _files
            .Where(f => !f.IsDeleted)
                .ToListAsync();
        }

        public async Task<File> GetByIdAsync(int id)
        { 
            return await _files
            .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted);
        }
        public async Task<File> AddAsync(File file)
        {
            var f = await _files.AddAsync(file);
            return f.Entity;
        }

        public async Task<File> UpdateAsync(int id, File file)
        {
            var f = await _files.FindAsync(id);

            f.Name = file.Name;
            f.Type = file.Type;
            f.Size = file.Size;
            f.FolderId = file.FolderId;

            f.UpdatedAt = DateTime.Now;

            //_files.Update(f);
            return f;
        }

        public async Task<File> DeleteAsync(int id)
        {
            var f = await _files.FindAsync(id);
            await Task.Run(() => _files.Remove(f));
            return f;
        }

        public async Task<File> SoftDeleteAsync(int id)
        {
            var f = await _files.FindAsync(id);

            f.IsDeleted = true;
            f.UpdatedAt = DateTime.Now;

            //_files.Update(f);
            return f;
        }
    }
}
