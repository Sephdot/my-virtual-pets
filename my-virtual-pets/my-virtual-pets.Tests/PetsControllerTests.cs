using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using my_virtual_pets_api.Controllers;
using my_virtual_pets_api.Data;
using my_virtual_pets_api.Services.Interfaces;
using my_virtual_pets_class_library.DTO;
using my_virtual_pets_class_library.Enums;

namespace my_virtual_pets.Tests
{
    public class PetsControllerTests
    {
        private Mock<IDbContext> _contextMock;
        private Mock<IPetService> _petServiceMock;
        private PetsController _controller;

        [SetUp]
        public void Setup()
        {

            _contextMock = new Mock<IDbContext>();
            _petServiceMock = new Mock<IPetService>();


            _controller = new PetsController(_petServiceMock.Object);
        }

        //GetAllPetsByUserId
        [Test]
        public async Task GetAllPetsByUserID_ReturnsOkResult_WithListOfPets()
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


            _petServiceMock.Setup(s => s.GetPetsByUser(userId))
                          .ReturnsAsync(petDtos);

            // Act
            var result = await _controller.GetAllPetsByUserID(userId);

            // Assert 
            result.Should().BeOfType<OkObjectResult>()
                  .Which.StatusCode.Should().Be(200);
            result.Should().BeOfType<OkObjectResult>()
                  .Which.Value.Should().BeEquivalentTo(petDtos);
        }

        [Test]
        public async Task GetAllPetsByUserID_ReturnsBadRequest_WhenInvaldUserId()
        {
            // Arrange
            var emptyUserId = Guid.Empty;

            // Act
            var result = await _controller.GetAllPetsByUserID(emptyUserId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                  .Which.StatusCode.Should().Be(400);
        }

        //GetPetById
        [Test]
        public async Task GetPetById_ReturnsOkResult_WithPet()
        {
            // Arrange
            var petId = Guid.NewGuid();
            var petDto = new PetCardDataDTO
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

            _petServiceMock.Setup(s => s.GetPetById(petId))
                          .ReturnsAsync(petDto);

            // Act
            var result = await _controller.GetPetById(petId);

            // Assert 
            result.Should().BeOfType<OkObjectResult>()
                  .Which.StatusCode.Should().Be(200);
            result.Should().BeOfType<OkObjectResult>()
                  .Which.Value.Should().BeEquivalentTo(petDto);
        }

        [Test]
        public async Task GetPetById_ReturnsNotFound_WhenPetDoesNotExist()
        {
            // Arrange
            var petId = Guid.NewGuid();
            _petServiceMock.Setup(s => s.GetPetById(petId)).ReturnsAsync((PetCardDataDTO?)null);

            // Act
            var result = await _controller.GetPetById(petId);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Test]
        public async Task GetPetById_ReturnsBadRequest_InvalidId()
        {
            // Arrange
            var invalidId = Guid.Empty;

            // Act
            var result = await _controller.GetPetById(invalidId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                  .Which.StatusCode.Should().Be(400);
        }

        //GetTop10Pets
        [Test]
        public async Task GetTop10Pets_ReturnsOkResult_WithListOfPets()
        {
            // Arrange
            var top10Pets = new List<PetCardDataDTO>
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
                }
            };

            _petServiceMock.Setup(s => s.GetTop10Pets()).ReturnsAsync(top10Pets);

            // Act
            var result = await _controller.GetTop10Pets();
            var okResult = result.Result as OkObjectResult;

            //Assert
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(top10Pets);
        }


    }

}
