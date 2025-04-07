using FilesProj.Core.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Frame = FilesProj.Core.Entities.Frame;

namespace FilesProj.Data.Repositories
{
    public class FrameRepository(DataContext context) : IFrameRepository
    {
        private readonly DbSet<Frame> _frames = context.FramesList;

        public async Task<IEnumerable<Frame>> GetAllAsync()
        {
            return await _frames.ToListAsync();
        }

        public async Task<Frame> GetByIdAsync(int id)
        {
            return await _frames.FindAsync(id);
        }

        public async Task<Frame> AddAsync(Frame frame)
        {
            var f = await _frames.AddAsync(frame);
            return f.Entity;
        }

        public async Task<Frame> UpdateAsync(int id, Frame frame)
        {
            var f = await _frames.FindAsync(id);

            if (f != null)
            {
                f.Name = frame.Name;
                f.Type = frame.Type;
                f.Size = frame.Size;

                f.UpdatedAt = DateTime.Now;

                //_frames.Update(f); // Update is not necessary if the context is being tracked
            }

            return f;
        }

        public async Task<Frame> DeleteAsync(int id)
        {
            var f = await _frames.FindAsync(id);
            if (f != null)
            {
                _frames.Remove(f);
            }
            return f;
        }
    }
}