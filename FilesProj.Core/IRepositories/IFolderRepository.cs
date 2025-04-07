using FilesProj.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using File = FilesProj.Core.Entities.File;

public interface IFolderRepository
{
    Task<IEnumerable<Folder>> GetAllAsync();
    Task<Folder> GetByIdAsync(int id);
    Task<Folder> AddAsync(Folder folder);
    Task<Folder> UpdateAsync(int id, Folder folder);
    Task<Folder> AddSubFolderAsync(int id, Folder folder);
    Task<Folder> AddFileAsync(int id, File file);

    Task<Folder> DeleteAsync(int id);
    Task<Folder> SoftDeleteAsync(int id);
}