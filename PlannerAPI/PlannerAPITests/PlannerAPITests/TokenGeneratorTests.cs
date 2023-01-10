using Microsoft.Extensions.Configuration;
using PlannerAPI.Services.Authentication;
using PlannerAPI.Models;

namespace PlannerAPITests {
    [TestClass]
    public class TokenGeneratorTests {
        private IConfiguration _testConfig;
        private TokenGenerator _tokenHandler;

        [TestInitialize]
        public void Initialize() {
             Dictionary<string, string> configDict = new Dictionary<string, string>{
                {"Jwt", ""},
                {"Jwt:key", "yROdirqY0vthrTth"},
                {"Jwt:issuer", "https://localhost:7074" },
                {"Jwt:audience", "https://localhost:7074"}
            };

            _testConfig = new ConfigurationBuilder().AddInMemoryCollection(configDict).Build();
            _tokenHandler = new TokenGenerator(_testConfig);

        }

        [TestMethod]
        public void GenerateTest() {
            PlannerUser user = new PlannerUser { UserName = "testUser", Email="test@test.com"};
            Assert.IsTrue(_tokenHandler.Generate(user).Length > 0);
        }

        [TestMethod]
        public void ValidateTest_InvalidToken() {
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6InN0cmluZyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InN0cmluZyIsImV4cCI6MTY3MjgwMTM2NywiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzA3NCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6MzAwMCJ9.ruUaqyd4fGRRv5EGGbiIGoC2q4JOMsRt_Y2V";
            string userId = _tokenHandler.Validate(token);
            Assert.AreEqual("", userId);
        }

        [TestMethod]
        public void ValidateTest_ValidToken() {
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6InlST2RpcnFZMHZ0aHJUdGgifQ.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjdkMDQxZWY1LWU1MzctNGQzZC1iM2U4LTdjNDZmNWQ4ODM2NiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InN0cmluZyIsImV4cCI6MTY3MjgwMzE2MiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzA3NCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6MzAwMCJ9.RUrJvT4A4KnCb8roXH5vBLq4TUEJEEa3110ocVePPH4";
            string userId = _tokenHandler.Validate(token);
            Assert.AreEqual("7d041ef5-e537-4d3d-b3e8-7c46f5d88366", userId);
        }

        

    }
}