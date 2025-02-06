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

        [HttpGet("top10")]
        public ActionResult<List<PetCardDataDTO>> GetTop10Pets()
        {
            var pets = _petService.GetTop10Pets();
            return Ok(pets);
        }
    }
}
//[HttpGet("top10")]
//public IActionResult GetTop10Pets()
//{
//    // Dummy data
//    var pets = new List<PetCardDataDTO>
//            {
//                new PetCardDataDTO { PetId = Guid.NewGuid(), PetName = "Max", ImageUrl = "url1", OwnerUsername = "Alice", Score = 90, Personality = Personality.BRAVE, PetType = PetType.DOG, Description = "Loyal dog", IsFavourited = true },
//                new PetCardDataDTO { PetId = Guid.NewGuid(), PetName = "Bella", ImageUrl = "url2", OwnerUsername = "Bob", Score = 85, Personality = Personality.QUIET, PetType = PetType.CAT, Description = "Cute cat", IsFavourited = false },
//                new PetCardDataDTO { PetId = Guid.NewGuid(), PetName = "Charlie", ImageUrl = "url3", OwnerUsername = "Eve", Score = 95, Personality = Personality.HASTY, PetType = PetType.BIRD, Description = "Smart bird", IsFavourited = true },
//                new PetCardDataDTO { PetId = Guid.NewGuid(), PetName = "Luna", ImageUrl = "url4", OwnerUsername = "David", Score = 100, Personality = Personality.BOLD, PetType = PetType.DOG, Description = "Energetic puppy", IsFavourited = false },
//                new PetCardDataDTO { PetId = Guid.NewGuid(), PetName = "Milo", ImageUrl = "url5", OwnerUsername = "Sam", Score = 80, Personality = Personality.CALM, PetType = PetType.CAT, Description = "Relaxed cat", IsFavourited = true },
//                new PetCardDataDTO { PetId = Guid.NewGuid(), PetName = "Oscar", ImageUrl = "url6", OwnerUsername = "Mike", Score = 75, Personality = Personality.QUIET, PetType = PetType.FISH, Description = "Colorful fish", IsFavourited = false },
//                new PetCardDataDTO { PetId = Guid.NewGuid(), PetName = "Daisy", ImageUrl = "url7", OwnerUsername = "Lily", Score = 88, Personality = Personality.HASTY, PetType = PetType.DOG, Description = "Loves the outdoors", IsFavourited = true },
//                new PetCardDataDTO { PetId = Guid.NewGuid(), PetName = "Rocky", ImageUrl = "url8", OwnerUsername = "Emma", Score = 92, Personality = Personality.BRAVE, PetType = PetType.CAT, Description = "Fearless", IsFavourited = false },
//                new PetCardDataDTO { PetId = Guid.NewGuid(), PetName = "Toby", ImageUrl = "url9", OwnerUsername = "Olivia", Score = 77, Personality = Personality.CHEERFUL, PetType = PetType.BIRD, Description = "Loves to sing", IsFavourited = false },
//                new PetCardDataDTO { PetId = Guid.NewGuid(), PetName = "Coco", ImageUrl = "url10", OwnerUsername = "Sophia", Score = 99, Personality = Personality.HASTY, PetType = PetType.DOG, Description = "Loves company", IsFavourited = true },
//                new PetCardDataDTO { PetId = Guid.NewGuid(), PetName = "Shadow", ImageUrl = "url11", OwnerUsername = "Henry", Score = 65, Personality = Personality.QUIET, PetType = PetType.CAT, Description = "Silent and smart", IsFavourited = true }
//            };

//    // Sort by Score descending and take the top 10
//    var top10Pets = pets.OrderByDescending(p => p.Score).Take(10).ToList();

//    return Ok(top10Pets);
//}

//[HttpGet("{userId}")]
//public IActionResult GetUserDetailsByUserId(Guid userId)
//{
//    // Dummy data
//    var users = new List<UserDisplayDTO>
//            {
//                new UserDisplayDTO { Username = "john_doe", FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PetCount = 3 },
//                new UserDisplayDTO { Username = "jane_smith", FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", PetCount = 5 },
//                new UserDisplayDTO { Username = "mike_ross", FirstName = "Mike", LastName = "Ross", Email = "mike.ross@example.com", PetCount = 2 }
//            };

//    // Find user by ID (simulating userId matching)
//    var user = users.Find(u => u.Username.Contains(userId.ToString().Substring(0, 4))); // Simulated lookup

//    if (user == null)
//    {
//        return NotFound("User not found.");
//    }

//    return Ok(user);
//}