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
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ILogger<CarController> _logger;
        private readonly ICarService _carService;
        private readonly UserManager<IdentityUser> _userManager;

        public CarController(ILogger<CarController> logger, ICarService carService, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _carService = carService;
            _userManager = userManager;
        }

        /// <summary>
        /// Insert a new car for user
        /// </summary>
        [HttpPost]
        [Route("api/Car/Insert")]
        public async Task<ActionResult> Insert([FromBody] InsertCarRequest request)
        {
            APIResponse<long> response = new APIResponse<long>();

            try
            {
                response.Value = await _carService.InsertAsync(_userManager.GetUserId(User), request.Brand, request.Model, request.Year, request.Kilometers);
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
        /// Remove users car by ID
        /// </summary>
        [HttpPost]
        [Route("api/Car/Remove")]
        public async Task<ActionResult> Remove([FromBody] RemoveCarRequest request)
        {
            APIResponse<string> response = new APIResponse<string>();

            try
            {
                response.Value = await _carService.RemoveAsync(_userManager.GetUserId(User), request.CarID);
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
        /// Get all cars for user
        /// </summary>
        [HttpGet]
        [Route("api/Car/GetByUser")]
        public async Task<ActionResult> GetByUser()
        {
            APIResponse<List<Models.Domain.Car>> response = new APIResponse<List<Models.Domain.Car>>();

            try
            {
                response.Value = await _carService.GetByUserAsync(_userManager.GetUserId(User));
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
        /// Update car kilometers
        /// </summary>
        [HttpPost]
        [Route("api/Car/Update_Kilometers")]
        public async Task<ActionResult> Update_Kilometers([FromBody] UpdateCarKilometersRequest request)
        {
            APIResponse<string> response = new APIResponse<string>();

            try
            {
                response.Value = await _carService.Update_KilometersAsync(_userManager.GetUserId(User), request.CarID, request.Kilometers);
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
