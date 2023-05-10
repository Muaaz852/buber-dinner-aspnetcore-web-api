using System.Net;

namespace BuberDinner.Application.Common.Errors
{
    public class DuplicateEmailException : Exception, IServiceException
    {
        public HttpStatusCode statusCode => HttpStatusCode.Conflict;

        public string errorMessage => "Email Already Exist";
    }
}