using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using P7CreateRestApi.Repositories.Interfaces;
using P7CreateRestApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P7Test
{
    public class UnitTestRatingEndPoint
    {
        [Fact]
        public async Task HomeTest()
        {
            var mockRepository = new Mock<IRatingRepository>();
            var mockLogger = new Mock<ILogger<RatingController>>();
            var service = new RatingService(mockRepository.Object);

            var newRating = new Rating
            {
                Id = 1,
                MoodysRating = "test",
                SandPRating = "test",
                FitchRating = "test"
            };

            var RatingEntity = new Rating
            {
                Id = 2,
                MoodysRating = "test",
                SandPRating = "test",
                FitchRating = "test"
            };

            IEnumerable<Rating> curvePoints = new List<Rating> { newRating, RatingEntity };
            mockRepository
                .Setup(repo => repo.GetAllAsync()) // Adjusted to match the repository method signature
                .ReturnsAsync(curvePoints); // Fixed the return type to match Task<IEnumerable<CurvePoint>>

            var controller = new RatingController(service, mockLogger.Object).WithAuthenticatedUser("1");
            var result = controller.Home();
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<List<Rating>>(okResult.Value); // Adjusted type to match IEnumerable
            Assert.NotEmpty(returned);
            Assert.Equal(returned[0], newRating);
            Assert.Equal(returned[1], RatingEntity);
        }
        [Fact]

        public async Task AddRatingTest()
        {
            var mockRepository = new Mock<IRatingRepository>();
            var mockLogger = new Mock<ILogger<RatingController>>();
            var service = new RatingService(mockRepository.Object);

            var newRating = new Rating
            {
                Id = 1,
                MoodysRating = "test",
                SandPRating = "test",
                FitchRating = "test"
            };

            mockRepository
               .Setup(repo => repo.CreateAsync(It.IsAny<Rating>()))
               .ReturnsAsync(newRating);

            var controller = new RatingController(service, mockLogger.Object).WithAuthenticatedUser("1");
            var result = controller.AddRatingForm(newRating);
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedRating = Assert.IsType<Rating>(okResult.Value);
            Assert.Equal(newRating, returnedRating);
        }
        [Fact]
        public async Task GetByIdTest()
        {
            var mockRepository = new Mock<IRatingRepository>();
            var mockLogger = new Mock<ILogger<RatingController>>();
            var service = new RatingService(mockRepository.Object);

            var rating = new Rating
            {
                Id = 1,
                MoodysRating = "test",
                SandPRating = "test",
                FitchRating = "test"
            };

            mockRepository
               .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(rating);
            mockRepository
             .Setup(repo => repo.ExistsAsync(It.IsAny<int>()))
             .ReturnsAsync(true);

            var controller = new RatingController(service, mockLogger.Object).WithAuthenticatedUser("1");
            var result = await controller.ShowUpdateForm(1);
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedRating = Assert.IsType<ServiceResult<Rating>>(okResult.Value);
            Assert.Equal(rating, returnedRating.Data);
        }
        [Fact]
        public async Task UpdateRatingTest()
        {
            var mockRepository = new Mock<IRatingRepository>();
            var mockLogger = new Mock<ILogger<RatingController>>();
            var service = new RatingService(mockRepository.Object);

            var updatedRating = new Rating
            {
                Id = 1,
                MoodysRating = "updated",
                SandPRating = "updated",
                FitchRating = "updated"
            };

            mockRepository
             .Setup(repo => repo.UpdateAsync(It.IsAny<Rating>()))
             .ReturnsAsync(updatedRating);
            mockRepository
             .Setup(repo => repo.ExistsAsync(It.IsAny<int>()))
             .ReturnsAsync(true);

            var controller = new RatingController(service, mockLogger.Object).WithAuthenticatedUser("1");
            var result = await controller.UpdateRating(1, updatedRating);
            Assert.NotNull(result);
            var okResult = Assert.IsType<RedirectToActionResult>(result);

        }
        [Fact]
        public async Task DeleteRatingTest()
        {
            var mockRepository = new Mock<IRatingRepository>();
            var mockLogger = new Mock<ILogger<RatingController>>();
            var service = new RatingService(mockRepository.Object);

            mockRepository
               .Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
               .ReturnsAsync(true);
            mockRepository
             .Setup(repo => repo.ExistsAsync(It.IsAny<int>()))
             .ReturnsAsync(true);

            var controller = new RatingController(service, mockLogger.Object).WithAuthenticatedUser("1");
            var result = await controller.DeleteRating(1);
            Assert.NotNull(result);
            var okResult = Assert.IsType<RedirectToActionResult>(result);

        }


    }
}
