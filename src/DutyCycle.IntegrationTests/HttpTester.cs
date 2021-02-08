using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DutyCycle.IntegrationTests
{
    public static class HttpTester
    {
        public static async Task<HttpResponseMessage> ExecuteAndAssertStatus(
            Func<Task<HttpResponseMessage>> execute,
            HttpStatusCode statusCode)
        {
            var response = await execute();
            Assert.AreEqual(statusCode, response.StatusCode);
            return response;
        }
    }
}