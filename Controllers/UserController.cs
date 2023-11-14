using Hikers_Diary.DTO;
using Hikers_Diary.Interfaces;
using Hikers_Diary.Models;
using HikersDiary_Web.Data;
using HikersDiary_Web.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Security.Claims;
using System.Text;

namespace Hikers_Diary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase {

        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly MyDbContext _context;

        public UserController(IUserRepository userRepository, IConfiguration configuration, MyDbContext context)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _context = context;
        }

        
        [Authorize]
        [HttpGet]
        public IActionResult GetUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return BadRequest("User ID claim not found.");
            }

            var userId = Convert.ToInt32(userIdClaim.Value);
            var userDto = _userRepository.GetUserById(userId);

            if (userDto == null)
            {
                return NotFound();
            }

            return Ok(userDto);
        }

        [HttpGet("user/{id}")]
        public IActionResult GetUserbyId(int id) { 
             var user = _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("username/{username}")]
        public IActionResult GetUserByUsername(string username)
        {
            var user = _userRepository.GetUserByUsername(username);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("search/{searchQuery}")]
        public IActionResult SearchUsers(string searchQuery)
        {
            var users = _userRepository.SearchUsers(searchQuery);
            return Ok(users);
        }

        [HttpPost("Add")]
        public IActionResult AddUser([FromForm] UserDto user)
        {
            _userRepository.AddUser(user);
            var successMessage = "User registered successfully.";
            var response = new
            {
                Message = successMessage,
                
            };

            return Ok(response);

            /*return Created(nameof(GetUserbyId),new {id = user.UserId});*/
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromForm] UserDto user)
        {
            if (id != user.UserId)
                return BadRequest();

            _userRepository.UpdateUser(user);
            var successMessage = "User Updated successfully.";
            var response = new
            {
                Message = successMessage,

            };
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromForm] LoginModel loginModel)
        {
            var user = _userRepository.ValidateUser(loginModel.UserName, loginModel.Password);
            if (user != null)
            {
                var token = GenerateJwtToken(loginModel.UserName,user.UserId);
                return Ok(new { token });
            }
            else
            {
                return Unauthorized("Invalid username or password.");
            }
        }
        

         private string GenerateJwtToken(string username, int userId)
          {
            var secretKey = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.NameIdentifier,userId.ToString())
                
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
          }
        [HttpGet("random")]
        public IActionResult GetRandomUsers(int currentUserId)
        {
            
            var followedUserIds = _context.Follow
                .Where(f => f.Follower_Id == currentUserId)
                .Select(f => f.Following_Id)
                .ToList();

           
            var users = _context.User
                .Where(u => u.UserId != currentUserId && !followedUserIds.Contains(u.UserId))
                .ToList();

            var random = new Random();
            var randomUsers = users.OrderBy(x => random.Next()).Take(12).ToList();

            return Ok(randomUsers);
        }
    }
}

