using FluentAssertions;
using Moq;
using my_virtual_pets_api.Repositories.Interfaces;
using my_virtual_pets_api.Services;
using my_virtual_pets_api.Services.Interfaces;
using my_virtual_pets_class_library.DTO;
using my_virtual_pets_class_library.Enums;

namespace my_virtual_pets.Tests
{
    public class PetServiceTests
    {
        private Mock<IPetRepository> _petRepositoryMock;
        private Mock<IImagesService> _imagesServiceMock;
        private IPetService _petService;

        [SetUp]
        public void Setup()
        {

            _petRepositoryMock = new Mock<IPetRepository>();
            _imagesServiceMock = new Mock<IImagesService>();



            _petService = new PetService(_petRepositoryMock.Object, _imagesServiceMock.Object);
        }
        //GetPetsByUserId
        [Test]
        public async Task GetPetsByUser_ReturnsListOfPets_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var petDtos = new List<PetCardDataDTO>
            {
                new PetCardDataDTO
                {
                    PetId = Guid.NewGuid(),
                    PetName = "TestPet",
                    ImageUrl = "Test/image.png",
                    OwnerUsername = "TestOwner",
                    Score = 0,
                    Personality = Personality.BRAVE,
                    PetType = PetType.DOG,
                    Description = "Testdescription",
                    IsFavourited = false
                }
            };

            _petRepositoryMock.Setup(r => r.GetAllPetsByUserID(userId))
                             .ReturnsAsync(petDtos);

            // Act
            var result = await _petService.GetPetsByUser(userId);

            // Assert 
            result.Should().BeEquivalentTo(petDtos);
        }

        [Test]
        public async Task GetAllPetsByUserID_ReturnsEmptyList_WhenUserHasNoPets()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _petRepositoryMock.Setup(r => r.GetAllPetsByUserID(userId)).ReturnsAsync(new List<PetCardDataDTO>());

            // Act
            var result = await _petService.GetPetsByUser(userId);

            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public async Task GetAllPetsByUserID_HandlesInvalidUserId()
        {
            // Arrange
            _petRepositoryMock.Setup(r => r.GetAllPetsByUserID(Guid.Empty)).ReturnsAsync(new List<PetCardDataDTO>());

            // Act
            var result = await _petService.GetPetsByUser(Guid.Empty);

            // Assert
            result.Should().BeEmpty();
        }


        // GetPetsById
        [Test]
        public async Task GetPetById_ReturnsPet_WhenPetExists()
        {
            // Arrange
            var petId = Guid.NewGuid();
            var expectedPet = new PetCardDataDTO
            {
                PetId = petId,
                PetName = "TestPet",
                ImageUrl = "Test/image.png",
                OwnerUsername = "TestOwner",
                Score = 0,
                Personality = Personality.BRAVE,
                PetType = PetType.DOG,
                Description = "Testdescription",
                IsFavourited = false
            };


            _petRepositoryMock.Setup(r => r.GetPetById(petId))
                             .ReturnsAsync(expectedPet);

            // Act
            var result = await _petService.GetPetById(petId);

            // Assert 
            result.Should().BeEquivalentTo(expectedPet);
        }

        [Test]
        public async Task GetPetById_HandlesInvalidId()
        {
            // Arrange
            _petRepositoryMock.Setup(r => r.GetPetById(Guid.Empty)).ReturnsAsync((PetCardDataDTO?)null);

            // Act
            var result = await _petService.GetPetById(Guid.Empty);

            // Assert
            result.Should().BeNull();
        }
        [Test]
        public async Task GetPetById_ReturnsNull_WhenPetDoesNotExist()
        {
            // Arrange
            var petId = Guid.NewGuid();
            _petRepositoryMock.Setup(r => r.GetPetById(petId)).ReturnsAsync((PetCardDataDTO?)null);

            // Act
            var result = await _petService.GetPetById(petId);

            // Assert
            result.Should().BeNull();
        }



        //GetTop10Pets
        [Test]
        public async Task GetTop10Pets_ReturnsListOfPets()
        {
            // Arrange
            var listOfPets = new List<PetCardDataDTO>
            {
                new PetCardDataDTO
                {
                    PetId = Guid.NewGuid(),
                    PetName = "TopPet1",
                    ImageUrl = "Test/image1.png",
                    OwnerUsername = "User1",
                    Score = 50,
                    Personality = Personality.BRAVE,
                    PetType = PetType.DOG,
                    Description = "Description1",
                    IsFavourited = false
                },
                new PetCardDataDTO
                {
                    PetId = Guid.NewGuid(),
                    PetName = "TopPet2",
                    ImageUrl = "Test/image2.png",
                    OwnerUsername = "User2",
                    Score = 40,
                    Personality = Personality.CALM,
                    PetType = PetType.CAT,
                    Description = "Description2",
                    IsFavourited = false
                },
                 new PetCardDataDTO
                {
                    PetId = Guid.NewGuid(),
                    PetName = "TopPet3",
                    ImageUrl = "Test/image2.png",
                    OwnerUsername = "User2",
                    Score = 30,
                    Personality = Personality.CALM,
                    PetType = PetType.CAT,
                    Description = "Description3",
                    IsFavourited = false
                }

            };

            _petRepositoryMock.Setup(r => r.GetTop10Pets()).ReturnsAsync(listOfPets);

            // Act
            var result = await _petService.GetTop10Pets();

            // Assert:
            result.Should().BeEquivalentTo(listOfPets);
        }

        [Test]
        public async Task GetTop10Pets_ReturnsEmptyList_WhenNoPetsExist()
        {
            // Arrange
            _petRepositoryMock.Setup(r => r.GetTop10Pets()).ReturnsAsync(new List<PetCardDataDTO>());

            // Act
            var result = await _petService.GetTop10Pets();

            // Assert
            result.Should().BeEmpty();
        }
    }
}
