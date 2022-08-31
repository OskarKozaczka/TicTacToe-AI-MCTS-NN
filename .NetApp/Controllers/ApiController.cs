using Microsoft.AspNetCore.Mvc;
using project.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace project.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [HttpPost("CreateNewGame")]
        public string CreateNewGame()
        {
            return GameManager.CreateNewGame();
        }

        [HttpGet("GetBoard/{id}")]
        public string GetBoard(string id)
        {
            return GameManager.GetBoard(id);
        }

        [HttpPost("GetMove/{id}")]
        public MoveResponseModel GetMove(string id, [FromBody] Move value)
        {
            return GameManager.GetMove(id, value);
        }
    }
}
