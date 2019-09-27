
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using FFImageLoading;
using FFImageLoading.Forms.Droid;
using Microsoft.Identity.Client;
using System;
using XamarinCore = ATFaceME.Xamarin.Core;
using XamarinForms = Xamarin.Forms;

namespace ATFaceME.Xamarin.Droid
{
    [Activity(Label = "ATFaceMe", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : XamarinForms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            XamarinForms.Forms.Init(this, bundle);
            LoadApplication(new XamarinCore.App());

            CachedImageRenderer.Init(true);

        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            AuthenticationAgentContinuationHelper.SetAuthenticationAgentContinuationEventArgs(requestCode, resultCode, data);
        }

        public override void OnTrimMemory([GeneratedEnum] TrimMemory level)
        {
            ImageService.Instance.InvalidateMemoryCache();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            base.OnTrimMemory(level);
        }
    }
}

