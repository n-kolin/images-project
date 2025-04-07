using AutoMapper;
using FilesProj.Core.DTOs;
using FilesProj.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = FilesProj.Core.Entities.File;

namespace FilesProj.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Role, opt =>
                opt.MapFrom(src => src.Role.Name))
                .ReverseMap()
                .ForMember(dest => dest.Role, opt =>
                opt.MapFrom(src => new Role { Name = src.Role }));

            CreateMap<File, FileDto>().ReverseMap();
            CreateMap<Frame, FrameDto>().ReverseMap();
            CreateMap<Folder, FolderDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();


        }
    }
}
