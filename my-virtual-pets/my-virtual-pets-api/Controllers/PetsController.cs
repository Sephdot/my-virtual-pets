using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using my_virtual_pets_api.Data;
using my_virtual_pets_api.Services.Interfaces;
using my_virtual_pets_class_library.DTO;
using my_virtual_pets_class_library.Enums;

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


        [HttpGet]
        public IActionResult GetPets()
        {
            try
            {
                var pets = _petService.GetPets();
                return Ok(pets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching pets: {ex.Message}");
            }
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetAllPetsByUserID(Guid userId)
        {
            try
            {
                if (userId == Guid.Empty)
                {
                    return BadRequest(new { message = "Invalid user ID." });
                }

                var pets = _petService.GetPetsByUser(userId);
                return Ok(pets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving pets: {ex.Message}");
            }
        }

        [HttpGet("{petId}")]
        public IActionResult GetPetById(Guid petId)
        {
            try
            {
                if (petId == Guid.Empty)
                {
                    return BadRequest(new { message = "Invalid pet ID." });
                }

                var pet = _petService.GetPetById(petId);
                if (pet == null)
                {
                    return NotFound(new { message = "Pet not found." });
                }

                return Ok(pet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the pet.", error = ex.Message });
            }

        }

        [HttpPost]
        public IActionResult AddPet(AddPetDTO addPetDTO)
        {
            try
            {
                PetCardDataDTO addedPet = _petService.AddPet(addPetDTO);
                return Ok(addedPet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding pet: {ex.Message}");
            }
        }

        [HttpGet("top10")]
        public ActionResult<List<PetCardDataDTO>> GetTop10Pets()
        {
            try
            {
                var pets = _petService.GetTop10Pets();
                return Ok(pets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the top pets: {ex.Message}");
            }
        }

        [HttpGet("recent")]
        public ActionResult<List<PetCardDataDTO>> GetRecentPets()
        {
            var pets = _petService.GetRecentPets();
            return Ok(pets);
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
