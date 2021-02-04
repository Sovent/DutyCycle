using System.Net;
using DutyCycle.API.Models;
using DutyCycle.Errors;
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
            }
        }
    }
}