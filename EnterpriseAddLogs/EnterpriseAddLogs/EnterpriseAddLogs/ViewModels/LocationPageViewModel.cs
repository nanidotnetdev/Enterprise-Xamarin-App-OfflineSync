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
            var position = await Geolocation.GetLocationAsync();
            Longitude = position.Longitude;
            Latitude = position.Latitude;
            Speed = position.Speed;
        }
    }
}
