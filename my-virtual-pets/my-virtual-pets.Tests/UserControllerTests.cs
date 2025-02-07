using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using my_virtual_pets_api.Controllers;
using my_virtual_pets_api.Services.Interfaces;
using my_virtual_pets_class_library.DTO;

namespace my_virtual_pets.Tests
{
    public class UserControllerTests
    {
        private Mock<IUserService> _userServiceMock;
        private Mock<IConfiguration> _configurationMock;
        private UserController _controller;

        [SetUp]
        public void Setup()
        {
            _userServiceMock = new Mock<IUserService>();
            _configurationMock = new Mock<IConfiguration>();
            _controller = new UserController(_userServiceMock.Object, _configurationMock.Object);
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

        [Test]
        public void UpdateUser_ShouldReturnOk_WhenValidData()
        {
            // Arrange
            var updatedUser = new UpdateUserDTO { UserId = Guid.NewGuid(), NewUsername = "newUsername", NewPassword = "newPassword" };


            _userServiceMock.Setup(s => s.UpdateUser(updatedUser)).Returns(true);

            // Act
            var result = _controller.UpdateUser(updatedUser);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be("User updated successfully.");
        }

        [Test]
        public void UpdateUser_ShouldReturnBadRequest_WhenUsernameConflict()
        {
            // Arrange
            var updatedUser = new UpdateUserDTO { UserId = Guid.NewGuid(), NewUsername = "newUsername", NewPassword = "newPassword" };


            _userServiceMock.Setup(s => s.UpdateUser(updatedUser)).Throws(new InvalidOperationException("Username is already in use."));

            // Act
            var result = _controller.UpdateUser(updatedUser);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>() .Which.Value.Should().Be("Username is already in use."); 
        }
    }
}