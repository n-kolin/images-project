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
    public class RoleController(IRoleService roleService, IMapper mapper) : ControllerBase
    {
        private readonly IRoleService _roleService = roleService;
        private readonly IMapper _mapper = mapper;
        // GET: api/<RoleController>
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]


        public async Task<ActionResult<IEnumerable<RoleDto>>> Get()
        {
            return Ok(await _roleService.GetAllAsync());
        }

        // GET api/<RoleController>/5
        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]

        public async Task<ActionResult<RoleDto>> Get(int id)
        {
            if (id < 0)
                return BadRequest();
            var role = await _roleService.GetByIdAsync(id);
            if (role == null)
                return NotFound();
            return role;
        }

        // GET api/<RoleController>/name/aaa
        [HttpGet("name/{name}")]
        [Authorize(Policy = "AdminOnly")]

        public async Task<ActionResult<RoleDto>> Get(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest();
            var role = await _roleService.GetByNameAsync(name);
            if (role == null)
                return NotFound();
            return role;
        }

        // POST api/<RoleController>
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]

        public async Task<ActionResult<RoleDto>> Post([FromBody] RolePostModel value)
        {
            var roleDto = _mapper.Map<RoleDto>(value);
            roleDto = await _roleService.AddAsync(roleDto);
            if (roleDto == null)
                return BadRequest();
            return roleDto;
        }

        // PUT api/<RoleController>/5
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]

        public async Task<ActionResult<RoleDto>> Put(int id, [FromBody] RolePostModel value)
        {
            if (id < 0)
                return BadRequest();
            var roleDto = _mapper.Map<RoleDto>(value);
            roleDto = await _roleService.UpdateAsync(id, roleDto);
            if (roleDto == null)
                return NotFound();
            return roleDto;
        }

        // DELETE api/<RoleController>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]

        public async Task<ActionResult<RoleDto>> Delete(int id)
        {
            if (id < 0)
                return BadRequest();
            var roleDto = await _roleService.DeleteAsync(id);
            if (roleDto == null)
                return NotFound();
            return roleDto;
        }
    }
}
