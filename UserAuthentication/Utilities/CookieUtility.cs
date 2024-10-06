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
                    Expires = expireTime.HasValue ? DateTime.Now.AddMinutes(expireTime.Value) : DateTime.Now.AddDays(14),
                    HttpOnly = false,
                    Secure = true,
                    SameSite = SameSiteMode.None
                };

                response.Cookies.Append(cookie.Key, cookie.Value, cookieOptions);
            }
        }
    }
}
