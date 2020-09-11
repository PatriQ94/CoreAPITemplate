using Models.Contracts.Requests;
using Models.Domain;
using Models.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<CarController> _logger;
        private readonly IUserService _userService;
        private readonly UserManager<IdentityUser> _userManager;


        public UserController(ILogger<CarController> logger, IUserService userService, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _userService = userService;
            _userManager = userManager;
        }

        /// <summary>
        /// Adds a movie to the users list of favourite movies
        /// </summary>
        [HttpPost]
        [Route("api/User/AddMovieToFavourites")]
        public async Task<ActionResult> AddMovieToFavourites([FromBody] MovieIdRequest request)
        {
            try
            {
                int response = await _userService.AddMovieToFavouritesAsync(_userManager.GetUserId(User), request.MovieId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Returns a list of all favourite movies for the user
        /// </summary>
        [HttpGet]
        [Route("api/User/GetFavouriteMoviesByUser")]
        public async Task<ActionResult> GetFavouriteMoviesByUser()
        {
            try
            {
                List<Movie> response = await _userService.GetFavouriteMoviesByUserAsync(_userManager.GetUserId(User));
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Removes a movie from the users list of favourite movies
        /// </summary>
        [HttpPost]
        [Route("api/User/RemoveMovieFromFavourites")]
        public async Task<ActionResult> RemoveMovieFromFavourites([FromBody] MovieIdRequest request)
        {
            try
            {
                string response = await _userService.RemoveMovieFromFavouritesAsync(_userManager.GetUserId(User), request.MovieId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Adds a movie to the users list of seen movies
        /// </summary>
        [HttpPost]
        [Route("api/User/AddMovieToSeen")]
        public async Task<ActionResult> AddMovieToSeen([FromBody] MovieIdRequest request)
        {
            try
            {
                int response = await _userService.AddMovieToSeenAsync(_userManager.GetUserId(User), request.MovieId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Returns a list of all seen movies for the user
        /// </summary>
        [HttpGet]
        [Route("api/User/GetSeenMoviesByUser")]
        public async Task<ActionResult> GetSeenMoviesByUser()
        {
            try
            {
                List<Movie> response = await _userService.GetSeenMoviesByUserAsync(_userManager.GetUserId(User));
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Removes a movie from the users list of seen movies
        /// </summary>
        [HttpPost]
        [Route("api/User/RemoveMovieFromSeenList")]
        public async Task<ActionResult> RemoveMovieFromSeenList([FromBody] MovieIdRequest request)
        {
            try
            {
                string response = await _userService.RemoveMovieFromSeenListAsync(_userManager.GetUserId(User), request.MovieId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
