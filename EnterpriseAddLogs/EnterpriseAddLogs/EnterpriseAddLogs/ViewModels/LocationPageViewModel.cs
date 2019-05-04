using EnterpriseAddLogs.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseAddLogs.ViewModels
{
    public class LocationPageViewModel: PageViewModel
    {

        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double? Speed { get; set; }

        public LocationPageViewModel(INavigator navigator)
            :base(navigator)
        {
            SetLocation();
        }

        public async void SetLocation()
        {
            var position = await Xamarin.Essentials.Geolocation.GetLocationAsync();
            Longitude = position.Longitude;
            Latitude = position.Latitude;
            Speed = position.Speed;
        }
    }
}
