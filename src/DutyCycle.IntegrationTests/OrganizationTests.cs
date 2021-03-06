using System.Net;
using System.Threading.Tasks;
using DutyCycle.API.Models;
using AutoFixture;
using NUnit.Framework;

namespace DutyCycle.IntegrationTests
{
    public class OrganizationTests : IntegrationTests
    {
        [Test]
        public async Task CreateOrganizationWithValidRequest_CreatesSuccessfullyWithAuthCookies()
        {
            var newOrganizationInfo = new NewOrganizationInfo()
            {
                Name = Fixture.Create<string>(),
                AdminCredentials = new UserCredentials()
                {
                    Email = Fixture.Create<string>() + "@test.com",
                    Password = "Qwerty.123"
                }
            };
            
            var createOrganizationResponse = await CreateOrganizationAndSignIn(newOrganizationInfo);
            
            Assert.AreEqual(HttpStatusCode.OK, createOrganizationResponse.StatusCode);
            Assert.IsTrue(createOrganizationResponse.Headers.Contains("Set-Cookie"));
        }
        
        [Test]
        public async Task CreateOrganizationWithNoAdminCredentials_BadRequest()
        {
            var newOrganizationInfo = new NewOrganizationInfo()
            {
                Name = Fixture.Create<string>(),
                AdminCredentials = null
            };
            
            var createOrganizationResponse = await CreateOrganizationAndSignIn(newOrganizationInfo);
            
            Assert.AreEqual(HttpStatusCode.BadRequest, createOrganizationResponse.StatusCode);
        }
        
        [Test]
        public async Task CreateOrganizationWithWeakAdminPassword_BadRequest()
        {
            var newOrganizationInfo = new NewOrganizationInfo()
            {
                Name = Fixture.Create<string>(),
                AdminCredentials = new UserCredentials()
                {
                    Email = Fixture.Create<string>(),
                    Password = "weakpassword"
                }
            };
            
            var createOrganizationResponse = await CreateOrganizationAndSignIn(newOrganizationInfo);
            
            Assert.AreEqual(HttpStatusCode.BadRequest, createOrganizationResponse.StatusCode);
        }
    }
}