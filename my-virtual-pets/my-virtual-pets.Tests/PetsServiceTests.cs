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
        public void GetPetsByUser_ReturnsListOfPets_WhenUserExists()
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
                             .Returns(petDtos);

            // Act
            var result = _petService.GetPetsByUser(userId);

            // Assert 
            result.Should().BeEquivalentTo(petDtos);
        }

        [Test]
        public void GetAllPetsByUserID_ReturnsEmptyList_WhenUserHasNoPets()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _petRepositoryMock.Setup(r => r.GetAllPetsByUserID(userId)).Returns(new List<PetCardDataDTO>());

            // Act
            var result = _petService.GetPetsByUser(userId);

            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public void GetAllPetsByUserID_HandlesInvalidUserId()
        {
            // Arrange
            _petRepositoryMock.Setup(r => r.GetAllPetsByUserID(Guid.Empty)).Returns(new List<PetCardDataDTO>());

            // Act
            var result = _petService.GetPetsByUser(Guid.Empty);

            // Assert
            result.Should().BeEmpty();
        }


        // GetPetsById
        [Test]
        public void GetPetById_ReturnsPet_WhenPetExists()
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
                             .Returns(expectedPet);

            // Act
            var result = _petService.GetPetById(petId);

            // Assert 
            result.Should().BeEquivalentTo(expectedPet);
        }

        [Test]
        public void GetPetById_HandlesInvalidId()
        {
            // Arrange
            _petRepositoryMock.Setup(r => r.GetPetById(Guid.Empty)).Returns((PetCardDataDTO?)null);

            // Act
            var result = _petService.GetPetById(Guid.Empty);

            // Assert
            result.Should().BeNull();
        }
        [Test]
        public void GetPetById_ReturnsNull_WhenPetDoesNotExist()
        {
            // Arrange
            var petId = Guid.NewGuid(); // Random but valid GUID
            _petRepositoryMock.Setup(r => r.GetPetById(petId)).Returns((PetCardDataDTO?)null);

            // Act
            var result = _petService.GetPetById(petId);

            // Assert
            result.Should().BeNull();
        }



        //GetTop10Pets
        [Test]
        public void GetTop10Pets_ReturnsListOfPets()
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

            _petRepositoryMock.Setup(r => r.GetTop10Pets()).Returns(listOfPets);

            // Act
            var result = _petService.GetTop10Pets();

            // Assert:
            result.Should().BeEquivalentTo(listOfPets);
        }

        [Test]
        public void GetTop10Pets_ReturnsEmptyList_WhenNoPetsExist()
        {
            // Arrange
            _petRepositoryMock.Setup(r => r.GetTop10Pets()).Returns(new List<PetCardDataDTO>());

            // Act
            var result = _petService.GetTop10Pets();

            // Assert
            result.Should().BeEmpty();
        }
    }
}
