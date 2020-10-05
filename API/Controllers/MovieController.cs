using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser> _userManager;

        public MovieController(ILogger<MovieController> logger, IMovieService carService, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _movieService = carService;
            _userManager = userManager;
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
                response.Value = await _movieService.GetByKeyWord(_userManager.GetUserId(User), request.SearchKeyWord, request.Page);
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
