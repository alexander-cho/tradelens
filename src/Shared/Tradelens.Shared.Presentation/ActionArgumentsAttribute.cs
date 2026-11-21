using Microsoft.AspNetCore.Mvc.Filters;

namespace Tradelens.Shared.Presentation;

/// <summary>
/// Model binding has occured at this point, and we can get access to all of the arguments
/// that are going to be used to execute the action method with context.ActionArguments
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class ActionArgumentsAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var actionArguments = context.ActionArguments;

        foreach (var argument in actionArguments)
        {
            Console.WriteLine($"{argument.Key} : {argument.Value}");
        }

        await next();
    }
}