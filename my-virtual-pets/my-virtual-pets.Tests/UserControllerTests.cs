﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using my_virtual_pets_api.Controllers;
using my_virtual_pets_api.Services.Interfaces;
using my_virtual_pets_class_library.DTO;

namespace my_virtual_pets.Tests
{
    public class UserControllerTests
    {
        private Mock<IUserService> _userServiceMock;
        private UserController _controller;

        [SetUp]
        public void Setup()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new UserController(_userServiceMock.Object);
        }

        [Test]
        public void GetUserDetailsByUserId_ReturnsOkResult_WithUserDisplayDTO()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDisplayDto = new UserDisplayDTO
            {
                Username = "TestUser",
                FirstName = "Testy",
                LastName = "aa",
                Email = "test@test.com",
                PetCount = 3
            };

            _userServiceMock.Setup(s => s.GetUserDetailsByUserId(userId)).Returns(userDisplayDto);

            // Act
            var result = _controller.GetUserDetailsByUserId(userId);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(userDisplayDto);
        }

        [Test]
        public void GetUserDetails_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userServiceMock.Setup(s => s.GetUserDetailsByUserId(userId)).Returns((UserDisplayDTO)null);

            // Act
            var result = _controller.GetUserDetailsByUserId(userId);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}