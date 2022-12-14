using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using PlannerAPI.Services.Authentication;

namespace PlannerAPITests {
    [TestClass]
    public class TokenGeneratorTests {
        [TestMethod]
        public void GenerateTest() {
            Dictionary<string, string> configDict = new Dictionary<string, string>{
                {"Jwt", ""},
                {"Jwt:key", "yROdirqY0vthrTth"},
                {"Jwt:issuer", "https://localhost:7074" },
                {"Jwt:audience", "https://localhost:7074"}
            };

            IConfiguration testConfig = new ConfigurationBuilder().AddInMemoryCollection(configDict).Build();
            TokenGenerator generator = new TokenGenerator(testConfig);
            IdentityUser user = new IdentityUser { UserName = "testUser" };
            Assert.IsTrue(generator.Generate(user).Length > 0);
        }

    }
}