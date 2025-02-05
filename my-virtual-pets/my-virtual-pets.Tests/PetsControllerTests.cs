﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using my_virtual_pets_api.Controllers;
using my_virtual_pets_api.Services.Interfaces;
using my_virtual_pets_class_library.DTO;
using my_virtual_pets_class_library.Enums;
using FluentAssertions;
using Moq;
using my_virtual_pets_api.Data;

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

            _controller = new PetsController(_contextMock.Object, _petServiceMock.Object);
        }


        [Test]
        public void GetAllPetsByUserID_ReturnsOkResult_WithListOfPets()
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
                          .Returns(petDtos);

            // Act
            var result = _controller.GetAllPetsByUserID(userId);

            // Assert 
            result.Should().BeOfType<OkObjectResult>()
                  .Which.StatusCode.Should().Be(200); 
            result.Should().BeOfType<OkObjectResult>()
                  .Which.Value.Should().BeEquivalentTo(petDtos);
        }

        [Test]
        public void GetPetById_ReturnsOkResult_WithPetDto()
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
                          .Returns(petDto);

            // Act
            var result = _controller.GetPetById(petId);

            // Assert 
            result.Should().BeOfType<OkObjectResult>()
                  .Which.StatusCode.Should().Be(200);
            result.Should().BeOfType<OkObjectResult>()
                  .Which.Value.Should().BeEquivalentTo(petDto);
        }

    }
}
