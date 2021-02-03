using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using DutyCycle.API;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Polly;

namespace DutyCycle.IntegrationTests
{
    public class ScheduledJobTests : IntegrationTests
    {
        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            Worker.Configure(serviceCollection);
        }

        [Test]
        public async Task MakeGroupWithEnoughMembersToRotate_RotatesAccordingToTheCronExpression()
        {
            var firstMemberName = Fixture.Create<string>();
            var secondMemberName = Fixture.Create<string>();
            var groupToRotateId = await CreateGroupAndGetId(cyclingCronExpression: "* * * * *", dutiesCount: 1);
            await AddMember(groupToRotateId, firstMemberName);
            await AddMember(groupToRotateId, secondMemberName);

            await WaitForEntityToSatisfyCondition(
                () => GetGroup(groupToRotateId), 
                group =>
                {
                    var singleDuty = group.CurrentDuties.Single();
                    var singleNonDuty = group.NextDuties.Single();
                    return firstMemberName == singleNonDuty.Name && secondMemberName == singleDuty.Name;
                }, 
                TimeSpan.FromMinutes(2));
        }

        private static async Task WaitForEntityToSatisfyCondition<T>(
            Func<Task<T>> getEntity,
            Func<T, bool> condition,
            TimeSpan maxWaitingTime)
        {
            var timeoutPolicy = Policy.TimeoutAsync(maxWaitingTime);
            var retryPolicy = Policy.HandleResult<T>(entity => !condition(entity))
                .WaitAndRetryForeverAsync(attempt => TimeSpan.FromSeconds(5 + attempt));
            var resultPolicy = timeoutPolicy.WrapAsync(retryPolicy);
            var s = await resultPolicy.ExecuteAsync(getEntity);
            Assert.IsTrue(condition(s));
        }
    }
}