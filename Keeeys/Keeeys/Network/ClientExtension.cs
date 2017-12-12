using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Keeeys.Common.Network
{
    public partial class Client
    {
        public const string AuthCookieName = "sessionCode";


        public string Token { get; set; }

        public void LinkCookies()
        {
            Cookies.Add(new Uri(BaseUrl), new System.Net.Cookie(Client.AuthCookieName, Token));
        }

        partial void ProcessResponse(System.Net.Http.HttpClient client, System.Net.Http.HttpResponseMessage response)
        {
            var pageUri = response.RequestMessage.RequestUri;
            var cookieContainer = new CookieContainer();
            IEnumerable<string> cookies;
            if (response.Headers.TryGetValues("set-cookie", out cookies))
            {
                foreach (var c in cookies)
                {
                    cookieContainer.SetCookies(pageUri, c);
                }

                CookieCollection collection = cookieContainer.GetCookies(pageUri);
                Cookie sessionCode = collection[AuthCookieName];
                if (sessionCode != null)
                    Token = sessionCode.Value;
            }
        }
    }
}
