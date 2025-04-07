
using AutoMapper;
using FilesProj.Core.DTOs;
using FilesProj.Core.Entities;
using FilesProj.Core.IRepositories;
using FilesProj.Core.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FilesProj.Service.Services
{
    public class AuthService(IConfiguration configuration, IRepositoryManager repositoryManager, IMapper mapper) : IAuthService
    {

        private readonly IConfiguration _configuration = configuration;
        private readonly IRepositoryManager _repositoryManager = repositoryManager;
        private readonly IMapper _mapper = mapper;

        public string GenerateJwtToken(int userId,string username, string[] roles)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim("userId", userId.ToString())
        };

            // הוספת תפקידים כ-Claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<UserWithTokenDto> LoginAsync(UserDto userDto)
        {
            
            var user = await _repositoryManager.Users.GetByEmailAsync(userDto.Email);

            if (user == null)
            {
                throw new KeyNotFoundException();
            }
            if (!user.Password.Equals(userDto.Password))
            {
                throw new UnauthorizedAccessException();
            }
            var role = await _repositoryManager.Roles.GetByIdAsync(user.RoleId);
            

            string token = GenerateJwtToken(user.Id, user.Name, [role.Name]);

            userDto = _mapper.Map<UserDto>(user);
            return new UserWithTokenDto { UserDto = userDto, Token = token };
        }


        public async Task<UserWithTokenDto> RegisterAsync(UserDto userDto)
        {
            var role = await _repositoryManager.Roles.GetByNameAsync(userDto.Role);

            if (role == null || string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Password))
            {
                throw new ArgumentException();
            }
            var userEmail = await _repositoryManager.Users.GetByEmailAsync(userDto.Email);
            if (userEmail != null)
            {
                throw new InvalidOperationException();
            }
            var user = _mapper.Map<User>(userDto);
            user.Role = role;

            user = await _repositoryManager.Users.AddAsync(user);
            await _repositoryManager.SaveAsync();
            string token = GenerateJwtToken(user.Id, user.Name, [user.Role.Name]);
            userDto = _mapper.Map<UserDto>(user);
            return new UserWithTokenDto { UserDto = userDto, Token = token };
        }

        public async Task<UserWithTokenDto> UpdateAsync(int id, UserDto userDto)
        {
            var u = await _repositoryManager.Users.GetByIdAsync(id);
            if (u == null)
                throw new KeyNotFoundException();

            var role = await _repositoryManager.Roles.GetByIdAsync(u.RoleId);

            if (role == null || string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Password))
            {
                throw new ArgumentException();
            }
            var userEmail = await _repositoryManager.Users.GetByEmailAsync(userDto.Email);
            if (userEmail != null && userEmail.Id != id)
            {
                throw new InvalidOperationException();
            }
            var user = _mapper.Map<User>(userDto);
            user.Role = role;

            user = await _repositoryManager.Users.UpdateAsync(id, user);
            await _repositoryManager.SaveAsync();
            string token = GenerateJwtToken(user.Id, user.Name, [user.Role.Name]);
            userDto = _mapper.Map<UserDto>(user);
            return new UserWithTokenDto { UserDto = userDto, Token = token };
        
        }

    }
}
