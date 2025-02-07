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

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
        }

        [Test]
        public void GetUserDetailsByUserId_ReturnsUserDisplayDTO_WhenUserExists()
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

                _userRepositoryMock.Setup(r => r.GetUserDetailsByUserId(userId)).Returns(userDisplayDto);

                // Act
                var result = _userService.GetUserDetailsByUserId(userId);

                // Assert
                result.Should().BeEquivalentTo(userDisplayDto);
            }

            [Test]
            public void UpdateUser_ShouldReturnTrue_WhenValidData()
            {
                // Arrange
                var updatedUser = new UpdateUserDTO { UserId = Guid.NewGuid(), NewUsername = "newUsername", NewPassword = "newPassword" };



                _userRepositoryMock.Setup(r => r.UpdateUser(updatedUser)).Returns(true);

                // Act
                var result = _userService.UpdateUser(updatedUser);

                // Assert
                result.Should().BeTrue();  
            }

            [Test]
            public void UpdateUser_ShouldThrowInvalidOperationException_WhenUsernameConflict()
            {
            // Arrange
            var updatedUser = new UpdateUserDTO { UserId = Guid.NewGuid(), NewUsername = "newUsername", NewPassword = "newPassword" };


            _userRepositoryMock.Setup(r => r.UpdateUser(updatedUser)).Throws(new InvalidOperationException("Username is already in use."));

                // Act and Assert
                Action act = () => _userService.UpdateUser(updatedUser);
                act.Should().Throw<InvalidOperationException>()
                    .WithMessage("Username is already in use.");
            }

        }
    }
