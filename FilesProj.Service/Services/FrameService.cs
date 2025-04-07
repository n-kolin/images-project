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
using Frame = FilesProj.Core.Entities.Frame;

namespace FilesProj.Service.Services
{
    public class FrameService(IRepositoryManager repositoryManager, IMapper mapper) : IFrameService
    {
        private readonly IRepositoryManager _repositoryManager = repositoryManager;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<FrameDto>> GetAllAsync()
        {
            var frames = await _repositoryManager.Frames.GetAllAsync();
            var frameDtos = _mapper.Map<List<FrameDto>>(frames);
            return frameDtos;
        }

        public async Task<FrameDto> GetByIdAsync(int id)
        {
            var frames = await _repositoryManager.Frames.GetByIdAsync(id);
            var frameDto = _mapper.Map<FrameDto>(frames);
            return frameDto;
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

        public async Task<FrameDto> AddAsync(FrameDto frameDto)
        {
            int mb = 5;


            if (!IsValidType(frameDto.Type) || string.IsNullOrEmpty(frameDto.Name) || frameDto.Size > mb * 1024 * 1024)
            {
                throw new ArgumentException();
            }

            var frame = _mapper.Map<Frame>(frameDto);
            frame = await _repositoryManager.Frames.AddAsync(frame);
            frameDto = _mapper.Map<FrameDto>(frame);
            await _repositoryManager.SaveAsync();
            return frameDto;
        }

        public async Task<FrameDto> UpdateAsync(int id, FrameDto frameDto)
        {
            var f = await _repositoryManager.Frames.GetByIdAsync(id);
            if (f == null)
                throw new KeyNotFoundException();
            int mb = 5;


            if (!IsValidType(frameDto.Type) || string.IsNullOrEmpty(frameDto.Name) || frameDto.Size > mb * 1024 * 1024)
            {
                throw new ArgumentException();
            }


            var frame = _mapper.Map<Frame>(frameDto);
            frame = await _repositoryManager.Frames.UpdateAsync(id, frame);
            frameDto = _mapper.Map<FrameDto>(frame);
            await _repositoryManager.SaveAsync();
            return frameDto;
        }
        public async Task<FrameDto> DeleteAsync(int id)
        {
            var f = await _repositoryManager.Frames.GetByIdAsync(id);
            if (f == null)
                throw new KeyNotFoundException();
            var frame = await _repositoryManager.Frames.DeleteAsync(id);
            var frameDto = _mapper.Map<FrameDto>(frame);
            await _repositoryManager.SaveAsync();
            return frameDto;
        }


        

    }
}