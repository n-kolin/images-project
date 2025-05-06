using AutoMapper;
using FilesProj.Api.PostModels;
using FilesProj.Api.PutModels;
using FilesProj.Core.DTOs;
using FilesProj.Core.Entities;
using FilesProj.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FilesProj.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController (IUserService userService, IMapper mapper): ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IMapper _mapper = mapper;

        // GET: api/<UserController>
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]

        public async Task<ActionResult<IEnumerable<UserDto>>> Get()
        {
            return Ok(await _userService.GetAllAsync());
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        [Authorize]

        public async Task<ActionResult<UserDto>> Get(int id)
        {
            if (id < 0)
                return BadRequest();
            var result = await _userService.GetByIdAsync(id);
            if(result == null)
                return NotFound();
            return result;

        }

        // GET api/<UserController>/email/a@a
        [HttpGet("email/{email}")]
        [Authorize]

        public async Task<ActionResult<UserDto>> Get(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest();
            var result = await _userService.GetByEmailAsync(email);
            if (result == null)
                return NotFound();
            return result;

        }

        // GET: api/<UserController>/logins-stats
        [HttpGet("logins-stats")]
        [Authorize(Policy = "AdminOnly")]

        public async Task<ActionResult<IEnumerable<MonthlyStatsDto>>> GetLoginsStats()
        {
            return Ok(await _userService.GetLoginsStatsAsync());
        }

        // GET api/<UserController>/5/files
        [HttpGet("{id}/files")]
        [Authorize]

        public async Task<ActionResult<IEnumerable<FileDto>>> GetFiles(int id)
        {
            if (id < 0)
                return BadRequest();
            var result = await _userService.GetFilesAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);

        }
        // GET api/<UserController>/5/folder
        [HttpGet("{id}/folders")]
        [Authorize]

        public async Task<ActionResult<IEnumerable<FileDto>>> GetFolders(int id)
        {
            if (id < 0)
                return BadRequest();
            var result = await _userService.GetFoldersAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);

        }


        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]

        public async Task<ActionResult<UserDto>> Delete(int id)
        {
            if (id < 0)
                return BadRequest();
            try
            {
                var result = await _userService.DeleteAsync(id);
                return result;
            }
            catch (KeyNotFoundException ) 
            {
                return NotFound("result not found");
            }
            catch (Exception)
            {
                return StatusCode(500, "internal server error");
            }

        }
    }
}
