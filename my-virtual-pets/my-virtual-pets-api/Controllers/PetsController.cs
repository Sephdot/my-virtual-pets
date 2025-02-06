using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using my_virtual_pets_api.Data;
using my_virtual_pets_api.Services.Interfaces;
using my_virtual_pets_class_library.DTO;

namespace my_virtual_pets_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetsController : ControllerBase
    {
        private readonly IPetService _petService;


        public PetsController( IPetService petService)
        {
            _petService = petService;
        }


        [HttpGet]
        public IActionResult GetPets()
        {
            var pets = _petService.GetPets();
            return Ok(pets);
        }

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

        [HttpPost]
        public IActionResult AddPet(AddPetDTO addPetDTO)
        {
            PetCardDataDTO addedPet = _petService.AddPet(addPetDTO);
            return Ok(addedPet);
        }

        [HttpDelete("{petId}")]
        public IActionResult DeletePet(Guid petId)
        {
            bool isDeleted = _petService.DeletePet(petId);
            if (isDeleted) return NoContent();
            return NotFound();
        }
    }
}
