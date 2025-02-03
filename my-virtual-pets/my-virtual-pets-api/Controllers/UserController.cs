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
            if(_userService.ExistsByUsername(newUserDto.Username)) return BadRequest("Username already taken");
            
            _userService.CreateNewLocalUser(newUserDto);
            
            return Created("/register", "New local user created"); 
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginDTO userLoginDto) 
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!_userService.ExistsByUsername(userLoginDto.Username)) return BadRequest("This username does not exist");
            if (!_userService.DoesPasswordMatch(userLoginDto)) return BadRequest("Password is incorrect");

            // return token for sessions?
            return Ok("You are logged in.");
        }

        
        // [HttpGet(Name = "GetGlobalUsers")]
        // public IActionResult GetGlobalUsers()
        // {
        //     var users = _context.GlobalUsers.Include(g => g.Pets).ToList();
        //     return Ok(users);
        // }
        
        // [HttpGet("/local")]
        // public IActionResult GetLocalUsers()
        // {
        //     var users = _context.LocalUsers.Include(u => u.GlobalUser).ToList();
        //     return Ok(users);
        // }
        
        
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
