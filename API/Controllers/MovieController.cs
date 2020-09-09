using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Contracts.Requests;
using Models.Domain;
using Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class MovieController : ControllerBase
    {

        private readonly ILogger<MovieController> _logger;
        private readonly IMovieService _movieService;

        public MovieController(ILogger<MovieController> logger, IMovieService carService)
        {
            _logger = logger;
            _movieService = carService;
        }

        /// <summary>
        /// Get all movies by popularity
        /// </summary>
        [HttpGet]
        [Route("api/Movie/GetByPopularity")]
        public async Task<ActionResult> GetByPopularity()
        {
            APIResponse<List<Movie>> response = new APIResponse<List<Movie>>();

            try
            {
                response.Value = await _movieService.GetByPopularity();
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                _logger.LogError(ex.Message);
            }

            return BadRequest(response);
        }

        /// <summary>
        /// Get all movies by popularity
        /// </summary>
        [HttpPost]
        [Route("api/Movie/GetByKeyWords")]
        public async Task<ActionResult> GetByKeyWord([FromBody] GetMovieByKeyWord request)
        {
            APIResponse<List<Movie>> response = new APIResponse<List<Movie>>();

            try
            {
                response.Value = await _movieService.GetByKeyWord(request.SearchKeyWord);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                _logger.LogError(ex.Message);
            }

            return BadRequest(response);
        }
    }
}
