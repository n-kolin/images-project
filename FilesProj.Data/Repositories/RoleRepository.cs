using FilesProj.Core.Entities;
using FilesProj.Core.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesProj.Data.Repositories
{
    public class RoleRepository(DataContext context) : IRoleRepository
    {
        private readonly DbSet<Role> _roles = context.RolesList;

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _roles.ToListAsync();
        }

        public async Task<Role> GetByIdAsync(int id)
        {
            return await _roles.FindAsync(id);
        }
        public async Task<Role> GetByNameAsync(string name)
        {
            return await _roles.Where(r => r.Name.Equals(name)).FirstOrDefaultAsync();

        }
        public async Task<Role> AddAsync(Role role)
        {
            var r = await _roles.AddAsync(role);
            return r.Entity;
        }

        public async Task<Role> UpdateAsync(int id, Role role)
        {
            var r = await _roles.FindAsync(id);

            r.Name = role.Name;
            r.Description = role.Description;
            r.Permissions = role.Permissions;

            r.UpdatedAt = DateTime.Now;

            //_roles.Update(r);
            return r;
        }

        public async Task<Role> DeleteAsync(int id)
        {
            var r = await _roles.FindAsync(id);
            await Task.Run(() => _roles.Remove(r));
            return r;
        }
    }
}
