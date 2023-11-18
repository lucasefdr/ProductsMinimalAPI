using Microsoft.AspNetCore.Authorization;
using ProductsMinimalAPI.Models;
using ProductsMinimalAPI.Services;

namespace ProductsMinimalAPI.Endpoints;

public static class LoginEndpoint
{
    // Login endpoint with extension method
    public static void MapLoginEndpoint(this WebApplication app)
    {

        app.MapPost(pattern: "/login", [AllowAnonymous] (UserModel userModel, ITokenService tokenService) =>
        {
            if (userModel == null) return Results.BadRequest("Invalid login.");

            if (userModel.UserName == "lucasefdr" && userModel.Password == "pass123")
            {
                var tokenString = tokenService.GetToken(
                    key: app.Configuration["Jwt:Key"],
                    issuer: app.Configuration["Jwt:Issuer"],
                    audience: app.Configuration["Jwt:Audience"],
                    userModel);

                return Results.Ok(new { token = tokenString });
            }
            else
            {
                return Results.BadRequest("Invalid login.");
            }
        }).Produces(StatusCodes.Status400BadRequest)
          .Produces(StatusCodes.Status200OK)
          .WithTags("Login");
    }
}
