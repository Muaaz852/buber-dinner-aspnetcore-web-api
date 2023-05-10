using System.Net;
namespace BuberDinner.Application.Common.Errors
{
    public interface IServiceException
    {
        public HttpStatusCode statusCode { get; }
        public string errorMessage { get; }
    }
}