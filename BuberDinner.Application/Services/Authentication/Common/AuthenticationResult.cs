using BuberDinner.Domain.Entities;

namespace BuberDinner.Application.Services.Authentication.Common;


public record AuthenticationResult
(
    User user,
    string Token
);