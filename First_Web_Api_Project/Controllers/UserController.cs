using First_Web_Api_Project.Data;
using First_Web_Api_Project.Models;
using First_Web_Api_Project.Models.DTOs;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace First_Web_Api_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly PharmacyContext _context;

        public UserController(PharmacyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Users()
        {
            var user = _context.Users.ToList();
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Signup(UserDTO_s userdata)
        {
            var checkUser = _context.Users.FirstOrDefault(u => u.Email == userdata.Email);
            if (checkUser != null)
            {
                return BadRequest("User Already Exists...");
            }
            else
            {
                var hasher = new PasswordHasher<string>();
                var hashPass = hasher.HashPassword(userdata.Email,userdata.Password);

                User newUser = new User()
                {
                    Username = userdata.Username,
                    Email = userdata.Email,
                    Password = hashPass,
                    RoleId = userdata.RoleId
                };

                var addedUser = _context.Users.Add(newUser);
                _context.SaveChanges();
                return Ok(addedUser.Entity);
            }
        }

        [HttpPost("userdata")]
        public IActionResult Login(LoginDTO_s userdata)
        {
            if (userdata == null)
            {
                return BadRequest();
            }
            else
            {
                var check = _context.Users.FirstOrDefault(u =>u.Email == userdata.Email);
                if (check != null)
                {
                    var hasher = new PasswordHasher<string>();
                    var result = hasher.VerifyHashedPassword(userdata.Email,check.Password,userdata.Password);
                    if (result == PasswordVerificationResult.Success)
                    {
                        return Ok("Login Success");
                    }
                    else
                    {
                        return Unauthorized("Invalid Credentials");
                    }
                }
                else
                {
                    return BadRequest("User Not Found");
                }
            }
        }
    }
}
