using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PaymentProcedureCore.IService;
using PaymentProcedureData.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace PaymentProcedureAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILoginService loginService;
        public LoginController(ILoginService loginService, IConfiguration _config)
        {
            this.loginService = loginService;
            this._config = _config;
        }

        [HttpPost, Route("usercredential")]
        public async Task<IActionResult> TokenAuthentication([FromBody] UserLogin loginDetails)
        {
            if (await loginService.LoginCredentials(loginDetails))
            {
                return Ok(new { token = GenerateJWT(), UserLoginId = await loginService.UserIdByUserName(loginDetails.UserName) });
            }
            else
                return Unauthorized();
        }

        private string GenerateJWT()
        {
            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(issuer: _config["Jwt:Issuer"], audience: _config["Jwt:Audience"], expires: DateTime.Now.AddHours(24), signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])), SecurityAlgorithms.HmacSha256)));
        }
    }
}
