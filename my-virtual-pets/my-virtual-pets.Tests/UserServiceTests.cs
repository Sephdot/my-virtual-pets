using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using my_virtual_pets_api.Repositories.Interfaces;
using my_virtual_pets_api.Services.Interfaces;
using my_virtual_pets_api.Services;
using my_virtual_pets_class_library.DTO;
using FluentAssertions;

namespace my_virtual_pets.Tests
{
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private IUserService _userService;
        private Mock<IPetService> _petServiceMock;

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _petServiceMock = new Mock<IPetService>();
            _userService = new UserService(_userRepositoryMock.Object, _petServiceMock.Object);
        }

        [Test]
        public async Task GetUserDetailsByUserId_ReturnsUserDisplayDTO_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDisplayDto = new UserDisplayDTO
            {
                Username = "TestUser",
                FirstName = "TestName",
                LastName = "LM",
                Email = "test@test.com",
                PetCount = 3
            };

            _userRepositoryMock.Setup(r => r.GetUserDetailsByUserId(userId)).ReturnsAsync(userDisplayDto);

            // Act
            var result = await _userService.GetUserDetailsByUserId(userId);

            // Assert
            result.Should().BeEquivalentTo(userDisplayDto);
        }

        [Test]
        public async Task UpdateUser_ShouldReturnTrue_WhenValidData()
        {
            // Arrange
            var updatedUser = new UpdateUserDTO { UserId = Guid.NewGuid(), NewUsername = "newUsername", NewPassword = "newPassword" };
            string currentPassword = "password";


            _userRepositoryMock.Setup(r => r.UpdateUser(updatedUser, currentPassword)).ReturnsAsync(true);

            // Act
            var result = await _userService.UpdateUser(updatedUser, currentPassword);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public async Task UpdateUser_ShouldThrowInvalidOperationException_WhenUsernameConflict()
        {
            // Arrange
            var updatedUser = new UpdateUserDTO { UserId = Guid.NewGuid(), NewUsername = "newUsername", NewPassword = "newPassword" };
            string currentPassword = "password";


            _userRepositoryMock.Setup(r => r.UpdateUser(updatedUser, currentPassword)).ThrowsAsync(new InvalidOperationException("Username is already in use."));

            //Act
            Func<Task> act = async () => await _userService.UpdateUser(updatedUser, currentPassword);

            //Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Username is already in use.");

        }

    }
}
