using Castle.Core.Configuration;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using P7CreateRestApi.Controllers;
using P7CreateRestApi.DTO.AuthDTO;
using P7CreateRestApi.DTO.UsersDTO;
using P7CreateRestApi.Services;
using P7CreateRestApi.Services.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P7Test
{
    public class UnitTestAuthEndPoint
    {
        public UnitTestAuthEndPoint() { }
        [Fact]
        public async Task Login_ReturnsOk_WhenCredentialsAreValid()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthService>();
            var mockJwtService = new Mock<IJwtService>();
            var mockLogger = new Mock<ILogger<AuthController>>();

            var loginRequest = new LoginRequestDTO
            {
                Email = "alice.dupont@email.com",
                Password = "password123",
                RememberMe = false
            };

            var authResponse = new AuthResponseDTO
            {
                Token = "fake-jwt-token",
                TokenType = "Bearer",
                ExpiresAt = DateTime.UtcNow.AddMinutes(60),
                User = new UserDTO
                {
                    Id = "U001",
                    Email = "alice.dupont@email.com",
                    Username = "alice.dupont",
                    Fullname = "Alice Dupont",
                    PhoneNumber = "+33612345678",
                    Roles = new List<string> { "Admin", "User" },
                    EmailConfirmed = true
                }
            };

            mockAuthService.Setup(s => s.LoginAsync(It.IsAny<LoginRequestDTO>()))
                .ReturnsAsync(ServiceResult<AuthResponseDTO>.Success(authResponse));

            var controller = new AuthController(
                mockAuthService.Object,
                mockJwtService.Object,
                mockLogger.Object
            );

            // Act
            var result = await controller.Login(loginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var json = JsonConvert.SerializeObject(okResult.Value);

            var response = JsonConvert.DeserializeObject<LoginResponseTestDto>(json);
            Assert.True(response!.success);
            Assert.Equal("Connexion réussie", response.message);
            Assert.Equal("fake-jwt-token", response.data!.Token);

        }
        [Fact]
        public async Task Register_ReturnsOk_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthService>();
            var mockJwtService = new Mock<IJwtService>();
            var mockLogger = new Mock<ILogger<AuthController>>();

            var registerRequest = new RegisterRequestDTO
            {
                Email = "alice.dupont@email.com",
                UserName = "alice.dupont",
                Fullname = "Alice Dupont",
                Password = "SuperPassword2025!"
            };

            var userDto = new UserDTO
            {
                Id = "U001",
                Email = registerRequest.Email,
                Username = registerRequest.UserName,
                Fullname = registerRequest.Fullname,
                PhoneNumber = "+33612345678",
                Roles = new List<string> { "User" },
                CreatedAt = DateTime.UtcNow,
                LastLoginAt = null,
                IsActive = true,
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                LockoutEnabled = false,
                LockoutEnd = null,
                AccessFailedCount = 0
            };

            var authResponse = new AuthResponseDTO
            {
                Token = "jwt-token",
                RefreshToken = "refresh-token",
                TokenType = "Bearer",
                ExpiresAt = DateTime.UtcNow.AddMinutes(60),
                User = userDto,
                Permissions = new List<string> { "read", "write" }
            };

            mockAuthService.Setup(s => s.RegisterAsync(It.IsAny<RegisterRequestDTO>()))
                .ReturnsAsync(ServiceResult<AuthResponseDTO>.Success(authResponse));

            var controller = new AuthController(
                mockAuthService.Object,
                mockJwtService.Object,
                mockLogger.Object
            );

            // Act
            var result = await controller.Register(registerRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Sérialisation/désérialisation pour accéder à l'objet anonyme retourné
            var json = System.Text.Json.JsonSerializer.Serialize(okResult.Value);
            var response = System.Text.Json.JsonSerializer.Deserialize<RegisterApiResponseTestDto>(json);

            Assert.True(response.success);
            Assert.NotNull(response.message);

            // On peut désérialiser le message en AuthResponseDTO si besoin
            var authResponseDtoJson = System.Text.Json.JsonSerializer.Serialize(response.message);
            var authResponseDto = System.Text.Json.JsonSerializer.Deserialize<AuthResponseDTO>(authResponseDtoJson);

            Assert.Equal("jwt-token", authResponseDto.Token);
            Assert.Equal("refresh-token", authResponseDto.RefreshToken);
            Assert.Equal("Bearer", authResponseDto.TokenType);
            Assert.Equal("alice.dupont@email.com", authResponseDto.User.Email);
            Assert.Contains("read", authResponseDto.Permissions);
        }
    }

        public class LoginResponseTestDto
        {
            public bool success { get; set; }
            public string? message { get; set; }
            public AuthResponseDTO? data { get; set; }
        }
        public class RegisterApiResponseTestDto
    {
            public bool success { get; set; }
            public AuthResponseDTO? message { get; set; }
        }
    }




