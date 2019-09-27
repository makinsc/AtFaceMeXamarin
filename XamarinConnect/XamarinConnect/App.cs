//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

using ATFaceME.Xamarin.Core.Utils;
using ATFaceME.Xamarin.Core.Views;
using Microsoft.Identity.Client;
using Xamarin.Forms;

namespace ATFaceME.Xamarin.Core
{
    public partial class App : Application
    {
        public static PublicClientApplication IdentityClientApp;
        public static string ClientID = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"; //encripted for security reasons
        public static string RedirectUri = "msal" + ClientID + "://auth";
        public static string Authority = "https://login.microsoftonline.com/common/v2.0";
        public static string[] Scopes = { ClientID };
        public static string extraScope = "User.Read Mail.Send Files.ReadWrite";
        public static string Username = string.Empty;
        public static string UserEmail = string.Empty;

        public App()
        {
            IdentityClientApp = new PublicClientApplication(Authority, ClientID);
            MainPage = new NavigationPage(new LoginPage());
            ImageUtils.LoadDummies();
            CreateStyles();
        }

        private void CreateStyles()
        {
            var buttonStyle = new Style(typeof(Button))
            {
                Setters =
                {
                    new Setter { Property = Button.FontSizeProperty, Value = NamedSize.Small }
                }
            };

            var labelStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.TextColorProperty, Value = Color.Black },
                    new Setter { Property = Label.FontSizeProperty, Value = NamedSize.Small }
                }
            };

            var entryStyle = new Style(typeof(Entry))
            {
                Setters =
                {
                    new Setter { Property = Entry.BackgroundColorProperty, Value = Color.Gray },
                    new Setter { Property = Entry.FontSizeProperty, Value = NamedSize.Small }
                }
            };

            var pickerStyle = new Style(typeof(Picker))
            {
                Setters =
                {
                    new Setter { Property = Picker.BackgroundColorProperty, Value = Color.Gray }
                }
            };

            Resources = new ResourceDictionary();
            Resources.Add(buttonStyle);
            Resources.Add(labelStyle);
            Resources.Add(entryStyle);
            Resources.Add(pickerStyle);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
