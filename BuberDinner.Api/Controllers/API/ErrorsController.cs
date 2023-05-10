using BuberDinner.Application.Common.Errors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers.API
{
    public class ErrorsController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Error()
        {
            Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
            var (statusCode, message) = exception switch
            {
                IServiceException serviceException => ((int)serviceException.statusCode, serviceException.errorMessage),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occoured"),

            };
            return Problem(title: exception?.Message);
        }
    }
}
