using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using my_virtual_pets_api.Data;
using my_virtual_pets_api.Entities;
using my_virtual_pets_api.TempClasses;

namespace my_virtual_pets_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase 
    {
        private readonly IDbContext _context;

        public UsersController(IDbContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetGlobalUsers")]
        public IActionResult GetGlobalUsers()
        {
            var users = _context.GlobalUsers.Include(g => g.Pets).ToList();
            return Ok(users);
        }

        [HttpGet("/local")]
        public IActionResult GetLocalUsers()
        {
            var users = _context.LocalUsers.Include(u => u.GlobalUser).ToList();
            return Ok(users);
        }

        [HttpGet("/pets")]
        public IActionResult GetPets()
        {
            var pets = _context.Pets.Include(u => u.Image).ToList();
            return Ok(pets);
        }

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



        [HttpPost(Name = "PostGlobalUser")]
        public IActionResult PostGlobalUser(InputGlobalUser userInput)
        {
            GlobalUser newGlobalUser = new GlobalUser()
            {
                Username = userInput.Username,
                Email = userInput.Email,
                GDPRPermissions = userInput.GDPRPermissions,
                DateJoined = userInput.DateJoined
            };
            _context.GlobalUsers.Add(newGlobalUser);
            _context.SaveChanges();
            return Ok(newGlobalUser);
        }

        [HttpGet("/image")]
        public IActionResult GetImage()
        {
            var image = _context.Images.First();
            //MemoryStream ms = new MemoryStream(image.ImageObj);
            //System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return Ok(File(image.ImageObj, "image/jpeg"));
        }


    }
}
