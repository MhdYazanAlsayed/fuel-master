using FuelMaster.HeadOffice.Core.Resources;
using System.Globalization;

namespace FuelMaster.HeadOffice.Core.Helpers
{
    public static class LocalizationUtilities
    {
        public static void SetCulture(string lang = "en")
        {
            var culture = new CultureInfo(lang);

            if (CultureInfo.CurrentUICulture?.TwoLetterISOLanguageName != culture.TwoLetterISOLanguageName)
            {
                ChangeCulture(culture);
            }
        }
        private static void ChangeCulture(CultureInfo newCulture)
        {
            Resource.Culture =
                Thread.CurrentThread.CurrentCulture =
                CultureInfo.DefaultThreadCurrentCulture =
                CultureInfo.CurrentUICulture = newCulture;

            Resource.Culture =
                Thread.CurrentThread.CurrentUICulture =
                CultureInfo.DefaultThreadCurrentUICulture =
                CultureInfo.CurrentCulture = newCulture;
        }
        public static bool IsArabic()
        {
            return Thread.CurrentThread?.CurrentUICulture?.TwoLetterISOLanguageName?.Contains("ar") ?? false;
        }
    }

}
