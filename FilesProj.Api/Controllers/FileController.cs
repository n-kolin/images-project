
using AutoMapper;
using FilesProj.Api.PostModels;
using FilesProj.Api.PutModels;
using FilesProj.Core.DTOs;
using FilesProj.Core.Entities;
using FilesProj.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FilesProj.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController(IFileService fileService, IMapper mapper, IAwsService awsService) : ControllerBase
    {
        private readonly IFileService _fileService = fileService;
        private readonly IMapper _mapper = mapper;
        private readonly IAwsService _awsService = awsService;

        // GET: api/<FileController>
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]


        public async Task<ActionResult<IEnumerable<FileDto>>> Get()
        {
            return Ok(await _fileService.GetAllAsync());
        }

        // GET api/<FileController>/5
        [HttpGet("{id}")]
        [Authorize]

        public async Task<ActionResult<FileDto>> Get(int id)
        {
            if (id <= 0)
                return BadRequest();
            var file = await _fileService.GetByIdAsync(id);
            if (file == null)
                return NotFound();
            return file;
        }

        // GET api/<FolderController>/user/{userId}
        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<FileDto>>> GetUserFiles(int userId)
        {
            if (userId < 0)
                return BadRequest("Invalid user ID");

            try
            {
                var files = await _fileService.GetUserFilesInRootFolderAsync(userId);
                return Ok(files);
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
        public async Task<ActionResult<IEnumerable<FileDto>>> GetFolderFiles(int parentId)
        {
            if (parentId < -1)
                return BadRequest("Invalid ID");

            try
            {
                var folders = await _fileService.GetFilesInFolderAsync(parentId);
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




        // POST api/<FileController>
        [HttpPost]
        [Authorize]

        public async Task<ActionResult<FileDto>> Post([FromBody] FilePostModel value)
        {
            if (value == null)
                return BadRequest("No value provided");

            //var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            //// פרוק הטוקן
            //var handler = new JwtSecurityTokenHandler();
            //var jwtToken = handler.ReadJwtToken(token);

            //// קבלת ה-USERID מתוך הclaims
            //var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId");
            //if (userIdClaim == null)
            //{
            //    return Unauthorized();
            //}

            //var userId = userIdClaim.Value;     

            var fileDto = _mapper.Map<FileDto>(value);
            //fileDto.CreatedBy = int.Parse(userId);

            

            try
            {
                var result = await _fileService.AddAsync(fileDto);
                return result;

            }
            catch (ArgumentException)
            {
                return BadRequest("invalid file data");
            }
            catch (Exception)
            {
                return StatusCode(500, "internal server error");
            }


            
        }

        // PUT api/<FileController>/5
        [HttpPut("{id}")]
        [Authorize]

        public async Task<ActionResult<FileDto>> Put(int id, [FromBody] FilePutModel value)
        {


            if (id < 0)
                return BadRequest();
            var fileDto = _mapper.Map<FileDto>(value);

            try
            {
                var result = await _fileService.UpdateAsync(id, fileDto);
                return result;

            }
            catch (ArgumentException)
            {
                return BadRequest("invalid file data");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("file not found");
            }
            catch (Exception)
            {
                return StatusCode(500, "internal server error");
            }

        }

        // DELETE api/<FileController>/5
        [HttpDelete("{id}")]
        [Authorize]

        public async Task<ActionResult<FileDto>> Delete(int id)
        {
            if (id < 0)
                return BadRequest();
            try
            {
                var result = await _fileService.DeleteAsync(id);
                return result;
            }
            catch (KeyNotFoundException)
            {
                return NotFound("result not found");
            }
            catch (Exception)
            {
                return StatusCode(500, "internal server error");
            }


        }

        // DELETE api/<FileController>/soft/5
        [HttpDelete("soft/{id}")]
        [Authorize]

        public async Task<ActionResult<FileDto>> SoftDelete(int id)
        {
            if (id < 0)
                return BadRequest();
            try
            {
                var result = await _fileService.SoftDeleteAsync(id);
                return result;
            }
            catch (KeyNotFoundException)
            {
                return NotFound("result not found");
            }
            catch (Exception)
            {
                return StatusCode(500, "internal server error");
            }


        }



        //upload

        [HttpGet("presigned-url")]
        [Authorize]

        public async Task<ActionResult<string>> GetPreSignedUrl([FromQuery] int userId, [FromQuery] string? path, [FromQuery] string fileName, [FromQuery] string contentType)
        {
            
            if(string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(contentType))
            {
                return BadRequest("Invalid file data");
            }
            var url = await _awsService.GetPreSignedUrlAsync(userId, path, fileName, contentType);
            return Ok(new { url });

            
        }



        //cop
        

        // Upload File Method
        //[HttpPost("upload")]
        //public async Task<ActionResult> UploadFile([FromForm] string filePath, [FromForm] string path, [FromForm] string userName)
        //{
        //    if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(path) || string.IsNullOrEmpty(userName))
        //    {
        //        return BadRequest("Invalid file data");
        //    }
        //    //**
        //   // await _awsService.UploadFileAsync(filePath, path, userName);
        //    return Ok("File uploaded successfully");
        //}

        // Download File Method
        [HttpGet("download-url")]
        //[Authorize]

        public async Task<ActionResult<string>> DownloadUrl([FromQuery] int userId, [FromQuery] string path)
        {
            
            if (string.IsNullOrEmpty(path))
            {
                return BadRequest("Invalid file data");
            }
            
            var url = await _awsService.GetDownloadUrlAsync(userId, path);
            return Ok(new { url });
        }

        // Delete File Method
        //[HttpDelete("delete")]
        //public async Task<ActionResult> DeleteFile([FromQuery] string path, [FromQuery] string userName)
        //{
        //    if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(userName))
        //    {
        //        return BadRequest("Invalid file data");
        //    }
        //    await _awsService.DeleteFileAsync(path, userName);
        //    return Ok("File deleted successfully");
        //}

        // List Files Method
        //[HttpGet("list")]
        //public async Task<ActionResult<IEnumerable<string>>> ListFiles([FromQuery] string userName)
        //{
        //    if (string.IsNullOrEmpty(userName))
        //    {
        //        return BadRequest("Invalid user data");
        //    }
        //    var files = await _awsService.ListFilesAsync(userName);
        //    return Ok(files);
        //}


        [HttpGet("detect-labels")]
        //[Authorize]

        public async Task<ActionResult<List<string>>> DetectLabels([FromQuery] string path)
        {

            if (string.IsNullOrEmpty(path))
            {
                return BadRequest("Invalid file data");
            }

            var labels = await _awsService.DetectLabelsAsync(path);
            return Ok(new { labels });
        }
    }
}

