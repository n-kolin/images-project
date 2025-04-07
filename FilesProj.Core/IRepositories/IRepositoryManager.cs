using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesProj.Core.IRepositories
{
    public interface IRepositoryManager
    {
        IUserRepository Users { get; }
        IFolderRepository Folders { get; }
        IFileRepository Files { get; }
        IFrameRepository Frames { get; }
        IRoleRepository Roles { get; }

        Task SaveAsync();

    }
}
