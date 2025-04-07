using Amazon.S3.Model;
using AutoMapper;
using FilesProj.Core.DTOs;
using FilesProj.Core.Entities;
using FilesProj.Core.IRepositories;
using FilesProj.Core.IServices;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = FilesProj.Core.Entities.File;

namespace FilesProj.Service.Services
{
    public class FileService(IRepositoryManager repositoryManager, IMapper mapper) : IFileService
    {
        private readonly IRepositoryManager _repositoryManager = repositoryManager;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<FileDto>> GetAllAsync()
        {
            var files = await _repositoryManager.Files.GetAllAsync();
            var fileDtos = _mapper.Map<List<FileDto>>(files);
            return fileDtos;
        }

        public async Task<FileDto> GetByIdAsync(int id)
        {
            var files = await _repositoryManager.Files.GetByIdAsync(id);
            var fileDto = _mapper.Map<FileDto>(files);
            return fileDto;
        }

        private bool IsValidType(string type)
        {
            string[] extensions = { "jpg", "jpeg", "png" };
            type = type.ToLower().Split('/')[1];
            if (extensions.Contains(type))
            {
                return true;
            }
            return false;
        }

        public async Task<FileDto> AddAsync(FileDto fileDto)
        {
            int mb = 5;


            if (!IsValidType(fileDto.Type) || string.IsNullOrEmpty(fileDto.Name) || fileDto.Size > mb * 1024 * 1024)
            {
                throw new ArgumentException();
            }

            var file = _mapper.Map<File>(fileDto);
            file = await _repositoryManager.Files.AddAsync(file);

            if (file.FolderId != null)
            {
                var parentFolder = await _repositoryManager.Folders.GetByIdAsync((int)file.FolderId);
                if (parentFolder == null)
                    throw new KeyNotFoundException("Parent folder not found");

                file.Folder = parentFolder;
                await _repositoryManager.Folders.AddFileAsync((int)file.FolderId, file); 
            }
            else //root
            {
                // עדכון היוזר שיצר את v FILE
                var user = await _repositoryManager.Users.GetByIdAsync(file.CreatedBy);
                if (user == null)
                    throw new KeyNotFoundException("User not found");

                file.User = user;
                await _repositoryManager.Users.AddFileAsync(file.CreatedBy, file);

            }
            await _repositoryManager.SaveAsync();
            fileDto = _mapper.Map<FileDto>(file);
            return fileDto;



        }

        public async Task<FileDto> UpdateAsync(int id, FileDto fileDto)
        {
            var f = await _repositoryManager.Files.GetByIdAsync(id);
            if (f == null)
                throw new KeyNotFoundException();
            int mb = 5;


            if (!IsValidType(fileDto.Type) || string.IsNullOrEmpty(fileDto.Name) || fileDto.Size > mb * 1024 * 1024)
            {
                throw new ArgumentException();
            }


            var file = _mapper.Map<File>(fileDto);
            file = await _repositoryManager.Files.UpdateAsync(id, file);
            fileDto = _mapper.Map<FileDto>(file);
            await _repositoryManager.SaveAsync();
            return fileDto;
        }
        public async Task<FileDto> DeleteAsync(int id)
        {
            var f = await _repositoryManager.Files.GetByIdAsync(id);
            if (f == null)
                throw new KeyNotFoundException();
            var file = await _repositoryManager.Files.DeleteAsync(id);
            var fileDto = _mapper.Map<FileDto>(file);
            await _repositoryManager.SaveAsync();
            return fileDto;
        }

        public async Task<FileDto> SoftDeleteAsync(int id)
        {
            var f = await _repositoryManager.Files.GetByIdAsync(id);
            if (f == null)
                throw new KeyNotFoundException();
            var file = await _repositoryManager.Files.SoftDeleteAsync(id);
            var fileDto = _mapper.Map<FileDto>(file);
            await _repositoryManager.SaveAsync();
            return fileDto;
        }

        // New function to get all files of a specific user in the root folder
        public async Task<IEnumerable<FileDto>> GetUserFilesInRootFolderAsync(int userId)
        {
            var user = await _repositoryManager.Users.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            var files = user.Files.Where(f => !f.IsDeleted).ToList();
            var fileDtos = _mapper.Map<List<FileDto>>(files);
            return fileDtos;
        }

        // New function to get all files in a specific folder
        public async Task<IEnumerable<FileDto>> GetFilesInFolderAsync(int folderId)
        {
            // החזרת כל הקבצים הבנות של התיקיה האב
            var parentFolder = await _repositoryManager.Folders.GetByIdAsync(folderId);
            if (parentFolder == null)
                throw new KeyNotFoundException("Parent folder not found");

            var childFiles = parentFolder.Files.Where(f => !f.IsDeleted).ToList();
            var childFileDtos = _mapper.Map<List<FileDto>>(childFiles);
            return childFileDtos;
        }

    }
}
