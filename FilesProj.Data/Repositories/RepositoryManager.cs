
using FilesProj.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesProj.Data.Repositories
{
    public class RepositoryManager(DataContext context,
        IUserRepository userRepository,
        IFolderRepository folderRepository,
        IFileRepository fileRepository,
        IFrameRepository frameRepository,
        IRoleRepository roleRepository
        ) : IRepositoryManager
    {
        private readonly DataContext _context = context;
        public IUserRepository Users => userRepository;
        public IFolderRepository Folders => folderRepository;
        public IFileRepository Files => fileRepository;
        public IFrameRepository Frames => frameRepository;
        public IRoleRepository Roles => roleRepository;

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    
    }
}
