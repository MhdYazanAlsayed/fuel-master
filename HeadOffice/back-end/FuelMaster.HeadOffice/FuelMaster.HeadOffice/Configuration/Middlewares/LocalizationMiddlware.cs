using FuelMaster.HeadOffice.Core.Helpers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace FuelMaster.HeadOffice.Extensions.Middlewares
{
    public class LocalizationMiddlware
    {
        private readonly RequestDelegate _next;

        public LocalizationMiddlware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) 
        {
            var language = context.Request.Headers["X-Language"].ToString();
            if (!string.IsNullOrEmpty(language))
                LocalizationUtilities.SetCulture(language);
            else
                LocalizationUtilities.SetCulture("en");

            await _next(context);
        }
    }
}
