using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using P7CreateRestApi.DTO.UsersDTO;
using P7CreateRestApi.Repositories.Interfaces;
using P7CreateRestApi.Services;
using P7CreateRestApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P7Test
{
    public class UnitTestUserEndPoint
    {
        [Fact]
        public void HomeTest()
        {
            // Arrange
            var store = new Mock<IUserStore<User>>();
            var options = new Mock<IOptions<IdentityOptions>>();
            var passwordHasher = new Mock<IPasswordHasher<User>>();
            var userValidators = new List<IUserValidator<User>>();
            var passwordValidators = new List<IPasswordValidator<User>>();
            var lookupNormalizer = new Mock<ILookupNormalizer>();
            var identityErrorDescriber = new Mock<IdentityErrorDescriber>();
            var serviceProvider = new Mock<IServiceProvider>();
            var logger = new Mock<ILogger<UserManager<User>>>();

            var mockManager = new Mock<UserManager<User>>(
                store.Object,
                options.Object,
                passwordHasher.Object,
                userValidators,
                passwordValidators,
                lookupNormalizer.Object,
                identityErrorDescriber.Object,
                serviceProvider.Object,
                logger.Object
            );
            var mockLogger = new Mock<ILogger<UserService>>();
            var service = new UserService(mockManager.Object, mockLogger.Object);
            var expectedUsers = new List<User>
            {
                new User {     Id = "U001",
    Email = "alice.dupont@email.com",
    UserName = "alice.dupont",
    Fullname = "Alice Dupont",
    PhoneNumber = "+33612345678",
    Roles = new List<string> { "Admin", "User" },
},
                new User {     Id = "U002",
    Email = "benjamin.leclerc@email.com",
    UserName = "benjamin.leclerc",
    Fullname = "Benjamin Leclerc",
    Roles = new List<string> { "User" },

            },
            };
            // Act
            var usersQueryable = expectedUsers.AsQueryable();

            mockManager.Setup(s => s.Users).Returns(usersQueryable);
            var result = service.GetAllUsersAsync();
            // Assert
            Assert.NotNull(result);
            Assert.IsType<ServiceResult<IEnumerable<UserDTO>>>(result);
            var serviceResult = result as ServiceResult<IEnumerable<UserDTO>>;
            Assert.NotNull(serviceResult);
            Assert.True(serviceResult.IsSuccess);



        }
        [Fact]
        public async Task GetUserTest()
        {
            // Arrange
            var store = new Mock<IUserStore<User>>();
            var options = new Mock<IOptions<IdentityOptions>>();
            var passwordHasher = new Mock<IPasswordHasher<User>>();
            var userValidators = new List<IUserValidator<User>>();
            var passwordValidators = new List<IPasswordValidator<User>>();
            var lookupNormalizer = new Mock<ILookupNormalizer>();
            var identityErrorDescriber = new Mock<IdentityErrorDescriber>();
            var serviceProvider = new Mock<IServiceProvider>();
            var logger = new Mock<ILogger<UserManager<User>>>();

            var mockManager = new Mock<UserManager<User>>(
                store.Object,
                options.Object,
                passwordHasher.Object,
                userValidators,
                passwordValidators,
                lookupNormalizer.Object,
                identityErrorDescriber.Object,
                serviceProvider.Object,
                logger.Object
            );
            var mockLogger = new Mock<ILogger<UserService>>();
            var service = new UserService(mockManager.Object, mockLogger.Object);
            var expectedUsers = new User
            {
                Id = "U001",
                Email = "alice.dupont@email.com",
                UserName = "alice.dupont",
                Fullname = "Alice Dupont",
                PhoneNumber = "+33612345678",
                Roles = new List<string> { "Admin", "User" },
            };
            // Act

            mockManager.Setup(s => s.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(expectedUsers));
            var result = await service.GetUserByIdAsync(1);
            // Assert
            Assert.NotNull(result);
            var serviceResult = Assert.IsType<ServiceResult<UserDTO>>(result);

            Assert.NotNull(serviceResult);
            Assert.True(serviceResult.IsSuccess);
        }
        [Fact]
        public async Task UpdateUserTest()
        {
            // Arrange
            var store = new Mock<IUserStore<User>>();
            var options = new Mock<IOptions<IdentityOptions>>();
            var passwordHasher = new Mock<IPasswordHasher<User>>();
            var userValidators = new List<IUserValidator<User>>();
            var passwordValidators = new List<IPasswordValidator<User>>();
            var lookupNormalizer = new Mock<ILookupNormalizer>();
            var identityErrorDescriber = new Mock<IdentityErrorDescriber>();
            var serviceProvider = new Mock<IServiceProvider>();
            var logger = new Mock<ILogger<UserManager<User>>>();

            var mockManager = new Mock<UserManager<User>>(
                store.Object,
                options.Object,
                passwordHasher.Object,
                userValidators,
                passwordValidators,
                lookupNormalizer.Object,
                identityErrorDescriber.Object,
                serviceProvider.Object,
                logger.Object
            );
            var mockLogger = new Mock<ILogger<UserService>>();
            var service = new UserService(mockManager.Object, mockLogger.Object);
            var expectedUsers = new UpdateUserDTO
            {
                Email = "benjamin.leclerc@email.com",
                Username = "benjamin.leclerc",
                Fullname = "Benjamin Leclerc",
                PhoneNumber = "+33698765432",
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                IsActive = true
            };
            var oldUsers = new User
            {
                Id = "U001",
                Email = "alice.dupont@email.com",
                UserName = "alice.dupont",
                Fullname = "Alice Dupont",
                PhoneNumber = "+33612345678",
                Roles = new List<string> { "Admin", "User" },
            };
            // Act

            mockManager.Setup(s => s.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(oldUsers));
            mockManager.Setup(s => s.UpdateAsync(It.IsAny<User>())).Returns(Task.FromResult(IdentityResult.Success));
            mockManager.Setup(s => s.FindByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(null as User));
            mockManager.Setup(s => s.FindByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(null as User));
            var result = await service.UpdateUserAsync(1, expectedUsers);
            // Assert
            Assert.NotNull(result);
            var serviceResult = Assert.IsType<ServiceResult<UserDTO>>(result);

            Assert.NotNull(serviceResult);
            Assert.True(serviceResult.IsSuccess);
        }
        [Fact]
        public async Task DeleteUserTest()
        {
            // Arrange
            var store = new Mock<IUserStore<User>>();
            var options = new Mock<IOptions<IdentityOptions>>();
            var passwordHasher = new Mock<IPasswordHasher<User>>();
            var userValidators = new List<IUserValidator<User>>();
            var passwordValidators = new List<IPasswordValidator<User>>();
            var lookupNormalizer = new Mock<ILookupNormalizer>();
            var identityErrorDescriber = new Mock<IdentityErrorDescriber>();
            var serviceProvider = new Mock<IServiceProvider>();
            var logger = new Mock<ILogger<UserManager<User>>>();

            var mockManager = new Mock<UserManager<User>>(
               store.Object,
               options.Object,
               passwordHasher.Object,
               userValidators,
               passwordValidators,
               lookupNormalizer.Object,
               identityErrorDescriber.Object,
               serviceProvider.Object,
               logger.Object
            );
            var mockLogger = new Mock<ILogger<UserService>>();
            var service = new UserService(mockManager.Object, mockLogger.Object);
            var expectedUsers = new User
            {
                Id = "U001",
                Email = "alice.dupont@email.com",
                UserName = "alice.dupont",
                Fullname = "Alice Dupont",
                PhoneNumber = "+33612345678",
                Roles = new List<string> { "Admin", "User" },

            };
            // Act
            mockManager.Setup(s => s.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(expectedUsers));
            mockManager.Setup(s => s.DeleteAsync(It.IsAny<User>())).Returns(Task.FromResult(IdentityResult.Success));
            var result = await service.DeleteUserAsync(1);
            // Assert
            Assert.NotNull(result);
            var serviceResult = Assert.IsType<ServiceResult<bool>>(result);
            Assert.NotNull(serviceResult);
            Assert.True(serviceResult.IsSuccess);
        }

    }
}
