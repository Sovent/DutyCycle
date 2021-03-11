using SlackAPI;

namespace DutyCycle.Infrastructure.Slack
{
    [RequestPath("team.info")]
    public class TeamInfoResponse : Response
    {
        public Team team;
    }
}