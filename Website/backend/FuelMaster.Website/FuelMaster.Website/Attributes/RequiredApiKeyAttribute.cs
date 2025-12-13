using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FuelMaster.Website.Attributes;

public class RequiredApiKeyAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var apiKey = context.HttpContext.Request.Headers["X-API-KEY"].ToString();
        if (string.IsNullOrEmpty(apiKey))
        {
            context.Result = new UnauthorizedObjectResult(new { message = "API key is required" });
        }

        var key = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>()["Headoffice:ApiKey"];
        if (apiKey != key)
        {
            context.Result = new UnauthorizedObjectResult(new { message = "Invalid API key" });
        }
    }
}