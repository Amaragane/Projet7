using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using P7CreateRestApi.Controllers;
using P7CreateRestApi.DTO.BidDTO;
using P7CreateRestApi.Services.Interfaces;
using P7CreateRestApi.Services;
using Dot.Net.WebApi.Services.Models;
using P7CreateRestApi.Repositories.Interfaces;
namespace P7Test
{
    public class UnitTestBidListEndPoint
    {
        [Fact]
        public async Task ADDTest()
        {
            // Arrange
            var mockBidListRepository = new Mock<IBidListRepository>();
            var mockLogger = new Mock<ILogger<BidListController>>();
            var bidListService = new BidListService(mockBidListRepository.Object);
            var newBidList = new BidList
            {
                BidListId = 1,
                Account = "TestAccount",
                BidType = "TestType",
                BidQuantity = 100.0,
                AskQuantity = 100.0,
                Bid = 50.0,
                Ask = 55.0,
                Benchmark = "Default Benchmark",
                BidListDate = DateTime.UtcNow,
                Commentary = "Default commentary text",
                BidSecurity = "Default Security",
                BidStatus = "Active",
                Trader = "John Doe",
                Book = "Default Book",
                CreationName = "System",
                CreationDate = DateTime.UtcNow,
                RevisionName = "System",
                RevisionDate = DateTime.UtcNow,
                DealName = "Default Deal",
                DealType = "Type A",
                SourceListId = "SRC-001",
                Side = "Buy"

            };
            var createBidList = new CreateBidDTO
            {
                Account = "TestAccount",
                BidType = "TestType",
                BidQuantity = 100.0,

            };

            mockBidListRepository
                .Setup( s => s.CreateAsync(It.IsAny<BidList>()))
                .Returns(Task.FromResult(newBidList));

            var controller = new BidListController(bidListService,mockLogger.Object).WithAuthenticatedUser("1");

            // Act
            var result = await controller.AddBid(createBidList);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBidList = Assert.IsType<BidList>(okResult.Value);
            Assert.Equal(newBidList.BidListId, returnedBidList.BidListId);
        }

        [Fact]
        public async Task UpdateBidTest()
        {
            var mockBidListRepository = new Mock<IBidListRepository>();
            var mockLogger = new Mock<ILogger<BidListController>>();
            var bidListService = new BidListService(mockBidListRepository.Object);
            var newBidList = new BidList
            {
                BidListId = 1,
                Account = "TestAccount",
                BidType = "TestType",
                BidQuantity = 111.0,
                AskQuantity = 100.0,
                Bid = 50.0,
                Ask = 55.0,
                Benchmark = "Default Benchmark",
                BidListDate = DateTime.UtcNow,
                Commentary = "Default commentary text",
                BidSecurity = "Default Security",
                BidStatus = "Active",
                Trader = "John Doe",
                Book = "Default Book",
                CreationName = "System",
                CreationDate = DateTime.UtcNow,
                RevisionName = "System",
                RevisionDate = DateTime.UtcNow,
                DealName = "Default Deal",
                DealType = "Type A",
                SourceListId = "SRC-001",
                Side = "Buy"

            };
            var updateBidList = new GetUpdateBidDTO
            {
                BidListId = 1,
                Account = "TestAccount",
                BidType = "TestType",
                BidQuantity = 111.0,
            };
            mockBidListRepository
                .Setup(s => s.UpdateAsync(It.IsAny<BidList>()))
                .Returns(Task.FromResult(newBidList));
            mockBidListRepository
                .Setup(s => s.ExistsAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(true));
            var controller = new BidListController(bidListService, mockLogger.Object).WithAuthenticatedUser("1");

            // Act
            var result = await controller.UpdateBid(updateBidList.BidListId,updateBidList);
            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBidList = Assert.IsType<BidList>(okResult.Value);
            Assert.Equal(newBidList, returnedBidList);
        }
        [Fact]
        public async Task GetBidTest()
        {
            var mockBidListRepository = new Mock<IBidListRepository>();
            var mockLogger = new Mock<ILogger<BidListController>>();
            var bidListService = new BidListService(mockBidListRepository.Object);
            var newBidList = new BidList
            {
                BidListId = 1,
                Account = "TestAccount",
                BidType = "TestType",
                BidQuantity = 111.0,
                AskQuantity = 100.0,
                Bid = 50.0,
                Ask = 55.0,
                Benchmark = "Default Benchmark",
                BidListDate = DateTime.UtcNow,
                Commentary = "Default commentary text",
                BidSecurity = "Default Security",
                BidStatus = "Active",
                Trader = "John Doe",
                Book = "Default Book",
                CreationName = "System",
                CreationDate = DateTime.UtcNow,
                RevisionName = "System",
                RevisionDate = DateTime.UtcNow,
                DealName = "Default Deal",
                DealType = "Type A",
                SourceListId = "SRC-001",
                Side = "Buy"

            };

            mockBidListRepository
                .Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(newBidList));
            mockBidListRepository
                .Setup(s => s.ExistsAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(true));
            var controller = new BidListController(bidListService, mockLogger.Object).WithAuthenticatedUser("1");

