using API.DTOs;
using API.Extensions;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AuthController(SignInManager<User> signInManager) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        var user = new User
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            UserName = registerDto.Email
        };

        var result = await signInManager.UserManager.CreateAsync(user, registerDto.Password);

        // format validation errors in a way that can be handled client-side
        //
        // ex: attempt to register with password of just "3"
        //
        // BEFORE ////
        //
        // [
        //     {
        //         "code": "PasswordTooShort",
        //         "description": "Passwords must be at least 6 characters."
        //     },
        //     {
        //         "code": "PasswordRequiresNonAlphanumeric",
        //         "description": "Passwords must have at least one non alphanumeric character."
        //     },
        //     {
        //         "code": "PasswordRequiresLower",
        //         "description": "Passwords must have at least one lowercase ('a'-'z')."
        //     },
        //     {
        //         "code": "PasswordRequiresUpper",
        //         "description": "Passwords must have at least one uppercase ('A'-'Z')."
        //     }
        // ]
        //
        //
        // AFTER ////
        //
        // {    
        //     "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
        //     "title": "One or more validation errors occurred.",
        //     "status": 400,
        //     "errors": {
        //         "PasswordTooShort": [
        //             "Passwords must be at least 6 characters."
        //         ],
        //         "PasswordRequiresLower": [
        //             "Passwords must have at least one lowercase ('a'-'z')."
        //         ],
        //         "PasswordRequiresUpper": [
        //             "Passwords must have at least one uppercase ('A'-'Z')."
        //         ],
        //         "PasswordRequiresNonAlphanumeric": [
        //             "Passwords must have at least one non alphanumeric character."
        //         ]
        //     },
        //     "traceId": "00-2ee08673cff7097f89b2d1281ab4045a-1853df526ec21fbc-00"
        // }
        //
        //
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            
            return ValidationProblem();
        }

        return Ok();
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        await signInManager.SignOutAsync();

        return NoContent();
    }

    [HttpGet("user-info")]
    public async Task<ActionResult> GetUserInfo()
    {
        if (User.Identity?.IsAuthenticated == false)
        {
            return NoContent();
        }

        var user = await signInManager.UserManager.GetUserByEmail(User);

        return Ok(new
        {
            user.FirstName,
            user.LastName,
            user.Email
        });
    }

    [HttpGet]
    public ActionResult GetAuthState()
    {
        return Ok(new
        {
            IsAuthenticated = User.Identity?.IsAuthenticated ?? false
        });
    }
}