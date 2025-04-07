using FilesProj.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesProj.Core.IServices
{
    public interface IAuthService
    {
        string GenerateJwtToken(int userID,string username, string[] roles);
        Task<UserWithTokenDto> LoginAsync(UserDto userDto);
        Task<UserWithTokenDto> RegisterAsync(UserDto userDto);
        Task<UserWithTokenDto> UpdateAsync(int id, UserDto userDto);

    }
}
