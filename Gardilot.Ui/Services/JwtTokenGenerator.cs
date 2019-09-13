using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Gardilot.Ui.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<JwtTokenGenerator> _logger;

        public JwtTokenGenerator(IOptions<AppSettings> appSettings, ILogger<JwtTokenGenerator> logger)
        {
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        public string GenerateJSONWebToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
            var signinCredentials  = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                issuer: _appSettings.Issuer,
                audience: _appSettings.Issuer,
                claims: new List<Claim>(),
                expires: DateTime.Now.AddDays(1),
                signingCredentials: signinCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        }

        public bool ValidateGenerateJSONWebToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out var _);
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex, $"Error while validating security token. {ex.Message}");
                return false;
            }

            return true;
        }

        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = _appSettings.Issuer,
                ValidAudience = _appSettings.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret)) // The same key as the one that generate the token
            };
        }
    }
}
