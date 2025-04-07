using FilesProj.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = FilesProj.Core.Entities.File;

namespace FilesProj.Core.IRepositories
{
    public interface IFileRepository
    {
        Task<IEnumerable<File>> GetAllAsync();

        Task<File> GetByIdAsync(int id);

        Task<File> AddAsync(File file);

        Task<File> UpdateAsync(int id, File file);

        Task<File> DeleteAsync(int id);
        Task<File> SoftDeleteAsync(int id);
    }
}
