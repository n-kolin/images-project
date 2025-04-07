using FilesProj.Core.Entities;
using FilesProj.Core.IRepositories;
using FilesProj.Data;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using Org.BouncyCastle.Asn1.X509;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using File = FilesProj.Core.Entities.File;

public class FolderRepository(DataContext context) : IFolderRepository
{
    private readonly DbSet<Folder> _folders = context.FoldersList;
    public async Task<IEnumerable<Folder>> GetAllAsync()
    {
        return await _folders
            .Include(f => f.SubFolders)
            .Include(f => f.Files)
            .Include(f => f.User)
            .Where(f => !f.IsDeleted)
            .ToListAsync()
        ;
    }

    public async Task<Folder> GetByIdAsync(int id)
    {
        return await _folders
            .Include(f => f.SubFolders)
            .Include(f => f.Files)
            .Include(f => f.User)
            .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted)
            ;
    }

    public async Task<Folder> AddAsync(Folder folder)
    {
        var f = await _folders.AddAsync(folder);
        return f.Entity;
    }

    public async Task<Folder> UpdateAsync(int id, Folder folder)
    {
        var f = await _folders.FindAsync(id);

        f.Name = folder.Name;
        f.Parent = folder.Parent;

        f.UpdatedAt = DateTime.Now;

        //_folders.Update(f);
        return f;
    }
    public async Task<Folder> AddSubFolderAsync(int id, Folder folder)
    {
        var f = await _folders.FindAsync(id);

        f.SubFolders.Add(folder);

        f.UpdatedAt = DateTime.Now;

        //_folders.Update(f);
        return f;
    }
    public async Task<Folder> AddFileAsync(int id, File file)
    {
        var f = await _folders.FindAsync(id);

        f.Files.Add(file);

        f.UpdatedAt = DateTime.Now;

        //_folders.Update(f);
        return f;
    }

    public async Task<Folder> DeleteAsync(int id)
    {
        var f = await _folders.FindAsync(id);
        await Task.Run(() => _folders.Remove(f));
        return f;


    }
    public async Task<Folder> SoftDeleteAsync(int id)
    {
        var f = await _folders.FindAsync(id);

        f.IsDeleted = true;
        f.UpdatedAt = DateTime.Now;

        //_folders.Update(f);
        return f;
    }
}