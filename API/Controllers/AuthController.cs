using Models.Contracts.Requests;
using Models.Contracts.Responses;
using Models.Domain;
using Models.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        [HttpPost]
        [Route("api/Auth/Register")]
        public async Task<ActionResult> Register([FromBody] RegistrationRequest request)
        {
            APIResponse<AuthenticationResponse> response = new APIResponse<AuthenticationResponse>();

            try
            {
                var authResponse = await _authService.RegisterAsync(request.Email, request.Password);
                if (authResponse.Success)
                {
                    response.Value = new AuthenticationResponse() {
                        AccessToken = authResponse.AccessToken,
                        RefreshToken = authResponse.RefreshToken
                    };
                    return Ok(response);
                }

                response.ErrorMessage = string.Join(',', authResponse.Errors);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                _logger.LogError(ex.Message);
            }

            return BadRequest(response);
        }

        /// <summary>
        /// Login with an existing user
        /// </summary>
        /// <remarks> If login is successful, an access token and a refresh token will be returned </remarks>
        [HttpPost]
        [Route("api/Auth/Login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            APIResponse<AuthenticationResponse> response = new APIResponse<AuthenticationResponse>();

            try
            {
                var authResponse = await _authService.LoginAsync(request.Email, request.Password);
                if (authResponse.Success)
                {
                    response.Value = new AuthenticationResponse()
                    {
                        AccessToken = authResponse.AccessToken,
                        RefreshToken = authResponse.RefreshToken
                    };
                    return Ok(response);
                }

                response.ErrorMessage = string.Join(',', authResponse.Errors);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                _logger.LogError(ex.Message);
            }

            return BadRequest(response);
        }

        /// <summary>
        /// Refresh access token
        /// </summary>
        /// <remarks> Exchanges refresh token for a newly generated access token </remarks>
        [HttpPost]
        [Route("api/Auth/RefreshToken")]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            APIResponse<AuthenticationResponse> response = new APIResponse<AuthenticationResponse>();

            try
            {
                var authResponse = await _authService.RefreshTokenAsync(request.AccessToken, request.RefreshToken);
                if (authResponse.Success)
                {
                    response.Value = new AuthenticationResponse()
                    {
                        AccessToken = authResponse.AccessToken,
                        RefreshToken = authResponse.RefreshToken
                    };
                    return Ok(response);
                }

                response.ErrorMessage = string.Join(',', authResponse.Errors);
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
