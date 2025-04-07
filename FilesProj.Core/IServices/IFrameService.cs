using FilesProj.Core.DTOs;
using FilesProj.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesProj.Core.IServices
{
    public interface IFrameService
    {
        Task<IEnumerable<FrameDto>> GetAllAsync();

        Task<FrameDto> GetByIdAsync(int id);
        Task<FrameDto> AddAsync(FrameDto file);
        Task<FrameDto> UpdateAsync(int id, FrameDto frame);
        Task<FrameDto> DeleteAsync(int id);

    }
}
