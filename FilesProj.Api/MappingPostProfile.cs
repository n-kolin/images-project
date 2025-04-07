using AutoMapper;
using FilesProj.Api.PostModels;
using FilesProj.Api.PutModels;
using FilesProj.Core.DTOs;
using FilesProj.Core.Entities;

namespace FilesProj.Api
{
    public class MappingPostProfile:Profile
    {
        public MappingPostProfile() 
        {
            CreateMap<UserPostModel, UserDto>();
            CreateMap<FilePostModel, FileDto>();
            CreateMap<FramePostModel, FrameDto>();
            CreateMap<FolderPostModel, FolderDto>();

            CreateMap<RolePostModel, RoleDto>();


            CreateMap<UserPutModel, UserDto>();
            CreateMap<FilePutModel, FileDto>();
            CreateMap<FolderPutModel, FolderDto>();
            CreateMap<FramePutModel, FrameDto>();



            CreateMap<LoginModel, UserDto>();
        }
    }
}
