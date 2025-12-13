namespace FuelMaster.Website.Extensions;

public static class HttpResponseExtensions
{
    public static void SetAuthCookie(this HttpResponse response, string token, DateTime expiresAt)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = expiresAt,
            Path = "/"
        };

        response.Cookies.Append("access_token", token, cookieOptions);
    }

    public static void DeleteAuthCookie(this HttpResponse response)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(-1), // Set to past date to ensure deletion
            Path = "/"
        };

        response.Cookies.Append("access_token", string.Empty, cookieOptions);
    }
}

