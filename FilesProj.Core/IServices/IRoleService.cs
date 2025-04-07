using FilesProj.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesProj.Core.IServices
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllAsync();

        Task<RoleDto> GetByIdAsync(int id);
        Task<RoleDto> GetByNameAsync(string name);
        Task<RoleDto> AddAsync(RoleDto role);
        Task<RoleDto> UpdateAsync(int id, RoleDto role);
        Task<RoleDto> DeleteAsync(int id);
    }
}
