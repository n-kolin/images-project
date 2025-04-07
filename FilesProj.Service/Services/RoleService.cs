using AutoMapper;
using FilesProj.Core.DTOs;
using FilesProj.Core.Entities;
using FilesProj.Core.IRepositories;
using FilesProj.Core.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesProj.Service.Services
{
    public class RoleService(IRepositoryManager repositoryManager, IMapper mapper) : IRoleService
    {
        private readonly IRepositoryManager _repositoryManager = repositoryManager;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<RoleDto>> GetAllAsync()
        {
            var roles = await _repositoryManager.Roles.GetAllAsync();
            var roleDtos = _mapper.Map<List<RoleDto>>(roles);
            return roleDtos;
        }

        public async Task<RoleDto> GetByIdAsync(int id)
        {
            var role = await _repositoryManager.Roles.GetByIdAsync(id);
            var roleDto = _mapper.Map<RoleDto>(role);
            return roleDto;
        }
        public async Task<RoleDto> GetByNameAsync(string name)
        {
            var role = await _repositoryManager.Roles.GetByNameAsync(name);
            var roleDto = _mapper.Map<RoleDto>(role);
            return roleDto;
        }
        public async Task<RoleDto> AddAsync(RoleDto roleDto)
        {
            var role = _mapper.Map<Role>(roleDto);
            role = await _repositoryManager.Roles.AddAsync(role);
            roleDto = _mapper.Map<RoleDto>(role);
            await _repositoryManager.SaveAsync();
            return roleDto;
        }

        public async Task<RoleDto> UpdateAsync(int id, RoleDto roleDto)
        {
            var role = _mapper.Map<Role>(roleDto);
            role = await _repositoryManager.Roles.UpdateAsync(id, role);
            roleDto = _mapper.Map<RoleDto>(role);
            await _repositoryManager.SaveAsync();
            return roleDto;
        }

        public async Task<RoleDto> DeleteAsync(int id)
        {
            var role = await _repositoryManager.Roles.DeleteAsync(id);
            var roleDto = _mapper.Map<RoleDto>(role);
            await _repositoryManager.SaveAsync();
            return roleDto;
        }
    }
}