            // Act
            var result = await controller.GetBid(newBidList.BidListId);
            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBidList = Assert.IsType<BidList>(okResult.Value);
            Assert.Equal(newBidList, returnedBidList);
        }
        [Fact]
        public async Task DeleteBidTest()
        {
            var mockBidListRepository = new Mock<IBidListRepository>();
            var mockLogger = new Mock<ILogger<BidListController>>();
            var bidListService = new BidListService(mockBidListRepository.Object);
            var newBidList = new BidList
            {
                BidListId = 1,
                Account = "TestAccount",
                BidType = "TestType",
                BidQuantity = 111.0,
                AskQuantity = 100.0,
                Bid = 50.0,
                Ask = 55.0,
                Benchmark = "Default Benchmark",
                BidListDate = DateTime.UtcNow,
                Commentary = "Default commentary text",
                BidSecurity = "Default Security",
                BidStatus = "Active",
                Trader = "John Doe",
                Book = "Default Book",
                CreationName = "System",
                CreationDate = DateTime.UtcNow,
                RevisionName = "System",
                RevisionDate = DateTime.UtcNow,
                DealName = "Default Deal",
                DealType = "Type A",
                SourceListId = "SRC-001",
                Side = "Buy"

            };

            mockBidListRepository
                .Setup(s => s.DeleteAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(true));
            mockBidListRepository
                .Setup(s => s.ExistsAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(true));
            var controller = new BidListController(bidListService, mockLogger.Object).WithAuthenticatedUser("1");

            // Act
            var result = await controller.DeleteBid(newBidList.BidListId);
            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBidList = Assert.IsType<string>(okResult.Value!.ToString());
            Assert.Equal("{ Message = Bid deleted successfully. }", returnedBidList);
        }
        [Fact]
        public async Task DeleteBidNotFoundTest()
        {
            var mockBidListRepository = new Mock<IBidListRepository>();
            var mockLogger = new Mock<ILogger<BidListController>>();
            var bidListService = new BidListService(mockBidListRepository.Object);
            var newBidList = new BidList
            {
                BidListId = 1,
                Account = "TestAccount",
                BidType = "TestType",
                BidQuantity = 111.0,
                AskQuantity = 100.0,
                Bid = 50.0,
                Ask = 55.0,
                Benchmark = "Default Benchmark",
                BidListDate = DateTime.UtcNow,
                Commentary = "Default commentary text",
                BidSecurity = "Default Security",
                BidStatus = "Active",
                Trader = "John Doe",
                Book = "Default Book",
                CreationName = "System",
                CreationDate = DateTime.UtcNow,
                RevisionName = "System",
                RevisionDate = DateTime.UtcNow,
                DealName = "Default Deal",
                DealType = "Type A",
                SourceListId = "SRC-001",
                Side = "Buy"

            };

            mockBidListRepository
               .Setup(s => s.DeleteAsync(It.IsAny<int>()))
               .Returns(Task.FromResult(false));
            mockBidListRepository
               .Setup(s => s.ExistsAsync(It.IsAny<int>()))
               .Returns(Task.FromResult(false));
            var controller = new BidListController(bidListService, mockLogger.Object).WithAuthenticatedUser("1");

            // Act
            var result = await controller.DeleteBid(newBidList.BidListId);
            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, okResult.StatusCode);
        }
        [Fact]
        public async Task GetBidNotFoundTest()
        {
            var mockBidListRepository = new Mock<IBidListRepository>();
            var mockLogger = new Mock<ILogger<BidListController>>();
            var bidListService = new BidListService(mockBidListRepository.Object);
            var newBidList = new BidList
            {
                BidListId = 1,
                Account = "TestAccount",
                BidType = "TestType",
                BidQuantity = 111.0,
                AskQuantity = 100.0,
                Bid = 50.0,
                Ask = 55.0,
                Benchmark = "Default Benchmark",
                BidListDate = DateTime.UtcNow,
                Commentary = "Default commentary text",
                BidSecurity = "Default Security",
                BidStatus = "Active",
                Trader = "John Doe",
                Book = "Default Book",
                CreationName = "System",
                CreationDate = DateTime.UtcNow,
                RevisionName = "System",
                RevisionDate = DateTime.UtcNow,
                DealName = "Default Deal",
                DealType = "Type A",
                SourceListId = "SRC-001",
                Side = "Buy"

            };

            mockBidListRepository
             .Setup(s => s.GetByIdAsync(It.IsAny<int>()))
             .Returns(Task.FromResult((BidList)null));
            mockBidListRepository
             .Setup(s => s.ExistsAsync(It.IsAny<int>()))
             .Returns(Task.FromResult(false));
            var controller = new BidListController(bidListService, mockLogger.Object).WithAuthenticatedUser("1");

            // Act
            var result = await controller.GetBid(newBidList.BidListId);
            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404,okResult.StatusCode);
        }

        [Fact]
        public async Task UpdateBidNotFoundTest()
        {
            var mockBidListRepository = new Mock<IBidListRepository>();
            var mockLogger = new Mock<ILogger<BidListController>>();
            var bidListService = new BidListService(mockBidListRepository.Object);
            var updateBidList = new GetUpdateBidDTO
            {
                BidListId = 1,
                Account = "TestAccount",
                BidType = "TestType",
                BidQuantity = 111.0,
            };

            mockBidListRepository
               .Setup(s => s.UpdateAsync(It.IsAny<BidList>()))
               .Returns(Task.FromResult((BidList)null));
            mockBidListRepository
               .Setup(s => s.ExistsAsync(It.IsAny<int>()))
               .Returns(Task.FromResult(false));
            var controller = new BidListController(bidListService, mockLogger.Object).WithAuthenticatedUser("1");

            // Act
            var result = await controller.UpdateBid(updateBidList.BidListId, updateBidList);
            // Assert
            Assert.NotNull(result);
            var notFoundResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, notFoundResult.StatusCode);
        }


    }
}
