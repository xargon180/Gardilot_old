using Gardilot.Ui.ViewModels;
using Gardilot.Ui.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Gardilot.Ui.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController  : ControllerBase
    {
        private IJwtTokenGenerator _jwtService;
        private readonly IOptions<AppSettings> _options;

        public AuthController (IJwtTokenGenerator userService, IOptions<AppSettings> options)
        {
            _jwtService = userService;
            _options = options;
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult Login([FromBody]LoginViewModel login)
        {
            if (login == null)
            {
                return BadRequest("Invalid client request");
            }

            if (_options.Value.Users.Any(u => u.UserName.Equals(login.UserName, StringComparison.OrdinalIgnoreCase) && u.Password.Equals(login.Password)))
            {
                var tokenString = _jwtService.GenerateJSONWebToken();
                return Ok(new { Token = tokenString });
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
