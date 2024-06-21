using GameOfLife.Models;
using GameOfLife.Services;
using GameOfLife.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameOfLife.Controllers
{
    /// <summary>
    /// Controller for managing Game of Life boards.
    /// </summary>
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

        /// <summary>
        /// Uploads a new board state and returns the ID of the created board.
        /// </summary>
        /// <param name="board">The board to upload.</param>
        /// <returns>The ID of the created board.</returns>
        [HttpPost("upload")]
        [SwaggerOperation(Summary = "Uploads a new board state.", Description = "Uploads a new board state and returns the ID of the created board.")]
        [SwaggerResponse(200, "Board successfully uploaded.", typeof(object))]
        [SwaggerResponse(400, "Invalid board state.")]
        [SwaggerResponse(500, "An error occurred while processing your request.")]
        public IActionResult UploadBoard([FromBody] GameOfLifeBoard board)
        {
            try
            {
                var validationResults = BoardValidator.Validate(board);
                if (validationResults.Count > 0)
                {
                    _logger.LogWarning("Invalid board state: {ModelStateErrors}", validationResults);
                    return BadRequest(validationResults);
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

        /// <summary>
        /// Gets the next state for a board.
        /// </summary>
        /// <param name="id">The ID of the board.</param>
        /// <returns>The next state of the board.</returns>
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

        /// <summary>
        /// Gets the nth state for a board.
        /// </summary>
        /// <param name="id">The ID of the board.</param>
        /// <param name="n">The number of steps to calculate.</param>
        /// <returns>The nth state of the board.</returns>
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
                _logger.LogInformation("{NthState}th state retrieved for board ID {BoardId}.", n, id);
                return Ok(nthState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the nth state.");
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        /// <summary>
        /// Gets the final state for a board.
        /// </summary>
        /// <param name="id">The ID of the board.</param>
        /// <param name="maxSteps">The maximum number of steps to calculate.</param>
        /// <returns>The final state of the board.</returns>
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
                    _logger.LogInformation("Final state retrieved for board ID {BoardId} after {MaxSteps} steps", id, maxSteps);
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
