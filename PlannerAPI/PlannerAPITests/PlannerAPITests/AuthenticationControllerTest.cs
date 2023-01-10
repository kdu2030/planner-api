using PlannerAPI.Controllers;
using Microsoft.AspNetCore.Identity;
using Moq;
using Microsoft.Extensions.Configuration;
using PlannerAPI.Services.Authentication;
using Microsoft.AspNetCore.Mvc;
using PlannerAPI.Types;
using PlannerAPI.Models;

namespace PlannerAPITests {
    [TestClass]
    public class AuthenticationControllerTest {
        private IUserStore<PlannerUser> _userStore;
        private Mock<UserManager<PlannerUser>> _userManagerMock;
        private TokenGenerator _tokenGenerator;

        [TestInitialize]
        public void Setup() {
            _userStore = Mock.Of<IUserStore<PlannerUser>>();
            
            _userManagerMock = new Mock<UserManager<PlannerUser>>(_userStore, null, null, null, null, null, null, null, null);
            
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
            BasicResult badRequestResult = new BasicResult("Empty Fields");
            OkObjectResult? result = await controller.Signup(request) as OkObjectResult;
            Assert.AreEqual(200, result?.StatusCode);
            Assert.AreEqual(badRequestResult, result?.Value);
        }

        [TestMethod]
        public async Task TestSignup_UserAlreadyExists() {
            //Setup Usermanager to return a user with username testUser
            PlannerUser existingUser = new PlannerUser { UserName = "testUser", Email = "test@test.com" };
            _userManagerMock.Setup(userManager => userManager.FindByNameAsync("testUser")).ReturnsAsync(existingUser);

            SignupRequest request = new SignupRequest { Username = "testUser", Email = "test2@test.com", PasswordHash = "password" };
            AuthenticationController controller = new AuthenticationController(_userManagerMock.Object, _tokenGenerator);
            BasicResult expectedResult = new BasicResult("User with username exists");
            OkObjectResult? result = await controller.Signup(request) as OkObjectResult;
            Assert.AreEqual(200, result?.StatusCode);
            Assert.AreEqual(expectedResult, result?.Value);
        }

        [TestMethod]
        public async Task TestSignup_UserWithEmailAlreadyExists() {
            //Setup Usermanager to return a user with email test@test.com
            PlannerUser existingUser = new PlannerUser { UserName = "testUser", Email = "test@test.com" };
            _userManagerMock.Setup(userManager => userManager.FindByEmailAsync("test@test.com")).ReturnsAsync(existingUser);

            SignupRequest request = new SignupRequest { Username = "testUser2", Email = "test@test.com", PasswordHash = "password" };
            AuthenticationController controller = new AuthenticationController(_userManagerMock.Object, _tokenGenerator);
            BasicResult expectedResult = new BasicResult("User with email exists");
            OkObjectResult? result = await controller.Signup(request) as OkObjectResult;
            Assert.AreEqual(200, result?.StatusCode);
            Assert.AreEqual(expectedResult, result?.Value);
        }

        [TestMethod]
        public async Task TestSignUp_NormalRequest() {
            //Setup Usermanager to return a user with email test@test.com
            PlannerUser existingUser = new PlannerUser { UserName = "testUser", Email = "test@test.com" };
            _userManagerMock.Setup(userManager => userManager.FindByNameAsync("testUser")).ReturnsAsync(existingUser);
            _userManagerMock.Setup(userManager => userManager.FindByEmailAsync("test@test.com")).ReturnsAsync(existingUser);
            _userManagerMock.Setup(userManager => userManager.CreateAsync(It.IsAny<PlannerUser>()));
            
            SignupRequest request = new SignupRequest { Username = "test2", Email = "test2@test.com", PasswordHash = "password" };
            AuthenticationController controller = new AuthenticationController(_userManagerMock.Object, _tokenGenerator);
            OkObjectResult? result = await controller.Signup(request) as OkObjectResult;
            Assert.AreEqual(200, result?.StatusCode);
            Assert.IsNotNull(result?.Value);
        }

        [TestMethod]
        public async Task TestLogin_BadRequest() {
            LoginRequest request = new LoginRequest { Email = "", PasswordHash= "password" };
            AuthenticationController controller = new AuthenticationController(_userManagerMock.Object, _tokenGenerator);
            OkObjectResult? result = await controller.Login(request) as OkObjectResult;
            BasicResult expectedResult = new BasicResult("Empty Fields");
            Assert.AreEqual(200, result?.StatusCode);
            Assert.AreEqual(expectedResult, result?.Value);
        }

        [TestMethod]
        public async Task TestLogin_UserNotFound() {
            LoginRequest request = new LoginRequest { Email = "test@test.com", PasswordHash = "password" };
            AuthenticationController controller = new AuthenticationController(_userManagerMock.Object, _tokenGenerator);
            OkObjectResult? result = await controller.Login(request) as OkObjectResult;
            BasicResult noUserResult = new BasicResult("User Not Found");
            Assert.AreEqual(200, result?.StatusCode);
            Assert.AreEqual(noUserResult, result?.Value);
        }

        [TestMethod]
        public async Task TestLogin_IncorrectPassword() {
            //Setup Usermanager to return a user with email test@test.com
            PlannerUser existingUser = new PlannerUser { UserName = "testUser", Email = "test@test.com", PasswordHash = "password" };
            _userManagerMock.Setup(userManager => userManager.FindByEmailAsync(existingUser.Email)).ReturnsAsync(existingUser);

            LoginRequest request = new LoginRequest { Email = "test@test.com", PasswordHash = "password1" };
            AuthenticationController controller = new AuthenticationController(_userManagerMock.Object, _tokenGenerator);
            BasicResult incorrectPasswordResult = new BasicResult("Incorrect password");
            OkObjectResult? result = await controller.Login(request) as OkObjectResult;
            Assert.AreEqual(200, result?.StatusCode);
            Assert.AreEqual(incorrectPasswordResult, result?.Value);
        }

        [TestMethod]
        public async Task TestLogin_CorrectPassword() {
            //Setup Usermanager to return a user with email test@test.com
            PlannerUser existingUser = new PlannerUser { UserName = "testUser", Email = "test@test.com", PasswordHash = "password" };
            _userManagerMock.Setup(userManager => userManager.FindByEmailAsync(existingUser.Email)).ReturnsAsync(existingUser);

            LoginRequest request = new LoginRequest { Email = "test@test.com", PasswordHash = "password" };
            AuthenticationController controller = new AuthenticationController(_userManagerMock.Object, _tokenGenerator);
            OkObjectResult? result = await controller.Login(request) as OkObjectResult;
            Assert.AreEqual(200, result?.StatusCode);
            Assert.IsNotNull(result?.Value);
        }




    }
}
