using System;

namespace Novini.Extensions
{
    public static class String
    {
        public static string GetDomain(this string url)
        {
            Uri originalUrl = new Uri(url);
            return originalUrl.Host;
        }
    }
}
