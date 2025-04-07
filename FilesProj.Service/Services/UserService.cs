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
    public class UserService(IRepositoryManager repositoryManager, IMapper mapper) : IUserService
    {
        private readonly IRepositoryManager _repositoryManager = repositoryManager;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _repositoryManager.Users.GetAllAsync();
            var userDtos = _mapper.Map<List<UserDto>>(users);
            return userDtos;
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await _repositoryManager.Users.GetByIdAsync(id);
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }
        public async Task<UserDto> GetByEmailAsync(string email)
        {
            var user = await _repositoryManager.Users.GetByEmailAsync(email);
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task<IEnumerable<FileDto>> GetFilesAsync(int id)
        {
            
            var files = await _repositoryManager.Users.GetFilesAsync(id);
            var filesDto = _mapper.Map<List<FileDto>>(files);
            var filesInRoot = filesDto.Where(f => f.FolderId == null).ToList();
            return filesInRoot;
        }
        public async Task<IEnumerable<FolderDto>> GetFoldersAsync(int id)
        {

            var folders = await _repositoryManager.Users.GetFoldersAsync(id);
            var foldersDto = _mapper.Map<List<FolderDto>>(folders);
            var foldersInRoot = foldersDto.Where(f=>f.ParentId == null).ToList();
            return foldersInRoot;
        }


        public async Task<UserDto> DeleteAsync(int id)
        {
            var u = await _repositoryManager.Users.GetByIdAsync(id);
            if (u == null)
                throw new KeyNotFoundException();

            var user = await _repositoryManager.Users.DeleteAsync(id);
            var userDto = _mapper.Map<UserDto>(user);
            await _repositoryManager.SaveAsync();
            return userDto;
        }

    }
}
