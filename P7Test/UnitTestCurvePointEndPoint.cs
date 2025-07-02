using Dot.Net.WebApi.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using P7CreateRestApi.Repositories.Interfaces;
using P7CreateRestApi.Services;
using P7CreateRestApi.DTO.CurveDTO;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
namespace P7Test
{
    public class UnitTestCurvePointEndPoint
    {
        [Fact]
        public async Task HomeTest()
        {
            var mockRepository = new Mock<ICurvePointRepository>();
            var mockLogger = new Mock<ILogger<CurveController>>();
            var service = new CurvePointService(mockRepository.Object);

            var newCurvePoint = new CurvePoint
            {
                CurveId = 1,
                Term = 100.0,
                CurvePointValue = 10,
            };

            var curvePointEntity = new CurvePoint
            {
                CurveId = 2,
                Term = 110,
                CurvePointValue = 11
            };

            IEnumerable<CurvePoint> curvePoints = new List<CurvePoint> { newCurvePoint, curvePointEntity };
            mockRepository
                .Setup(repo => repo.GetAllAsync()) // Adjusted to match the repository method signature
                .ReturnsAsync(curvePoints); // Fixed the return type to match Task<IEnumerable<CurvePoint>>

            var controller = new CurveController(service, mockLogger.Object).WithAuthenticatedUser("1");
            var result = await controller.Home();
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCurvePoints = Assert.IsType<List<CurvePoint>>(okResult.Value); // Adjusted type to match IEnumerable
            Assert.NotEmpty(returnedCurvePoints);
            Assert.Equal(returnedCurvePoints[0], newCurvePoint);
            Assert.Equal(returnedCurvePoints[1], curvePointEntity);
        }
        [Fact]
        public async Task AddCurvePointTest()
        {
            var mockRepository = new Mock<ICurvePointRepository>();
            var mockLogger = new Mock<ILogger<CurveController>>();
            var service = new CurvePointService(mockRepository.Object);

            var newCurvePoint = new CurvePoint
            {
                CurveId = 1,
                Term = 100.0,
                CurvePointValue = 10,
            };

            var createCurvePointDTO = new CreateCurvePointDTO
            {
                CurveId = 1,
                Term = 100.0,
                CurvePointValue = 10,
            };

            mockRepository
               .Setup(repo => repo.CreateAsync(It.IsAny<CurvePoint>()))
               .ReturnsAsync(newCurvePoint);

            var controller = new CurveController(service, mockLogger.Object).WithAuthenticatedUser("1");
            var result = await controller.AddCurvePoint(createCurvePointDTO);
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCurvePoint = Assert.IsType<CurvePoint>(okResult.Value);
            Assert.Equal(newCurvePoint.CurveId, returnedCurvePoint.CurveId);
            Assert.Equal(newCurvePoint.Term, returnedCurvePoint.Term);
            Assert.Equal(newCurvePoint.CurvePointValue, returnedCurvePoint.CurvePointValue);

        }
        [Fact]
        public async Task GetCurvePointByIdTest()
        {
            var mockRepository = new Mock<ICurvePointRepository>();
            var mockLogger = new Mock<ILogger<CurveController>>();
            var service = new CurvePointService(mockRepository.Object);

            var curvePointId = 1;
            var curvePoint = new CurvePoint
            {
                CurveId = 1,
                Term = 100.0,
                CurvePointValue = 10,
            };

            mockRepository
               .Setup(repo => repo.GetByIdAsync(curvePointId))
               .ReturnsAsync(curvePoint);

            var controller = new CurveController(service, mockLogger.Object).WithAuthenticatedUser("1");
            var result = await controller.GetCurvePoint(curvePointId);
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCurvePoint = Assert.IsType<CurvePoint>(okResult.Value);
            Assert.Equal(curvePoint.CurveId, returnedCurvePoint.CurveId);
            Assert.Equal(curvePoint.Term, returnedCurvePoint.Term);
            Assert.Equal(curvePoint.CurvePointValue, returnedCurvePoint.CurvePointValue);
        }
        [Fact]
        public async Task GetCurvePointByIdNotFoundTest()
        {
            var mockRepository = new Mock<ICurvePointRepository>();
            var mockLogger = new Mock<ILogger<CurveController>>();
            var service = new CurvePointService(mockRepository.Object);

            var curvePointId = 1;

            mockRepository
             .Setup(repo => repo.GetByIdAsync(curvePointId))
             .ReturnsAsync((CurvePoint)null); // Simulate not found

            var controller = new CurveController(service, mockLogger.Object).WithAuthenticatedUser("1");
            var result = await controller.GetCurvePoint(curvePointId);
            Assert.NotNull(result);
            var notFoundResult = Assert.IsType<OkObjectResult>(result);
            Assert.Null(notFoundResult.Value);
        }
        [Fact]
        public async Task UpdateCurvePointTest()
        {
            var mockRepository = new Mock<ICurvePointRepository>();
            var mockLogger = new Mock<ILogger<CurveController>>();
            var service = new CurvePointService(mockRepository.Object);

            var curvePointId = 1;
            var updatedCurvePoint = new CurvePoint
            {
                CurveId = 1,
                Term = 120.0,
                CurvePointValue = 15,
            };

            mockRepository
             .Setup(repo => repo.GetByIdAsync(curvePointId))
             .ReturnsAsync(updatedCurvePoint);

            var controller = new CurveController(service, mockLogger.Object).WithAuthenticatedUser("1");
            var result = await controller.GetCurvePoint(curvePointId);
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCurvePoint = Assert.IsType<CurvePoint>(okResult.Value);
            Assert.Equal(updatedCurvePoint.CurveId, returnedCurvePoint.CurveId);
            Assert.Equal(updatedCurvePoint.Term, returnedCurvePoint.Term);
            Assert.Equal(updatedCurvePoint.CurvePointValue, returnedCurvePoint.CurvePointValue);
        }
        [Fact]
        public async Task UpdateCurvePointNotFoundTest()
        {
            var mockRepository = new Mock<ICurvePointRepository>();
            var mockLogger = new Mock<ILogger<CurveController>>();
            var service = new CurvePointService(mockRepository.Object);

            var curvePointId = 1;

            mockRepository
             .Setup(repo => repo.GetByIdAsync(curvePointId))
             .ReturnsAsync((CurvePoint)null); // Simulate not found

            var controller = new CurveController(service, mockLogger.Object).WithAuthenticatedUser("1");
            var result = await controller.GetCurvePoint(curvePointId);
            Assert.NotNull(result);
            var notFoundResult = Assert.IsType<OkObjectResult>(result);
            Assert.Null(notFoundResult.Value);
        }
        [Fact]
        public async Task DeleteCurvePointTest()
        {
            var mockRepository = new Mock<ICurvePointRepository>();
            var mockLogger = new Mock<ILogger<CurveController>>();
            var service = new CurvePointService(mockRepository.Object);

            var curvePointId = 1;

            mockRepository
             .Setup(repo => repo.DeleteAsync(curvePointId))
             .ReturnsAsync(true); // Simulate successful deletion
            mockRepository
             .Setup(repo => repo.ExistsAsync(curvePointId))
             .ReturnsAsync(true);

            var controller = new CurveController(service, mockLogger.Object).WithAuthenticatedUser("1");
            var result = await controller.DeleteCurve(curvePointId);
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("{ Message = CurvePoint deleted successfully. }", okResult.Value!.ToString());
        }
        [Fact]
        public async Task DeleteCurvePointNotFoundTest()
        {
            var mockRepository = new Mock<ICurvePointRepository>();
            var mockLogger = new Mock<ILogger<CurveController>>();
            var service = new CurvePointService(mockRepository.Object);

            var curvePointId = 1;

            mockRepository
             .Setup(repo => repo.DeleteAsync(curvePointId))
             .ReturnsAsync(false); // Simulate not found
            mockRepository
             .Setup(repo => repo.ExistsAsync(curvePointId))
             .ReturnsAsync(false);

            var controller = new CurveController(service, mockLogger.Object).WithAuthenticatedUser("1");
            var result = await controller.DeleteCurve(curvePointId);
            Assert.NotNull(result);
            var notFoundResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, notFoundResult.StatusCode);
        }


    }
}
