using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using my_virtual_pets_api.Services.Interfaces;
using my_virtual_pets_class_library.DTO;

namespace my_virtual_pets_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase 
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("register")]
        public IActionResult NewLocalUser(NewUserDTO newUserDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (_userService.ExistsByEmail(newUserDto.Email)) return BadRequest("Email already registered");
            if (_userService.ExistsByUsername(newUserDto.Username)) return BadRequest("Username already taken");

            _userService.CreateNewLocalUser(newUserDto);

            return Created("/register", "New local user created");
        }

        [HttpGet("login")]
        public IActionResult Forbidden()
        {
            return Ok("You need to log in");
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(UserLoginDTO userLoginDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!_userService.ExistsByUsername(userLoginDto.Username)) return BadRequest("This username does not exist");
            if (!_userService.DoesPasswordMatch(userLoginDto)) return BadRequest("Password is incorrect");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userLoginDto.Username),
                new Claim(ClaimTypes.Role, "User"),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2),
                IsPersistent = true,
                IssuedUtc = DateTimeOffset.UtcNow,
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Ok("You are logged in.");
        }

        [HttpGet("logout")]
        public async Task LogoutAsync()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
        }

        // [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpPut("/s3")]
        public async Task<IActionResult> UploadImageTest([FromBody] string keyName)
        {
            Cloud.S3StorageService s3 = new();
            var result = await s3.UploadFileAsync(keyName);
            if (result.Item1)
            {
                // if operation was successful
                return Ok(result.Item2);
            }
            else
            {
                return StatusCode(500, result.Item2);
            }
        }



    }
}
