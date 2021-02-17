using System.Net;
using DutyCycle.API.Models;
using DutyCycle.Common;
using DutyCycle.Groups.Domain;
using DutyCycle.Groups.Domain.Organizations;
using DutyCycle.Groups.Domain.Slack;
using DutyCycle.Users.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DutyCycle.API.Filters
{
    public class DomainExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            void SetResult<T>(DomainException<T> exception, HttpStatusCode statusCode) where T : DomainError<T>
            {
                context.Result = new ObjectResult(
                    new ErrorResponse
                    {
                        ErrorDescription = exception.Error.Description
                    })
                {
                    StatusCode = (int) statusCode
                };
                context.ExceptionHandled = true;
            }
            
            switch (context.Exception)
            {
                case DomainException<GroupNotFound> exception:
                    SetResult(exception, HttpStatusCode.NotFound);
                    break;
                case DomainException<InvalidGroupSettings> exception:
                    SetResult(exception, HttpStatusCode.BadRequest);
                    break;
                case DomainException<SlackInteractionFailed> exception:
                    SetResult(exception, HttpStatusCode.BadRequest);
                    break;
                case DomainException<OrganizationNotFound> exception:
                    SetResult(exception, HttpStatusCode.NotFound);
                    break;
                case DomainException<CouldNotSignUpUser> exception:
                    SetResult(exception, HttpStatusCode.BadRequest);
                    break;
                case DomainException<PermissionDenied> exception:
                    SetResult(exception, HttpStatusCode.Forbidden);
                    break;
                case DomainException<SlackConnectionNotFound> exception:
                    SetResult(exception, HttpStatusCode.NotFound);
                    break;
            }
        }
    }
}