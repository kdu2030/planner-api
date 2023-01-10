using PlannerAPI.Models;
using PlannerAPI.Services.Authentication;
using PlannerAPI.Controllers;
using Moq;
using Microsoft.AspNetCore.Mvc;
using PlannerAPI.Types;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace PlannerAPITests {
    [TestClass]
    public class YearTermControllerTests {
        private IUserStore<PlannerUser> _userStore;
        private TokenGenerator _tokenGenerator;

        [TestInitialize]
        public void Setup() {
            _userStore = Mock.Of<IUserStore<PlannerUser>>();
            Dictionary<string, string> configDict = new Dictionary<string, string>{
                {"Jwt", ""},
                {"Jwt:key", "yROdirqY0vthrTth"},
                {"Jwt:issuer", "https://localhost:7074" },
                {"Jwt:audience", "https://localhost:7074"}
            };
            IConfiguration testConfig = new ConfigurationBuilder().AddInMemoryCollection(configDict).Build();
            _tokenGenerator = new TokenGenerator(testConfig);

        }

        [TestMethod]
        public void TestGetYears_InvalidToken() {
            YearTermController controller = new YearTermController(_tokenGenerator, _appDbContextMock.Object);
            BasicResult expectedResult = new BasicResult("User Not Found");
            BadRequestObjectResult? result = controller.GetYears("mockToken") as BadRequestObjectResult;
            Assert.AreEqual(400, result?.StatusCode);
            Assert.AreEqual(expectedResult, result?.Value);
        }   


    }
}
