using System.Security.Authentication;
using System.Security.Claims;
using Tradelens.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Tradelens.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static async Task<User> GetUserByEmail(this UserManager<User> userManager, ClaimsPrincipal user)
    {
        var userToReturn = await userManager.Users.FirstOrDefaultAsync(x => x.Email == user.GetEmail());
        if (userToReturn == null)
        {
            throw new AuthenticationException("Email claim not found");
        }

        return userToReturn;
    }

    private static string GetEmail(this ClaimsPrincipal user)
    {
        var email = user.FindFirstValue(ClaimTypes.Email);

        if (email == null)
        {
            throw new AuthenticationException("Email claim not found");
        }

        return email;
    }
}
