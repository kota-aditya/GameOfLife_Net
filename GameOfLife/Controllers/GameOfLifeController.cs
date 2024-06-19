using GameOfLife.Models;
using GameOfLife.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameOfLife.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameOfLifeController : ControllerBase
    {
        private readonly IGameOfLifeService _service;

        public GameOfLifeController(IGameOfLifeService service)
        {
            _service = service;
        }

        [HttpPost("upload")]
        public IActionResult UploadBoard([FromBody] GameOfLifeBoard board)
        {
            var id = _service.AddBoard(board);
            return Ok(new { Id = id });
        }

        [HttpGet("{id}/next")]
        public IActionResult GetNextState(string id)
        {
            var board = _service.GetBoard(id);
            if (board == null)
                return NotFound();

            var nextState = _service.GetNextState(board);
            return Ok(nextState);
        }

        [HttpGet("{id}/states/{n}")]
        public IActionResult GetNthState(string id, int n)
        {
            var board = _service.GetBoard(id);
            if (board == null)
                return NotFound();

            var nthState = _service.GetNthState(board, n);
            return Ok(nthState);
        }

        [HttpGet("{id}/final/{maxSteps}")]
        public IActionResult GetFinalState(string id, int maxSteps)
        {
            var board = _service.GetBoard(id);
            if (board == null)
                return NotFound();

            try
            {
                var finalState = _service.GetFinalState(board, maxSteps);
                return Ok(finalState);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
