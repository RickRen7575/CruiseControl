using Android.App;
using Android.Widget;
using Android.OS;
using Android.Locations;
using System.Collections.Generic;
using System.Linq;
using Android.Runtime;
using System;

namespace CruiseControlAndroid
{
    [Activity(Label = "CruiseControl", MainLauncher = false, Icon = "@drawable/icon")]
    public class MainActivity : Activity, ILocationListener
    {
        const float metersPerMile = (float)1609.344;
        const float metersPmsTomilesPhour = (float)2236.94;
        const float metersToMiles = (float)0.000621371;
        const float speedThreshold = 1;
        int _points = 0;
        float _currentProgress = 0;
        Location _currentLocation;
        LocationManager _locationManager;
        string _locationProvider;
        TextView _pointsView;
        ProgressBar _progressBar;
        TextView _progressText;
        TextView _messagePrompt;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _pointsView = FindViewById<TextView>(Resource.Id.PointsDisplayLabel);
            _pointsView.Text = _points.ToString();

            _progressBar = FindViewById<ProgressBar>(Resource.Id.CurrentProgressBar);
            _progressBar.Progress = Convert.ToInt32((_currentProgress / metersPerMile) * 100);

            _progressText = FindViewById<TextView>(Resource.Id.CurrentProgressText);
            _progressText.Text = (_currentProgress * 0.000621371).ToString() + " / 1 mile";

            _messagePrompt = FindViewById<TextView>(Resource.Id.MessagePrompt);
            _messagePrompt.Text = "Start driving to earn points!";

            InitializeLocationManager();
        }

        protected override void OnResume()
        {
            base.OnResume();
            _locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
        }

        protected override void OnPause()
        {
            base.OnPause();
            _locationManager.RemoveUpdates(this);
        }

        #region Location Management

        private void InitializeLocationManager()
        {
            _locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria { Accuracy = Accuracy.Fine };
            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);
            if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                _locationProvider = string.Empty;
            }
        }

        public void OnLocationChanged(Location location)
        {
            if (_currentLocation != null)
            {
                long timeDifference = location.Time - _currentLocation.Time;
                float locationDifference = _currentLocation.DistanceTo(location);

                float speed = (locationDifference / timeDifference) * metersPmsTomilesPhour;

                if (speed > speedThreshold)
                {
                    _currentProgress += locationDifference;
                    while (_currentProgress > metersPerMile)
                    {
                        _points++;
                        _pointsView.Text = _points.ToString();
                        _currentProgress -= metersPerMile;
                    }
                    _messagePrompt.Text = "Lock Phone";
                }
                else
                {
                    _messagePrompt.Text = "Start driving to earn points!";
                }
            }
            _currentLocation = location;
            _progressText.Text = String.Format("{0:0.00} / 1 mile", (_currentProgress * metersToMiles));
            _progressBar.Progress = Convert.ToInt32((_currentProgress / metersPerMile) * 100);
        }

        public void OnProviderDisabled(string provider) { }

        public void OnProviderEnabled(string provider) { }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras) { }

        #endregion
    }
}

