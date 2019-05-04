using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using Xamarin.Essentials;

namespace EnterpriseAddLogs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LocationPage : ContentPage
	{
		public LocationPage ()
		{
			InitializeComponent ();

            AddLocation();
		}

        public async void AddLocation()
        {
            //var position = await Xamarin.Essentials.Geolocation.GetLocationAsync();

            //LongitudeValue.Text = position.Longitude.ToString();
            //LatitudeValue.Text = position.Latitude.ToString();
            //SpeedValue.Text = position.Speed.ToString();

            //try
            //{
            //    string mapKey = null; //only needed on UWP
            //    //var addresses = await locator.GetAddressesForPositionAsync(position, mapKey);
            //    var address = addresses.FirstOrDefault();

            //    if (address == null)
            //    {
            //        //Console.WriteLine("No address found for position.");
            //        DisplayAlert("Erro", "No Address Found", "ok");
            //    }
            //    else
            //    {
            //        //Console.WriteLine("Addresss: {0} {1} {2}", address.Thoroughfare, address.Locality, address.CountryCode);
            //        AddressValue.Text = string.Format("Address: {0} {1} {2}",address.Thoroughfare, address.Locality, address.CountryCode);
            //        AddressValue.Text = string.Format("Address: {0}\n{1}\n{2}\n{3}\n{4}\n{5}\n{6}\n{7}\n{8}\n {9}",
            //            address.AdminArea,
            //            address.CountryName,
            //            address.FeatureName,
            //            address.SubAdminArea,
            //            address.SubLocality,
            //            address.SubThoroughfare,
            //            address.Thoroughfare ,
            //            address.Locality,
            //            address.PostalCode, 
            //            address.CountryCode);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine("Unable to get address: " + ex);
            //}
        }
    }
}