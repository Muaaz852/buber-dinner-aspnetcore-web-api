using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace BuberDinner.Api.Filters
{
    public class ErrorHandlingFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var problemDetail = new ProblemDetails{
                Title = "An Error Occoured while processing you request",
                Status = (int)HttpStatusCode.InternalServerError
            };
            context.Result = new ObjectResult(problemDetail);
            context.ExceptionHandled = true;
        }
    }
}