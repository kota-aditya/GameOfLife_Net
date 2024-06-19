using GameOfLife.Models;
using GameOfLife.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Swashbuckle.AspNetCore.Annotations;
using GameOfLife.Helper;

namespace GameOfLife.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameOfLifeController : ControllerBase
    {
        private readonly IGameOfLifeService _service;
        private readonly ILogger<GameOfLifeController> _logger;

        public GameOfLifeController(IGameOfLifeService service, ILogger<GameOfLifeController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("upload")]
        [SwaggerOperation(Summary = "Uploads a new board state.", Description = "Uploads a new board state and returns the ID of the created board.")]
        [SwaggerResponse(200, "Board successfully uploaded.", typeof(object))]
        [SwaggerResponse(400, "Invalid board state.")]
        [SwaggerResponse(500, "An error occurred while processing your request.")]
        public IActionResult UploadBoard([FromBody] GameOfLifeBoard board)
        {
            try
            {
                if (!BoardValidator.IsValid(board, out string errorMessage))
                {
                    _logger.LogWarning("Invalid board state: {ErrorMessage}", errorMessage);
                    return BadRequest(new { Error = errorMessage });
                }

                var id = _service.AddBoard(board);
                _logger.LogInformation("Board uploaded with ID: {BoardId}", id);
                return Ok(new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while uploading the board.");
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("{id}/next")]
        [SwaggerOperation(Summary = "Gets the next state for a board.", Description = "Gets the next state for the specified board ID.")]
        [SwaggerResponse(200, "Next state successfully retrieved.", typeof(GameOfLifeBoard))]
        [SwaggerResponse(404, "Board not found.")]
        [SwaggerResponse(500, "An error occurred while processing your request.")]
        public IActionResult GetNextState(string id)
        {
            try
            {
                var board = _service.GetBoard(id);
                if (board == null)
                {
                    _logger.LogWarning("Board with ID {BoardId} not found.", id);
                    return NotFound();
                }

                var nextState = _service.GetNextState(board);
                _logger.LogInformation("Next state retrieved for board ID: {BoardId}", id);
                return Ok(nextState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the next state.");
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("{id}/states/{n}")]
        [SwaggerOperation(Summary = "Gets the nth state for a board.", Description = "Gets the nth state for the specified board ID and number of steps.")]
        [SwaggerResponse(200, "Nth state successfully retrieved.", typeof(GameOfLifeBoard))]
        [SwaggerResponse(404, "Board not found.")]
        [SwaggerResponse(500, "An error occurred while processing your request.")]
        public IActionResult GetNthState(string id, int n)
        {
            try
            {
                var board = _service.GetBoard(id);
                if (board == null)
                {
                    _logger.LogWarning("Board with ID {BoardId} not found.", id);
                    return NotFound();
                }

                var nthState = _service.GetNthState(board, n);
                _logger.LogInformation("{NthState}th state retrieved for board ID: {BoardId}", n, id);
                return Ok(nthState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the nth state.");
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("{id}/final/{maxSteps}")]
        [SwaggerOperation(Summary = "Gets the final state for a board.", Description = "Gets the final state for the specified board ID after a maximum number of steps.")]
        [SwaggerResponse(200, "Final state successfully retrieved.", typeof(GameOfLifeBoard))]
        [SwaggerResponse(400, "Board did not reach a final state within the specified max steps.")]
        [SwaggerResponse(404, "Board not found.")]
        [SwaggerResponse(500, "An error occurred while processing your request.")]
        public IActionResult GetFinalState(string id, int maxSteps)
        {
            try
            {
                var board = _service.GetBoard(id);
                if (board == null)
                {
                    _logger.LogWarning("Board with ID {BoardId} not found.", id);
                    return NotFound();
                }

                try
                {
                    var finalState = _service.GetFinalState(board, maxSteps);
                    _logger.LogInformation("Final state retrieved for board ID: {BoardId} after {MaxSteps} steps", id, maxSteps);
                    return Ok(finalState);
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogWarning(ex, "Board with ID {BoardId} did not reach a final state within {MaxSteps} steps", id, maxSteps);
                    return BadRequest(new { Error = ex.Message });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the final state.");
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }
    }
}
