using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using my_virtual_pets_api.Data;
using my_virtual_pets_api.Services.Interfaces;

namespace my_virtual_pets_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetsController : ControllerBase 
    {
        private readonly IPetService _petService;
        
        public PetsController(IPetService petService)
        {
            _petService = petService;
        }
        
        [HttpGet("{userId}")]
        public IActionResult GetAllPetsByUserID(Guid userId)
        {
            var petCards = _petService.GetPetsByUser(userId);
            return Ok(petCards);
        }
    }
}
