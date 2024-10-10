using Azure;

namespace UserAuthentication.Utilities
{
    public class CookieUtility
    {
        public static void AddCookies(HttpResponse response, Dictionary<string, string> cookies, int? expireTime = null)
        {
            foreach (var cookie in cookies)
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = expireTime.HasValue ? DateTime.UtcNow.AddDays(expireTime.Value) : DateTime.UtcNow.AddDays(14),
                    HttpOnly = false,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    IsEssential = true,
                };

                response.Cookies.Append(cookie.Key, cookie.Value, cookieOptions);
            }
        }

        public static void DeleteCookies(HttpResponse response, string[] cookies)
        {
            foreach (string cookie in cookies)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = false,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                };
                response.Cookies.Delete(cookie, cookieOptions);
            }
        }
    }
}
             