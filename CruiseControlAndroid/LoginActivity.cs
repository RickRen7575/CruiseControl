using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CruiseControlAndroid
{
    [Activity(Label = "CruiseControl", MainLauncher = true, Icon = "@drawable/icon")]
    public class LoginActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Login);
            Button loginButton = FindViewById<Button>(Resource.Id.loginButton);

            loginButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            };

            //FacebookSdk.sdkInitialize(getApplicationContext());
            //AppEventsLogger.activateApp(this);
        }
    }
}