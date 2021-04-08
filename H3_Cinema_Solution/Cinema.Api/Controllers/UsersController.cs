using Cinema.Api.Models;
using Cinema.Data;
using Cinema.Domain.DTOs;
using Cinema.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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

        [Authorize]
        [HttpGet("TokenValidate")]
        public ActionResult<bool> ValidateToken()
        {
            return true;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login([FromBody] User user)
        {
            user = await _context.Users.Include(x => x.Customer)
                .Where(x => x.Username == user.Username && x.Password == user.Password).FirstOrDefaultAsync();

            if (user == null)
            {
                return Unauthorized();
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

        [HttpGet("CheckUserName")]
        public async Task<ActionResult<bool>> CheckUserName(string username) //Rewrite maybe 
        {
            var userlist = await _context.Users.Where(x => x.Username == username).ToListAsync();
            bool result = false;


            foreach (var item in userlist.Where(item => item.Username == username))
            {
                result = true;
            }

            return result;
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            //Check that there is no already a customer assigned to user account
            var userCustomer = await _context.Users.Where(x => x.CustomerId == user.CustomerId).ToListAsync();

            if (userCustomer.Count != 0)
            {
                return Conflict();
            }
            

            ////Check that the User does not exist.
            var userName = await _context.Users.Where(x=> x.Username == user.Username).ToListAsync();

            if (userName.Count != 0)
            {
                return Conflict();
            }

            user.Rights = 0;
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            user.Password = "*************";
            
            return user;
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Checks if the customerId isn't the same as the queried Id and if the user isn't Admin.
            if (!User.IsInRole("Admin") && User.Identity.Name != id.ToString())
            {
                return Unauthorized();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
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
                Expires = DateTime.UtcNow.AddDays(14),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
