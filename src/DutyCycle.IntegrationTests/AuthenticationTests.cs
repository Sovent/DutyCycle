using System.Net;
using System.Threading.Tasks;
using DutyCycle.API.Models;
using NUnit.Framework;

namespace DutyCycle.IntegrationTests
{
    public class AuthenticationTests : IntegrationTests
    {
        [SetUp]
        public async Task SetupOrganization()
        {
            _adminCredentials = await CreateOrganizationAndAssertSuccess();
        }
        
        [Test]
        public async Task GetGroupsAfterSigningOut_RespondsUnauthorized()
        {
            await SignOut();
            
            var response = await HttpClient.GetAsync("/groups");
            
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Test]
        public async Task GetGroupsAfterSigningOutAndSigningBackIn_RespondsOk()
        {
            await SignOut();
            await SignIn(_adminCredentials);
            
            var response = await HttpClient.GetAsync("/groups");
            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        private UserCredentials _adminCredentials;
    }
}