using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        [HttpGet("GetBoard/{id}")]
        public string GetBoard(string id)
        {
            return GameManager.GetBoard(id);
        }

        // POST api/GetMove/id
        [HttpPost("GetMove/{id}")]
        public object GetMovePost(string id, [FromBody] Move value)
        {
            return GameManager.GetMove(id, value);
        }

        [HttpPost("CreateNewGame")]
        public string CreateNewGamePost()
        {
            return GameManager.CreateNewGame();
        }
    }
}
