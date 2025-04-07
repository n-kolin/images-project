using FilesProj.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesProj.Core.IServices
{
    public interface IFileService
    {
        Task<IEnumerable<FileDto>> GetAllAsync();

        Task<FileDto> GetByIdAsync(int id);
        Task<FileDto> AddAsync(FileDto file);
        Task<FileDto> UpdateAsync(int id, FileDto file);
        Task<FileDto> DeleteAsync(int id);
        Task<FileDto> SoftDeleteAsync(int id);
        Task<IEnumerable<FileDto>> GetUserFilesInRootFolderAsync(int userId);

        Task<IEnumerable<FileDto>> GetFilesInFolderAsync(int folderId);

    }
}
