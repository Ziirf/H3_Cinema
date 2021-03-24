using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Cinema.Domain.DTOs;
using Cinema.Data;
using Cinema.Api.models;
using Cinema.Domain.Models;

namespace Cinema.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CinemaContext _context;
        private readonly JWTSettings _jwtsettings;

        public UsersController(CinemaContext context, IOptions<JWTSettings> jwtsettings)
        {
            _context = context;
            _jwtsettings = jwtsettings.Value;
        }


        //[HttpGet("Login")]
        //public async Task<ActionResult<UserDTO>> Login([FromBody] User user)
        //{
        //    user = await _context.Users.Include(x => x.Customer)
        //        .Where(x => x.Username == user.Username && x.Password == user.Password).FirstOrDefaultAsync();

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    var token = GenerateAccessToken(user);

        //    var userDTO = new UserDTO()
        //    {
        //        Id = user.Id,
        //        Username = user.Username,
        //        Rights = user.Rights,
        //        CustomerId = user.Customer.Id,
        //        Token = token
        //    };

        //    return userDTO;
        //}

        [HttpGet("Login")]
        public async Task<ActionResult<UserDTO>> Login(string username, string password)
        {
            User user = _context.Users.FirstOrDefault(x => x.Username == username);

            user = await _context.Users.Include(x => x.Customer)
                .Where(x => x.Username == username && x.Password == password).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            var token = GenerateAccessToken(user);

            var userDTO = new UserDTO()
            {
                Id = user.Id,
                Username = user.Username,
                Rights = user.Rights,
                CustomerId = user.Customer.Id,
                Token = token
            };

            return userDTO;
        }

        private string GenerateAccessToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtsettings.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, Convert.ToString(user.Id)),
                    new Claim("CustomerId", Convert.ToString(user.Customer.Id)),
                    new Claim(ClaimTypes.Role, Convert.ToString(user.Rights))
                }),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
