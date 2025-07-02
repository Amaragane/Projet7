using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Controllers;
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
    
    public class UnitTestRulesNamesEndPoint

    {
        [Fact]
        public async Task HomeTest()
        {
            var mockRepository = new Mock<IRuleNameRepository>();
            var mockLogger = new Mock<ILogger<RuleNameController>>();
            var service = new RuleNameService(mockRepository.Object);

            var newRating = new RuleName
            {
                Id = 1,
                Name = "CheckAge",
                Description = "Vérifie si l'âge est supérieur à 18 ans",
                Json = "{\"minAge\":18}",
                Template = "SELECT * FROM Users WHERE Age > @minAge",
                SqlStr = "Age > 18",
                SqlPart = "AND Age > 18"
            };

            var RatingEntity = new RuleName
            {
                Id = 2,
                Name = "CheckStatus",
                Description = "Vérifie si le statut de l'utilisateur est actif",
                Json = "{\"status\":\"active\"}",
                Template = "SELECT * FROM Users WHERE Status = @status",
                SqlStr = "Status = 'active'",
                SqlPart = "AND Status = 'active'"
            };

            IEnumerable<RuleName> list = new List<RuleName> { newRating, RatingEntity };
            mockRepository
                .Setup(repo => repo.GetAllAsync()) // Adjusted to match the repository method signature
                .ReturnsAsync(list); // Fixed the return type to match Task<IEnumerable<CurvePoint>>

            var controller = new RuleNameController(service, mockLogger.Object).WithAuthenticatedUser("1");
            var result = await controller.Home();
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<List<RuleName>>(okResult.Value); // Adjusted type to match IEnumerable
            Assert.NotEmpty(returned);
            Assert.Equal(returned[0], newRating);
            Assert.Equal(returned[1], RatingEntity);
        }
        [Fact]
        public async Task AddRuleNameTest()
        {
            var mockRepository = new Mock<IRuleNameRepository>();
            var mockLogger = new Mock<ILogger<RuleNameController>>();
            var service = new RuleNameService(mockRepository.Object);

            var newRuleName = new RuleName
            {
                Id = 1,
                Name = "CheckAge",
                Description = "Vérifie si l'âge est supérieur à 18 ans",
                Json = "{\"minAge\":18}",
                Template = "SELECT * FROM Users WHERE Age > @minAge",
                SqlStr = "Age > 18",
                SqlPart = "AND Age > 18"
            };

            mockRepository
               .Setup(repo => repo.CreateAsync(It.IsAny<RuleName>()))
               .ReturnsAsync(newRuleName);

            var controller = new RuleNameController(service, mockLogger.Object).WithAuthenticatedUser("1");
            var result = await controller.AddRuleName(newRuleName);
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedRuleName = Assert.IsType<RuleName>(okResult.Value);
            Assert.Equal(newRuleName, returnedRuleName);
        }
        [Fact]
        public async Task ShowUpdateFormTest()
        {
            var mockRepository = new Mock<IRuleNameRepository>();
            var mockLogger = new Mock<ILogger<RuleNameController>>();
            var service = new RuleNameService(mockRepository.Object);

            var ruleName = new RuleName
            {
                Id = 1,
                Name = "CheckAge",
                Description = "Vérifie si l'âge est supérieur à 18 ans",
                Json = "{\"minAge\":18}",
                Template = "SELECT * FROM Users WHERE Age > @minAge",
                SqlStr = "Age > 18",
                SqlPart = "AND Age > 18"
            };

            mockRepository
             .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
             .ReturnsAsync(ruleName);

            var controller = new RuleNameController(service, mockLogger.Object).WithAuthenticatedUser("1");
            var result = await controller.ShowUpdateForm(1);
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedRuleName = Assert.IsType<RuleName>(okResult.Value);
            Assert.Equal(ruleName, returnedRuleName);
        }
        [Fact]
        public async Task UpdateRuleNameTest()
        {
            var mockRepository = new Mock<IRuleNameRepository>();
            var mockLogger = new Mock<ILogger<RuleNameController>>();
            var service = new RuleNameService(mockRepository.Object);

            var updatedRuleName = new RuleName
            {
                Id = 1,
                Name = "CheckAgeUpdated",
                Description = "Vérifie si l'âge est supérieur à 21 ans",
                Json = "{\"minAge\":21}",
                Template = "SELECT * FROM Users WHERE Age > @minAge",
                SqlStr = "Age > 21",
                SqlPart = "AND Age > 21"
            };

            mockRepository
             .Setup(repo => repo.UpdateAsync(It.IsAny<RuleName>()))
             .ReturnsAsync(updatedRuleName);
            mockRepository.Setup(mockRepository => mockRepository.ExistsAsync(It.IsAny<int>()))
             .ReturnsAsync(true);
            var controller = new RuleNameController(service, mockLogger.Object).WithAuthenticatedUser("1");
            var result = await controller.UpdateRuleName(1, updatedRuleName);
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedRuleName = Assert.IsType<RuleName>(okResult.Value);
            Assert.Equal(updatedRuleName, returnedRuleName);
        }
        [Fact]
        public async Task DeleteRuleNameTest()
        {
            var mockRepository = new Mock<IRuleNameRepository>();
            var mockLogger = new Mock<ILogger<RuleNameController>>();
            var service = new RuleNameService(mockRepository.Object);

            var ruleNameId = 1;

            mockRepository
             .Setup(repo => repo.DeleteAsync(ruleNameId))
             .ReturnsAsync(true);
            mockRepository.Setup(mockRepository => mockRepository.ExistsAsync(It.IsAny<int>()))
             .ReturnsAsync(true);

            var controller = new RuleNameController(service, mockLogger.Object).WithAuthenticatedUser("1");
            var result = await controller.DeleteRuleName(ruleNameId);
            Assert.NotNull(result);
            var okResult = Assert.IsType<RedirectToActionResult>(result);
        }
    }
}
