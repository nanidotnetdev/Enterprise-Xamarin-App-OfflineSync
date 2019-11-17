using System;
using System.Threading.Tasks;
using EnterpriseAddLogs.Helpers;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using Map = Xamarin.Forms.Maps.Map;

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

        public Map Map { get; private set; }

        public LocationPageViewModel(INavigator navigator)
            :base(navigator)
        {
            Map = new Map();

            SetLocation();
        }

        public async void SetLocation()
        {
            Location position = null;

            if (await CheckLocationAccess())
            {
                position = await Geolocation.GetLocationAsync();
            }

            if (position != null)
            {
                Longitude = position.Longitude;
                Latitude = position.Latitude;
                Speed = position.Speed;

                Map.IsShowingUser = true;
                Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude),
                    Distance.FromMiles(1)));
            }
            else
            {
                Notifications.Alert("Need Location Permissions!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CheckLocationAccess()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);

                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        //await DisplayAlert("Need location", "Gunna need that location", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    status = results[Permission.Location];

                }

                if (status == PermissionStatus.Granted)
                {
                    //Query permission
                    return true;
                }
                // if (status != PermissionStatus.Unknown)
                //{
                //    //location denied

                //}
            }
            catch (Exception ex)
            {
                //Something went wrong
            }

            return false;
        }
    }
}
