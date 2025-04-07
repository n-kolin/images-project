using FilesProj.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesProj.Core.IRepositories
{
    public interface IFrameRepository
    {
        Task<IEnumerable<Frame>> GetAllAsync();

        Task<Frame> GetByIdAsync(int id);

        Task<Frame> AddAsync(Frame frame);

        Task<Frame> UpdateAsync(int id, Frame frame);

        Task<Frame> DeleteAsync(int id);
    }
}
