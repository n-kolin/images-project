using AutoMapper;
using FilesProj.Core.DTOs;
using FilesProj.Core.Entities;
using FilesProj.Core.IRepositories;
using FilesProj.Core.IServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class FolderService : IFolderService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public FolderService(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<IEnumerable<FolderDto>> GetAllAsync()
    {
        var folders = await _repositoryManager.Folders.GetAllAsync();
        
        var folderDtos = _mapper.Map<List<FolderDto>>(folders);
        return folderDtos;
    }

    public async Task<FolderDto> GetByIdAsync(int id)
    {
        var folder = await _repositoryManager.Folders.GetByIdAsync(id);
        if (folder == null)
            throw new KeyNotFoundException();
        var folderDto = _mapper.Map<FolderDto>(folder);
        return folderDto;
        
    }
    public async Task<IEnumerable<FolderDto>> GetParentFoldersAsync(int userId)
    {
        var user = await _repositoryManager.Users.GetByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        var parentFolders = user.Folders.Where(f=>f.ParentId == null && !f.IsDeleted).ToList();
        var parentFolderDtos = _mapper.Map<List<FolderDto>>(parentFolders);
        return parentFolderDtos;
    }
    public async Task<IEnumerable<FolderDto>> GetChildFoldersAsync(int parentId)
    {

        // החזרת כל התיקיות הבנות של התיקיה האב
        var parentFolder = await _repositoryManager.Folders.GetByIdAsync(parentId);
        if (parentFolder == null)
            throw new KeyNotFoundException("Parent folder not found");

        var childFolders = parentFolder.SubFolders.Where(f => !f.IsDeleted).ToList();
        var childFolderDtos = _mapper.Map<List<FolderDto>>(childFolders);
        return childFolderDtos;

    }

    public async Task<FolderDto> AddAsync(FolderDto folderDto)
    {
        if (string.IsNullOrEmpty(folderDto.Name))
            throw new ArgumentException("Folder name is required");

        var folder = _mapper.Map<Folder>(folderDto);

        

        // עדכון התיקיה האב אם קיימת
        if (folderDto.ParentId != null)
        {
            var parentFolder = await _repositoryManager.Folders.GetByIdAsync((int)folderDto.ParentId);
            if (parentFolder == null)
                throw new KeyNotFoundException("Parent folder not found");

            folder.Parent = parentFolder;
            await _repositoryManager.Folders.AddSubFolderAsync((int)folderDto.ParentId, folder); // עדכון רשימת התיקיות הבנות של התיקיה האב

            //parentFolder.SubFolders.Add(folder); // עדכון רשימת התיקיות הבנות של התיקיה האב
        }
        //בכל מקרה לעדכן את היוזר
        //else // אם אין תיקית אב מעדכנים את ה USER
        //{
            // עדכון היוזר שיצר את התיקיה
            var user = await _repositoryManager.Users.GetByIdAsync(folderDto.CreatedBy);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            folder.User = user;
            await _repositoryManager.Users.AddFolderAsync(folderDto.CreatedBy, folder);
            //user.Folders.Add(folder); // עדכון רשימת התיקיות של המשתמש

        //}

        folder = await _repositoryManager.Folders.AddAsync(folder);
        await _repositoryManager.SaveAsync();
        folderDto = _mapper.Map<FolderDto>(folder);
        return folderDto;
    }



    public async Task<FolderDto> UpdateAsync(int id, FolderDto folderDto)
    {
        var f = await _repositoryManager.Folders.GetByIdAsync(id);
        if (f == null)
            throw new KeyNotFoundException();

        if (string.IsNullOrEmpty(folderDto.Name))
            throw new ArgumentException("Folder name is required");

        f.Name = folderDto.Name;

        // עדכון התיקיה האב אם קיימת
        if (folderDto.ParentId != null && folderDto.ParentId != f.ParentId)
        {

            var parentFolder = await _repositoryManager.Folders.GetByIdAsync((int)folderDto.ParentId);
            if (parentFolder == null)
                throw new KeyNotFoundException("Parent folder not found");
            f.Parent = parentFolder;
            parentFolder.SubFolders.Add(f); // עדכון רשימת התיקיות הבנות של התיקיה האב החדשה
        }

        // אם התיקיה עברה מתיקיה אב אחת לאחרת, הסר אותה מרשימת התיקיות הבנות של התיקיה האב הקודמת
        if (f.ParentId != null && f.ParentId != folderDto.ParentId)
        {
            var oldParentFolder = await _repositoryManager.Folders.GetByIdAsync((int)f.ParentId);
            if (oldParentFolder != null)
            {
                oldParentFolder.SubFolders.Remove(f);
            }
        }

        f.ParentId = folderDto.ParentId;

        var folder = await _repositoryManager.Folders.UpdateAsync(id, f);
        folderDto = _mapper.Map<FolderDto>(folder);
        await _repositoryManager.SaveAsync();
        return folderDto;
    }


    public async Task<FolderDto> DeleteAsync(int id)
    {
        var f = await _repositoryManager.Folders.GetByIdAsync(id);
        if (f == null)
            throw new KeyNotFoundException();

        // בדיקה אם התיקיה מכילה תתי-תיקיות
        if (f.SubFolders != null && f.SubFolders.Count > 0)
            throw new InvalidOperationException("Cannot delete a folder that contains sub-folders");

        // בדיקה אם התיקיה מכילה קבצים
        if (f.Files != null && f.Files.Count > 0)
            throw new InvalidOperationException("Cannot delete a folder that contains files");

        // עדכון רשימת התיקיות של המשתמש
        f.User.Folders.Remove(f);

        // עדכון רשימת התיקיות הבנות של התיקיה האב אם קיימת
        if (f.ParentId != null)
        {
            f.Parent.SubFolders.Remove(f);
        }

        var folder = await _repositoryManager.Folders.DeleteAsync(id);
        var folderDto = _mapper.Map<FolderDto>(folder);
        await _repositoryManager.SaveAsync();
        return folderDto;
    }
    public async Task DeleteFolderAndContentsAsync(int id)
    {
        var f = await _repositoryManager.Folders.GetByIdAsync(id);
        if (f == null)
            throw new KeyNotFoundException();

        // מחיקת כל הקבצים בתיקיה
        foreach (var file in f.Files.ToList())
        {
            await _repositoryManager.Files.DeleteAsync(file.Id);
        }

        // מחיקת כל תתי-התיקיות בתיקיה
        foreach (var subFolder in f.SubFolders.ToList())
        {
            await DeleteFolderAndContentsAsync(subFolder.Id);
        }

        // עדכון רשימת התיקיות של המשתמש
        f.User.Folders.Remove(f);

        // עדכון רשימת התיקיות הבנות של התיקיה האב אם קיימת
        if (f.ParentId != null)
        {
            f.Parent.SubFolders.Remove(f);
        }

        await _repositoryManager.Folders.DeleteAsync(f.Id);
        await _repositoryManager.SaveAsync();

    }

    //soft delete
    public async Task<FolderDto> SoftDeleteAsync(int id)
    {
        var f = await _repositoryManager.Folders.GetByIdAsync(id);
        if (f == null)
            throw new KeyNotFoundException();

        // בדיקה אם התיקיה מכילה תתי-תיקיות
        if (f.SubFolders != null && f.SubFolders.Any(subFolder => !subFolder.IsDeleted))
            throw new InvalidOperationException("Cannot delete a folder that contains sub-folders");

        // בדיקה אם התיקיה מכילה קבצים
        if (f.Files != null && f.Files.Any(file => !file.IsDeleted))
            throw new InvalidOperationException("Cannot delete a folder that contains files");

        // עדכון רשימת התיקיות של המשתמש
        //f.User.Folders.Remove(f);

        // עדכון רשימת התיקיות הבנות של התיקיה האב אם קיימת
        //if (f.ParentId != null)
        //{
        //    f.Parent.SubFolders.Remove(f);
        //}

        var folder = await _repositoryManager.Folders.SoftDeleteAsync(id);
        var folderDto = _mapper.Map<FolderDto>(folder);
        await _repositoryManager.SaveAsync();
        return folderDto;
    }
    public async Task SoftDeleteFolderAndContentsAsync(int id)
    {
        var f = await _repositoryManager.Folders.GetByIdAsync(id);
        if (f == null)
            throw new KeyNotFoundException();

        // מחיקת כל הקבצים בתיקיה
        foreach (var file in f.Files.Where(f=>!f.IsDeleted).ToList())
        {
            await _repositoryManager.Files.SoftDeleteAsync(file.Id);
        }

        // מחיקת כל תתי-התיקיות בתיקיה
        foreach (var subFolder in f.SubFolders.Where(f => !f.IsDeleted).ToList())
        {
            await SoftDeleteFolderAndContentsAsync(subFolder.Id);
        }

        // עדכון רשימת התיקיות של המשתמש
        //f.User.Folders.Remove(f);

        // עדכון רשימת התיקיות הבנות של התיקיה האב אם קיימת
        //if (f.ParentId != null)
        //{
        //    f.Parent.SubFolders.Remove(f);
        //}

        await _repositoryManager.Folders.SoftDeleteAsync(f.Id);
        await _repositoryManager.SaveAsync();

    }


}