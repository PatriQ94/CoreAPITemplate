using Models;
using Models.Contracts.Requests;
using Models.Contracts.Responses;
using Models.Domain;
using Models.Options;
using Models.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private JwtSettings _jwtSettings { get; }

        //Dependency injection
        public AuthService(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, JwtSettings jwtSettings, TokenValidationParameters tokenValidationParameters)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _tokenValidationParameters = tokenValidationParameters;
        }

        public async Task<AuthenticationResult> RegisterAsync(string email, string password)
        {
            //Check if user already exists
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return new AuthenticationResult()
                {
                    Errors = new List<string>() { "An error occured. Please use another email." }
                };
            }

            //Create new user
            var newUser = new IdentityUser()
            {
                Email = email,
                UserName = email
            };

            var createdUser = await _userManager.CreateAsync(newUser, password);

            //Check if successfully created
            if (createdUser.Succeeded == false)
            {
                return new AuthenticationResult()
                {
                    Errors = createdUser.Errors.Select(x => x.Description).ToList()
                };
            }

            //Generate new access token with user credentials
            return await GenerateJWTAccessToken(newUser);
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            //Check if user already exists
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new AuthenticationResult()
                {
                    Errors = new List<string>() { "Email or password is incorrect" }
                };
            }

            //Check if password is correct
            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);
            if (userHasValidPassword == false)
            {
                return new AuthenticationResult()
                {
                    Errors = new List<string>() { "Email or password is incorrect" }
                };
            }

            //Generate new access token with user credentials
            return await GenerateJWTAccessToken(user);
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            //Manually validate access token
            var validatedToken = GetPrincipalFromToken(accessToken);
            if (validatedToken == null)
            {
                return new AuthenticationResult()
                {
                    Errors = new List<string>() { "Invalid access token" }
                };
            }

            //Validate if the access token has already expired or not
            var dateEndUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            DateTime dateEndUTC = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(dateEndUnix);
            if (dateEndUTC > DateTime.UtcNow)
            {
                return new AuthenticationResult()
                {
                    Errors = new List<string>() { "The access token hasn't expired yet" }
                };
            }

            //Get the refresh token from the database
            var storedRefreshToken = await _unitOfWork.Auth.GetRefreshTokenAsync(refreshToken);
            if (storedRefreshToken == null)
            {
                return new AuthenticationResult()
                {
                    Errors = new List<string>() { "This refresh token does not exist" }
                };
            }

            //Check if refresh token has expired
            if (DateTime.UtcNow > storedRefreshToken.DateEnd) {
                return new AuthenticationResult()
                {
                    Errors = new List<string>() { "This refresh token has expired" }
                };
            }

            //Check if token has been manually invalidated
            if (storedRefreshToken.Invalidated == true)
            {
                return new AuthenticationResult()
                {
                    Errors = new List<string>() { "This refresh token has been invalidated" }
                };
            }

            //Check if token has been manually used
            if (storedRefreshToken.Used == true)
            {
                return new AuthenticationResult()
                {
                    Errors = new List<string>() { "This refresh token has already been used" }
                };
            }

            //Validate if the access token and refresh token are the same pair
            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            if (storedRefreshToken.JwtId != jti)
            {
                return new AuthenticationResult()
                {
                    Errors = new List<string>() { "This refresh token does not match this JWT" }
                };
            }

            //The refresh token is valid, update that it has been used
            storedRefreshToken.Used = true;
            bool updated = await _unitOfWork.Auth.UpdateRefreshTokenAsync(storedRefreshToken);
            if (updated == false)
            {
                return new AuthenticationResult()
                {
                    Errors = new List<string>() { "Error when updating refresh token" }
                };
            }

            //Generate new access token with user credentials
            var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Sub).Value);
            return await GenerateJWTAccessToken(user);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            //Override Microsoft defaults exaplained here https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/415
            tokenHandler.InboundClaimFilter.Clear();
            tokenHandler.InboundClaimTypeMap.Clear();
            tokenHandler.OutboundClaimTypeMap.Clear();
            try
            {
                var tokenValidationParameters = _tokenValidationParameters.Clone();
                tokenValidationParameters.ValidateLifetime = false;
                var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var validatedToken);
                if (IsJwtWithValidSecurityAlhorithm(validatedToken) == false)
                {
                    return null;
                }
                return principal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool IsJwtWithValidSecurityAlhorithm(SecurityToken validatedToken) {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) 
                && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }

        public async Task<AuthenticationResult> GenerateJWTAccessToken(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            //Get secret key
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

            //Signing credentials
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Information about the user
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString() )
            };

            //Generate access token
            var accessToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Issuer,
                claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddSeconds(_jwtSettings.LifeTime),
                signingCredentials: credentials
            );

            //Generate refresh token
            var refreshToken = new RefreshToken()
            {
                JwtId = accessToken.Id,
                UserId = user.Id,
                DateStart = DateTime.UtcNow,
                DateEnd = DateTime.UtcNow.AddMonths(6)
            };

            //Save refresh token
            string refreshTokenId = await _unitOfWork.Auth.SaveRefreshTokenAsync(refreshToken);
            if (string.IsNullOrEmpty(refreshTokenId))
            {
                return new AuthenticationResult()
                {
                    Errors = new List<string>() { "Error occured when saving refresh token" }
                };
            }

            //Encode the access token and return
            return new AuthenticationResult()
            {
                Success = true,
                AccessToken = tokenHandler.WriteToken(accessToken),
                RefreshToken = refreshTokenId
            };
        }
    }
}
