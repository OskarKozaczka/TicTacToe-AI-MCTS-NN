using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace project.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [HttpGet("StudioJajo")]
        public string Get()
        {
            return "Co oni tam nadają";
        }

        [HttpGet("JakaMapaBedzieGrana")]
        public string GetMap()
        {
            return "Dust 2";
        }

        // POST api/GetMove/id
        [HttpPost("GetMove/{id}")]
        public string Post(string id, [FromBody] string value)
        {
            return "git";
        }

        // PUT api/<ApiController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ApiController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
