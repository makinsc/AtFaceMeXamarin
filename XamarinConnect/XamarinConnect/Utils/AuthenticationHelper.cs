using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ATFaceME.Xamarin.Core
{
    public class AuthenticationHelper
    {
        public static string TokenForUser = null;
        public static DateTimeOffset Expiration;
        public static string EmailUser;
        private static List<string> AdminList = new List<string>()
        {
            "jjsalguero@atsistemas.com",
            "agallego@atsistemas.com",
            "dgarcia.romero@atsistemas.com",
            "jloliva@atsistemas.com",
            "jutrera@atsistemas.com",
            "rdjimenez@atsistemas.com",
            "dcrespo@atsistemas.com"
        };

        public static bool IsAuthenticated()
        {
            return (TokenForUser != null);
        }

        public static bool IsAdmin
        {
            get { return AdminList.Exists(a => a == EmailUser); }
        }

        /// <summary>
        /// Get Token for User.
        /// </summary>
        /// <returns>Token for user.</returns>
        public static async Task<string> Authenticate()
        {
            if (TokenForUser == null || Expiration <= DateTimeOffset.UtcNow.AddMinutes(5))
            {
                try
                {
                    AuthenticationResult authResult = await App.IdentityClientApp.AcquireTokenAsync(App.Scopes,
                        "", UiOptions.ForceLogin, "", App.extraScope.Split(' '), "https://login.microsoftonline.com/common/v2.0", ""
                        );
                    TokenForUser = authResult.IdToken;
                    Expiration = authResult.ExpiresOn;
                    EmailUser = authResult.User.DisplayableId;
                }
                catch (Exception ex)
                {
                    TokenForUser = null;
                    Debug.WriteLine("Error acquiring the token: " + ex.Message);
                }
            }
            return (IsAuthenticated()) ? "OK" : "Autenticación fallida";
        }

        /// <summary>
        /// Signs the user out of the service.
        /// </summary>
        public static void SignOut()
        {          
            TokenForUser = null;
        }
    }
}
