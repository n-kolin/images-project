using AutoMapper;
using FilesProj.Api.PostModels;
using FilesProj.Api.PutModels;
using FilesProj.Core.DTOs;
using FilesProj.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilesProj.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FolderController(IFolderService folderService, IMapper mapper) : ControllerBase
    {
        private readonly IFolderService _folderService = folderService;
        private readonly IMapper _mapper = mapper;


        // GET: api/<FolderController>
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<IEnumerable<FolderDto>>> Get()
        {
            try
            {
                var folders = await _folderService.GetAllAsync();
                return Ok(folders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // GET api/<FolderController>/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<FolderDto>> Get(int id)
        {
            if (id < 0)
                return BadRequest("Invalid folder ID");

            try
            {
                var folder = await _folderService.GetByIdAsync(id);
                return folder;
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Folder not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // GET api/<FolderController>/user/{userId}
        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<FolderDto>>> GetParentFolders(int userId)
        {
            if (userId < 0)
                return BadRequest("Invalid user ID");

            try
            {
                var folders = await _folderService.GetParentFoldersAsync(userId);
                return Ok(folders);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("User not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // GET api/<FolderController>/children/{parentId}
        [HttpGet("children/{parentId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<FolderDto>>> GetChildFolders(int parentId)
        {
            if (parentId < -1)
                return BadRequest("Invalid ID");

            try
            {
                var folders = await _folderService.GetChildFoldersAsync(parentId);
                return Ok(folders);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // POST api/<FolderController>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<FolderDto>> Post([FromBody] FolderPostModel value)
        {
            if (value == null)
                return BadRequest("No value provided");

            try
            {
                var folderDto = _mapper.Map<FolderDto>(value);
                var result = await _folderService.AddAsync(folderDto);
                return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // PUT api/<FolderController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<FolderDto>> Put(int id, [FromBody] FolderPutModel value)
        {
            if (id < 0)
                return BadRequest("Invalid folder ID");

            if (value == null)
                return BadRequest("No value provided");

            try
            {
                var folderDto = _mapper.Map<FolderDto>(value);
                var result = await _folderService.UpdateAsync(id, folderDto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // DELETE api/<FolderController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<FolderDto>> Delete(int id)
        {
            if (id < 0)
                return BadRequest("Invalid folder ID");

            try
            {
                var result = await _folderService.DeleteAsync(id);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // DELETE api/<FolderController>/recursive/{id}
        [HttpDelete("rec/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteFolderRecursively(int id)
        {
            if (id < 0)
                return BadRequest("Invalid folder ID");

            try
            {
                await _folderService.DeleteFolderAndContentsAsync(id);
                return Ok("Folder and its contents deleted successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        // DELETE api/<FolderController>/soft/5
        [HttpDelete("soft/{id}")]
        [Authorize]
        public async Task<ActionResult<FolderDto>> SoftDelete(int id)
        {
            if (id < 0)
                return BadRequest("Invalid folder ID");

            try
            {
                var result = await _folderService.SoftDeleteAsync(id);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict("Cannot delete folder because it contains subfolders or files");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // DELETE api/<FolderController>/recursive/{id}
        [HttpDelete("soft/rec/{id}")]
        [Authorize]
        public async Task<IActionResult> SoftDeleteFolderRecursively(int id)
        {
            if (id < 0)
                return BadRequest("Invalid folder ID");

            try
            {
                await _folderService.SoftDeleteFolderAndContentsAsync(id);
                return Ok("Folder and its contents deleted successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}