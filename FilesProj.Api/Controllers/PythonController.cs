using FilesProj.Api.PostModels;
using FilesProj.Core.DTOs;
using FilesProj.Core.IServices;
using FilesProj.Service.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FilesProj.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PythonController(IPyService pyService) : ControllerBase
    {
        private readonly IPyService _pyService = pyService;

        // GET: api/<PythonController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PythonController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PythonController>
        [HttpPost]
        public async Task<ActionResult<PyResponse>> Post([FromBody] PromptModel value)
        {
            var res  = await _pyService.ProcessPromptAsync(value);
            if (value.Prompt == null || value.Prompt == "")
                return BadRequest();
            return res;
        }

        // PUT api/<PythonController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PythonController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
