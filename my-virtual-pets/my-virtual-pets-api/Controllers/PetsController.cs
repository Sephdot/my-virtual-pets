using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using my_virtual_pets_api.Services.Interfaces;
using my_virtual_pets_class_library.DTO;

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
        public async Task<IActionResult> GetPets()
        {
            try
            {
                var pets = await _petService.GetPets();
                return Ok(pets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching pets: {ex.Message}");
            }
        }

        [Authorize(AuthenticationSchemes = "loginjwt")]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllPetsByUserID(Guid userId)
        {
            try
            {
                if (userId == Guid.Empty)
                {
                    return BadRequest(new { message = "Invalid user ID." });
                }

                var pets = await _petService.GetPetsByUser(userId);
                return Ok(pets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving pets: {ex.Message}");
            }
        }

        [Authorize(AuthenticationSchemes = "loginjwt")]
        [HttpGet("{petId}")]
        public async Task<IActionResult> GetPetById(Guid petId)
        {
            try
            {
                if (petId == Guid.Empty)
                {
                    return BadRequest("Invalid pet ID.");
                }

                var pet = await _petService.GetPetById(petId);
                if (pet == null)
                {
                    return NotFound("Pet not found.");
                }

                return Ok(pet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the pet: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPet(AddPetDTO addPetDTO)
        {
            try
            {
                PetCardDataDTO addedPet = await _petService.AddPet(addPetDTO);
                return Ok(addedPet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding pet: {ex.Message}");
            }
        }

        [HttpGet("top10")]
        public async Task<ActionResult<List<PetCardDataDTO>>> GetTop10Pets()
        {
            try
            {
                var pets = await _petService.GetTop10Pets();
                if (pets == null) return BadRequest("Less than 4 pets in the database");
                return Ok(pets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the top pets: {ex.Message}");
            }
        }

        [HttpGet("recent")]
        public async Task<ActionResult<List<PetCardDataDTO>>> GetRecentPets()
        {
            try
            {
                var pets = await _petService.GetRecentPets();
                if (pets == null) return BadRequest("There are less than 2 pets in the database");
                return Ok(pets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the most recent pets: {ex.Message}");
            }
        }


        [HttpDelete("{petId}")]
        public async Task<IActionResult> DeletePet(Guid petId)
        {
            try
            {
                bool isDeleted = await _petService.DeletePet(petId);
                if (isDeleted) return NoContent();
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the pet: {ex.Message}");
            }
        }
    }
}
