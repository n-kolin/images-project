using AutoMapper;
using FilesProj.Api.PostModels;
using FilesProj.Api.PutModels;
using FilesProj.Core.DTOs;
using FilesProj.Core.Entities;
using FilesProj.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace FilesProj.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrameController(IFrameService frameService, IMapper mapper, IAwsService awsService) : ControllerBase
    {
        private readonly IFrameService _frameService = frameService;
        private readonly IMapper _mapper = mapper;
        private readonly IAwsService _awsService = awsService;

        // GET: api/<FrameController>
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<IEnumerable<FrameDto>>> Get()
        {
            return Ok(await _frameService.GetAllAsync());
        }

        // GET api/<FrameController>/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<FrameDto>> Get(int id)
        {
            if (id <= 0)
                return BadRequest();
            var frame = await _frameService.GetByIdAsync(id);
            if (frame == null)
                return NotFound();
            return frame;
        }

        // POST api/<FrameController>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<FrameDto>> Post([FromBody] FramePostModel value)
        {
            if (value == null)
                return BadRequest("No value provided");

            var frameDto = _mapper.Map<FrameDto>(value);
            try
            {
                var result = await _frameService.AddAsync(frameDto);
                return result;
            }
            catch (ArgumentException)
            {
                return BadRequest("invalid frame data");
            }
            catch (Exception)
            {
                return StatusCode(500, "internal server error");
            }
        }

        // PUT api/<FrameController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<FrameDto>> Put(int id, [FromBody] FramePutModel value)
        {
            if (id < 0)
                return BadRequest();
            var frameDto = _mapper.Map<FrameDto>(value);

            try
            {
                var result = await _frameService.UpdateAsync(id, frameDto);
                return result;
            }
            catch (ArgumentException)
            {
                return BadRequest("invalid frame data");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("frame not found");
            }
            catch (Exception)
            {
                return StatusCode(500, "internal server error");
            }
        }

        // DELETE api/<FrameController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<FrameDto>> Delete(int id)
        {
            if (id < 0)
                return BadRequest();
            try
            {
                var result = await _frameService.DeleteAsync(id);
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

        // Get PreSigned URL
        [HttpGet("presigned-url")]
        [Authorize]
        public async Task<ActionResult<string>> GetPreSignedUrl([FromQuery] string fileName, [FromQuery] string contentType)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(contentType))
            {
                return BadRequest("Invalid file data");
            }
            var url = await _awsService.GetPreSignedUrlAsync(fileName, contentType);
            return Ok(new { url });
        }

        // Download URL
        [HttpGet("download-url")]
        [Authorize]
        public async Task<ActionResult<string>> DownloadUrl([FromQuery] string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("Invalid file data");
            }
            var url = await _awsService.GetDownloadUrlAsync(fileName);
            return Ok(new { url });
        }
    }
}