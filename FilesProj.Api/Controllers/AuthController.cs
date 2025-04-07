
using AutoMapper;
using FilesProj.Api.PostModels;
using FilesProj.Api.PutModels;
using FilesProj.Core.DTOs;
using FilesProj.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService, IMapper mapper) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly IMapper _mapper = mapper;

    // POST api/<UserController>/login
    [HttpPost("login")]
    public async Task<ActionResult<UserWithTokenDto>> Login([FromBody] LoginModel value)
    {
        if(value == null)
            return BadRequest("No value provided");

        var userDto = _mapper.Map<UserDto>(value);
        try
        {
            var result = await _authService.LoginAsync(userDto);
            return Ok(result);

        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("invalid password");
        }
        catch (KeyNotFoundException)
        {
            return NotFound("user not found");
        }
        catch (Exception )
        {
            return StatusCode(500, "internal server error");
        }
        
    }


    // POST api/<UserController>/register
    [HttpPost("register")]
    public async Task<ActionResult<UserWithTokenDto>> Register([FromBody] UserPostModel value)
    {
        if (value == null)
            return BadRequest("No value provided");

        var userDto = _mapper.Map<UserDto>(value);
        try
        {
            var result = await _authService.RegisterAsync(userDto);
            return Ok(result);

        }
        catch (ArgumentException)
        {
            return BadRequest("invalid user data");
        }
        catch (InvalidOperationException)
        {
            return Conflict("Email already in use");
        }
        catch (Exception)
        {
            return StatusCode(500, "internal server error");
        }
    }


    // PUT api/<UserController>/5
    [HttpPut("{id}")]
    [Authorize]

    public async Task<ActionResult<UserWithTokenDto>> Put(int id, [FromBody] UserPutModel value)
    {
        if (id < 0)
            return BadRequest();
        var userDto = _mapper.Map<UserDto>(value);

        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        // פרוק הטוקן
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // קבלת ה-USERID מתוך הclaims
        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId");
        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        var userId = userIdClaim.Value;


        if (int.Parse(userId) != id)
            return Forbid("forbidden");

        try
        {
            var result = await _authService.UpdateAsync(id, userDto);
            return result;

        }
        catch (ArgumentException)
        {
            return BadRequest("invalid user data");
        }
        catch (KeyNotFoundException)
        {
            return NotFound("user not found");
        }
        catch (InvalidOperationException)
        {
            return Conflict("Email already in use");
        }
        catch (Exception)
        {
            return StatusCode(500, "internal server error");
        }

        
    }
}

