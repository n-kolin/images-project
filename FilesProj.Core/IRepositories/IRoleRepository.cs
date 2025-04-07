using FilesProj.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesProj.Core.IRepositories
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllAsync();

        Task<Role> GetByIdAsync(int id);
        Task<Role> GetByNameAsync(string name);

        Task<Role> AddAsync(Role role);

        Task<Role> UpdateAsync(int id, Role role);

        Task<Role> DeleteAsync(int id);
    }
}
