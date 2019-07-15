using System;
using Acr.UserDialogs;
using EnterpriseAddLogs.Helpers;
using Xamarin.Essentials;

namespace EnterpriseAddLogs.ViewModels
{
    public class LocationPageViewModel: PageViewModel
    {

        private double _longitude;

        private double _latitude;

        private double? _speed;


        public double Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        public double Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }

        public double? Speed
        {
            get => _speed;
            set => SetProperty(ref _speed, value);
        }

        public LocationPageViewModel(INavigator navigator)
            :base(navigator)
        {
            SetLocation();
        }

        public async void SetLocation()
        {
            try
            {
                var position = await Geolocation.GetLocationAsync();
                Longitude = position.Longitude;
                Latitude = position.Latitude;
                Speed = position.Speed;
                Console.WriteLine($"Latitude: {position.Latitude}, Longitude: {position.Longitude}, Altitude: {position.Altitude}");
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                Notifications.Alert("Location Feature Not supported!");
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                Notifications.Alert("Location Services Not enabled!");
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                Notifications.Alert("Need Location Permissions!");
            }
            catch (Exception ex)
            {
                // Unable to get location
                Notifications.Alert(ex.Message);
            }
        }
    }
}
