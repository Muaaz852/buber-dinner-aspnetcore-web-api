using BuberDinner.Api.Controllers.API;
using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Queries.Login;
using BuberDinner.Application.Services.Authentication.Common;
using BuberDinner.Contracts.Authentication;
using BuberDinner.Domain.Common.Errors;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers.Api;

//[ApiController]
[Route("auth")]
public class AuthenticationController : ApiController
{
    #region Private Fields
    private readonly IMediator _mediator;
    #endregion

    #region Constructor
    public AuthenticationController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    #endregion

    #region CRUD - C

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var command = new RegisterCommand(
                registerRequest.FirstName,
                registerRequest.LastName,
                registerRequest.Email,
                registerRequest.Password
            );
            ErrorOr<AuthenticationResult> registerResponse = await _mediator.Send(command);

            return registerResponse.Match(
                authResponse => Ok(MapAuthResponse(authResponse)),
                errors => Problem(errors));
        }
        catch (Exception ex)
        {
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: ex.InnerException?.ToString() ?? "An error occurred while registering");
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var query = new LoginQuery(request.Email, request.Password);
            var authResult = await _mediator.Send(query);
            if (authResult.IsError && authResult.FirstError == Errors.Authentication.InvalidCredentials)
            {
                return Problem(
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: authResult.FirstError.Description);
            }

            return authResult.Match(
                authResult => Ok(MapAuthResponse(authResult)),
                errors => Problem(errors));
        }
        catch (Exception ex)
        {
            return Problem(title: ex.InnerException?.ToString() ?? "An error occurred while logging in");
        }
    }
    private static AuthenticationResponse MapAuthResponse(AuthenticationResult authResponse)
    {
        return new AuthenticationResponse(
                        authResponse.user.Id.ToString(),
                        authResponse.user.FirstName,
                        authResponse.user.LastName,
                        authResponse.user.Email,
                        authResponse.Token);
    }
    #endregion

}

