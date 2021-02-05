using System;
using System.Linq;
using DutyCycle.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DutyCycle.API.Filters
{
    public class ModelValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var error = new ErrorResponse
                {
                    ErrorDescription = CreateErrorDescription(context.ModelState)
                };
                context.Result = new BadRequestObjectResult(error);
            }

            base.OnActionExecuting(context);
        }

        private static string CreateErrorDescription(ModelStateDictionary modelState)
        {
            var allErrors = modelState.Values.SelectMany(value => value.Errors.Select(error => error.ErrorMessage));
            return string.Join(Environment.NewLine, allErrors);
        }
    }
}