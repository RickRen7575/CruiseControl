using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CruiseControlAndroid
{
    [Activity(Label = "CruiseControl", MainLauncher = false, Icon = "@drawable/icon")]
    public class RewardsActivity : Activity
    {
        Button _backButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Rewards);

            _backButton = FindViewById<Button>(Resource.Id.backButton);
            _backButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            };
        }
    }
}