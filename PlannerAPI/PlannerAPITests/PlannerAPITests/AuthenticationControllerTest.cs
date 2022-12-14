using PlannerAPI.Controllers;
using Microsoft.AspNetCore.Identity;
using Moq;
using Microsoft.Extensions.Configuration;
using PlannerAPI.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace PlannerAPITests {
    [TestClass]
    public class AuthenticationControllerTest {
        private IUserStore<IdentityUser> _userStore;
        private Mock<UserManager<IdentityUser>> _userManagerMock;
        private TokenGenerator _tokenGenerator;

        [TestInitialize]
        public void Setup() {
            _userStore = Mock.Of<IUserStore<IdentityUser>>();
            
            _userManagerMock = new Mock<UserManager<IdentityUser>>(_userStore, null, null, null, null, null, null, null, null);
            
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
        public async Task TestSignup_InvalidRequest() {
            AuthenticationController controller = new AuthenticationController(_userManagerMock.Object, _tokenGenerator);
            SignupRequest request = new SignupRequest { Username = "", Email = "test@test.com", PasswordHash = "password" };
            BadRequestObjectResult badRequest = controller.BadRequest(new {result = "None of the fields should be blank."});
            BadRequestObjectResult? result = await controller.Signup(request) as BadRequestObjectResult;
            Assert.AreEqual(badRequest.StatusCode, result?.StatusCode);
        }

        [TestMethod]
        public async Task TestSignup_UserAlreadyExists() {
            //Setup Usermanager to return a user with email test@test.com
            IdentityUser existingUser = new IdentityUser { UserName = "testUser", Email = "test@test.com" };
            _userManagerMock.Setup(userManager => userManager.FindByNameAsync("testUser")).ReturnsAsync(existingUser);

            SignupRequest request = new SignupRequest { Username = "testUser", Email = "test@test.com", PasswordHash = "password" };
            AuthenticationController controller = new AuthenticationController(_userManagerMock.Object, _tokenGenerator);
            BadRequestObjectResult badRequest = controller.BadRequest(new { result = "A user with that username already exists" });
            BadRequestObjectResult? result = await controller.Signup(request) as BadRequestObjectResult;
            Assert.AreEqual(badRequest.StatusCode, result?.StatusCode);
        }

        [TestMethod]
        public async Task TestSignUp_NormalRequest() {
            //Setup Usermanager to return a user with email test@test.com
            IdentityUser existingUser = new IdentityUser { UserName = "testUser", Email = "test@test.com" };
            _userManagerMock.Setup(userManager => userManager.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(existingUser);
            _userManagerMock.Setup(userManager => userManager.CreateAsync(It.IsAny<IdentityUser>()));
            SignupRequest request = new SignupRequest { Username = "test2", Email = "test2@test.com", PasswordHash = "password" };
            AuthenticationController controller = new AuthenticationController(_userManagerMock.Object, _tokenGenerator);
            OkObjectResult? result = await controller.Signup(request) as OkObjectResult;
            Assert.AreEqual(200, result?.StatusCode);
        }

    }
}
