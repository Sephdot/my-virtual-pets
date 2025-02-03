using Microsoft.AspNetCore.Mvc;
using my_virtual_pets_api.Services.Interfaces;

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

        [HttpPost(Name = "AddUser")]
        public IActionResult NewUser() // user DTO required
        {
            return Created();
        }

        //[HttpPost(Name = "UserLogin")]
        //public IActionResult Login() // login DTO required 
        //{
        //    return Ok();
        //}

        //
        // [HttpPost(Name = "PostGlobalUser")]
        // public IActionResult PostGlobalUser(InputGlobalUser userInput)
        // {
        //     GlobalUser newGlobalUser = new GlobalUser()
        //     {
        //         Username = userInput.Username,
        //         Email = userInput.Email,
        //         GDPRPermissions = userInput.GDPRPermissions,
        //         DateJoined = userInput.DateJoined
        //     };
        //     _context.GlobalUsers.Add(newGlobalUser);
        //     _context.SaveChanges();
        //     return Ok(newGlobalUser);
        // }
        //
        // [HttpGet(Name = "GetGlobalUsers")]
        // public IActionResult GetGlobalUsers()
        // {
        //     var users = _context.GlobalUsers.Include(g => g.Pets).ToList();
        //     return Ok(users);
        // }
        //
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
            if (result.Item1 == true)
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
