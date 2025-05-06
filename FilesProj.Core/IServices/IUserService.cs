using FilesProj.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesProj.Core.IServices
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();

        Task<UserDto> GetByIdAsync(int id);
        Task<UserDto> GetByEmailAsync(string email);
        Task<IEnumerable<MonthlyStatsDto>> GetLoginsStatsAsync();

        Task<IEnumerable<FileDto>> GetFilesAsync(int id);
        Task<IEnumerable<FolderDto>> GetFoldersAsync(int id);
        Task<UserDto> DeleteAsync(int id);


    }
}
