using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
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
    public class UnitTestTradeEndPoint
    {
        [Fact]
        public async Task HomeTest()
        {
            var mockRepository = new Mock<ITradeRepository>();
            var mockLogger = new Mock<ILogger<TradeController>>();
            var service = new TradeService(mockRepository.Object);

            var newTrade = new Trade
            {
                TradeId = 1,
                Account = "ACC12345",
                AccountType = "Client",
                BuyQuantity = 1000.00,
                SellQuantity = null,
                BuyPrice = 50.25,
                SellPrice = null,
                TradeDate = new DateTime(2025, 6, 30, 14, 30, 0),
                TradeSecurity = "AAPL",
                TradeStatus = "OPEN",
                Trader = "Jean Dupont",
                Benchmark = "S&P 500",
                Book = "Book1",
                CreationName = "System",
                CreationDate = new DateTime(2025, 6, 30, 14, 15, 0),
                RevisionName = "System",
                RevisionDate = new DateTime(2025, 6, 30, 14, 20, 0),
                DealName = "DealA",
                DealType = "Buy",
                SourceListId = "SRC001",
                Side = "Buy"
            };

            IEnumerable<Trade> list = new List<Trade> { newTrade };
            mockRepository
               .Setup(repo => repo.GetAllAsync())
               .ReturnsAsync(list);

            var controller = new TradeController(service, mockLogger.Object).WithAuthenticatedUser("1");
            var result = await controller.Home();
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<List<Trade>>(okResult.Value);
            Assert.NotEmpty(returned);
            Assert.Equal(returned[0], newTrade);
        }
        [Fact]
        public void AddTradeTest()
        {
            var mockRepository = new Mock<ITradeRepository>();
            var mockLogger = new Mock<ILogger<TradeController>>();
            var service = new TradeService(mockRepository.Object);

            var newTrade = new Trade
            {
                TradeId = 1,
                Account = "ACC12345",
                AccountType = "Client",
                BuyQuantity = 1000.00,
                SellQuantity = null,
                BuyPrice = 50.25,
                SellPrice = null,
                TradeDate = new DateTime(2025, 6, 30, 14, 30, 0),
                TradeSecurity = "AAPL",
                TradeStatus = "OPEN",
                Trader = "Jean Dupont",
                Benchmark = "S&P 500",
                Book = "Book1",
                CreationName = "System",
                CreationDate = new DateTime(2025, 6, 30, 14, 15, 0),
                RevisionName = "System",
                RevisionDate = new DateTime(2025, 6, 30, 14, 20, 0),
                DealName = "DealA",
                DealType = "Buy",
                SourceListId = "SRC001",
                Side = "Buy"
            };

            mockRepository
             .Setup(repo => repo.CreateAsync(It.IsAny<Trade>()))
             .ReturnsAsync(newTrade);

            var controller = new TradeController(service, mockLogger.Object).WithAuthenticatedUser("1");
            var result = controller.AddTrade(newTrade);
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTrade = Assert.IsType<Trade>(okResult.Value);
            Assert.Equal(returnedTrade.TradeId, newTrade.TradeId);
        }
        [Fact]
        public void ShowUpdateFormTest()
        {
            var mockRepository = new Mock<ITradeRepository>();
            var mockLogger = new Mock<ILogger<TradeController>>();
            var service = new TradeService(mockRepository.Object);

            var existingTrade = new Trade
            {
                TradeId = 1,
                Account = "ACC12345",
                AccountType = "Client",
                BuyQuantity = 1000.00,
                SellQuantity = null,
                BuyPrice = 50.25,
                SellPrice = null,
                TradeDate = new DateTime(2025, 6, 30, 14, 30, 0),
                TradeSecurity = "AAPL",
                TradeStatus = "OPEN",
                Trader = "Jean Dupont",
                Benchmark = "S&P 500",
                Book = "Book1",
                CreationName = "System",
                CreationDate = new DateTime(2025, 6, 30, 14, 15, 0),
                RevisionName = "System",
                RevisionDate = new DateTime(2025, 6, 30, 14, 20, 0),
                DealName = "DealA",
                DealType = "Buy",
                SourceListId = "SRC001",
                Side = "Buy"
            };

            mockRepository
             .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
             .ReturnsAsync(existingTrade);

            var controller = new TradeController(service, mockLogger.Object).WithAuthenticatedUser("1");
            var result = controller.ShowUpdateForm(1);
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTrade = Assert.IsType<Trade>(okResult.Value);
            Assert.Equal(returnedTrade.TradeId, existingTrade.TradeId);
        }
        [Fact]
        public void UpdateTradeTest()
        {
            var mockRepository = new Mock<ITradeRepository>();
            var mockLogger = new Mock<ILogger<TradeController>>();
            var service = new TradeService(mockRepository.Object);

            var updatedTrade = new Trade
            {
                TradeId = 1,
                Account = "ACC12345",
                AccountType = "Client",
                BuyQuantity = 1000.00,
                SellQuantity = null,
                BuyPrice = 50.25,
                SellPrice = null,
                TradeDate = new DateTime(2025, 6, 30, 14, 30, 0),
                TradeSecurity = "AAPL",
                TradeStatus = "OPEN",
                Trader = "Jean Dupont",
                Benchmark = "S&P 500",
                Book = "Book1",
                CreationName = "System",
                CreationDate = new DateTime(2025, 6, 30, 14, 15, 0),
                RevisionName = "System",
                RevisionDate = new DateTime(2025, 6, 30, 14, 20, 0),
                DealName = "DealA",
                DealType = "Buy",
                SourceListId = "SRC001",
                Side = "Buy"
            };

            mockRepository
             .Setup(repo => repo.UpdateAsync(It.IsAny<Trade>()))
             .ReturnsAsync(updatedTrade);
            mockRepository.Setup(repo => repo.ExistsAsync(It.IsAny<int>()))
             .ReturnsAsync(true);
            var controller = new TradeController(service, mockLogger.Object).WithAuthenticatedUser("1");
            var result = controller.UpdateTrade(1, updatedTrade);
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTrade = Assert.IsType<Trade>(okResult.Value);
            Assert.Equal(returnedTrade.TradeId, updatedTrade.TradeId);
        }
        [Fact]
        public void DeleteTradeTest()
        {
            var mockRepository = new Mock<ITradeRepository>();
            var mockLogger = new Mock<ILogger<TradeController>>();
            var service = new TradeService(mockRepository.Object);

            mockRepository
             .Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
             .ReturnsAsync(true);
            mockRepository.Setup(repo => repo.ExistsAsync(It.IsAny<int>()))
             .ReturnsAsync(true);

            var controller = new TradeController(service, mockLogger.Object).WithAuthenticatedUser("1");
            var result = controller.DeleteTrade(1);
            Assert.NotNull(result);
            var okResult = Assert.IsType<RedirectToActionResult>(result);
        }
    }
}
