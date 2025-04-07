using FilesProj.Core.IRepositories;
using FilesProj.Core.IServices;
using FilesProj.Core;

using System.Configuration;
using FilesProj.Service.Services;
using FilesProj.Data.Repositories;
using FilesProj.Data;


namespace FilesProj.Api
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IFrameService, FrameService>();
            services.AddScoped<IFolderService, FolderService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IAwsService, AwsService>();


            services.AddScoped<IRepositoryManager, RepositoryManager>();


            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IFrameRepository, FrameRepository>();
            services.AddScoped<IFolderRepository, FolderRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();


            //IServiceCollection serviceCollection = services.AddScoped<IConfiguration,Configuration>();


            services.AddAutoMapper(typeof(MappingProfile), typeof(MappingPostProfile));
            
            services.AddDbContext<DataContext>();
        }
    }
}
