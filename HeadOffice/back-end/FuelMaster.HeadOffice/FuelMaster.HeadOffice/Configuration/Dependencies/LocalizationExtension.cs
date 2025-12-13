using FuelMaster.HeadOffice.Core.Helpers;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace FuelMaster.HeadOffice.Extensions.Dependencies
{
    public static class LocalizationExtension
    {
        public static IServiceCollection AddFuelMasterLocalization (this IServiceCollection services)
        {
            services.AddLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                List<CultureInfo> supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en"),
                    new CultureInfo("ar"),
                };

                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.FallBackToParentUICultures = true;

                var defaultCultureProvider = options
                    .RequestCultureProviders
                    .FirstOrDefault(a => a.GetType() == typeof(AcceptLanguageHeaderRequestCultureProvider));

                if (defaultCultureProvider is not null)
                    options.RequestCultureProviders.Remove(defaultCultureProvider);

                CultureInfo.DefaultThreadCurrentUICulture = supportedCultures[0];
                CultureInfo.DefaultThreadCurrentCulture = supportedCultures[0];

                //CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";
                //CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ".";

                options.AddInitialRequestCultureProvider(new CustomRequestCultureProvider(async context =>
                {
                    var languageHeader = context.Request.Headers["X-Language"];

                    if (string.IsNullOrWhiteSpace(languageHeader))
                        LocalizationUtilities.SetCulture("en");
                    else
                        LocalizationUtilities.SetCulture(languageHeader.ToString());

                    var lang = CultureInfo.CurrentUICulture?.TwoLetterISOLanguageName ?? "en";

                    // My custom request culture logic
                    return await Task.FromResult(new ProviderCultureResult(lang));
                }));
            });

            return services;
        }
    }
}
