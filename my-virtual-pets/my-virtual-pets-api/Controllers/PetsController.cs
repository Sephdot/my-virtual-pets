using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using my_virtual_pets_api.Data;

namespace my_virtual_pets_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetsController : ControllerBase 
    {
        private readonly IDbContext _context;

        public PetsController(IDbContext context)
        {
            _context = context;
        }


        [HttpGet("/pets")]
        public IActionResult GetPets()
        {
            var pets = _context.Pets.Include(u => u.Image).ToList();
            return Ok(pets);
        }
<<<<<<< Updated upstream
=======

        [HttpGet("user/{userId}")]
        public IActionResult GetAllPetsByUserID(Guid userId)
        {
            var petCards = _petService.GetPetsByUser(userId);
            return Ok(petCards);
        }

        [HttpGet("{petId}")]
        public IActionResult GetPetById(Guid petId)
        {
                var pet = _petService.GetPetById(petId);
                return Ok(pet);
        }
>>>>>>> Stashed changes
    }
}
