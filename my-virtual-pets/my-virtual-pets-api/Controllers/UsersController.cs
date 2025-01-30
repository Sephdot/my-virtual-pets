using Microsoft.AspNetCore.Mvc;
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
            var users = _context.GlobalUsers.ToList();
            return Ok(users);
        }

        [HttpPost(Name = "PostGloablUser")]
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

    }
}
