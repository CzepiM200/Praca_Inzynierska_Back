using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Praca_dyplomowa.Context;
using Praca_dyplomowa.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Praca_dyplomowa.Controllers
{
    [Route("auth")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly ProgramContext _context;

        public TokenController(IConfiguration config, ProgramContext context)
        {
            _configuration = config;
            _context = context;
        }

        [HttpPost, Route("login")]
        public IActionResult Post(LoginModel _userData)
        {
            if(_userData == null)
                return BadRequest("Invalid credentials");
            if(_userData.UserEmail == "maslo123" && _userData.Password == "def@123")
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokenOptions = new JwtSecurityToken(
                   issuer: "https://localhost:5001",
                   audience: "https://localhost:5001",
                   claims: new List<Claim>(),
                   expires: DateTime.Now.AddMinutes(15),
                   signingCredentials: signingCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(new { Token = tokenString });
            }

            return Unauthorized();

            //if (_userData != null && _userData.Email != null && _userData.Password != null)
            //{
            //    var user = await GetUser(_userData.Email, _userData.Password);

            //    if (user != null)
            //    {
            //        //create claims details based on the user information
            //        var claims = new[] {
            //        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
            //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            //        new Claim("Id", user.Id.ToString()),
            //        new Claim("FirstName", user.FirstName),
            //        new Claim("LastName", user.LastName),
            //        new Claim("UserName", user.UserName),
            //        new Claim("Email", user.Email)
            //       };

            //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            //        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

            //        return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            //    }
            //    else
            //    {
            //        return BadRequest("Invalid credentials");
            //    }
            //}
            //else
            //{
            //    return BadRequest();
            //}
        }
    }
}
