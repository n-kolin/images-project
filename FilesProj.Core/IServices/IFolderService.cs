using AutoMapper;
using FilesProj.Core.DTOs;
using FilesProj.Core.Entities;
using FilesProj.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesProj.Core.IServices
{
    public interface IFolderService
    {
        Task<IEnumerable<FolderDto>> GetAllAsync();
        Task<FolderDto> GetByIdAsync(int id);
        Task<IEnumerable<FolderDto>> GetParentFoldersAsync(int userId);
        Task<IEnumerable<FolderDto>> GetChildFoldersAsync(int parentId);
        Task<FolderDto> AddAsync(FolderDto folderDto);
        Task<FolderDto> UpdateAsync(int id, FolderDto folderDto);
        Task<FolderDto> DeleteAsync(int id);
        Task DeleteFolderAndContentsAsync(int id);
        Task<FolderDto> SoftDeleteAsync(int id);
        Task SoftDeleteFolderAndContentsAsync(int id);





    }
}
