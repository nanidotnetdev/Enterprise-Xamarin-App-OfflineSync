using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EnterpriseAddLogs.ViewModels;
using Autofac;
using System;
using Plugin.Media;

namespace EnterpriseAddLogs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LogCreatePage : ContentPage
	{
		public LogCreatePage ()
		{
			InitializeComponent ();
            //BindingContext = Ioc.Container.Resolve<LogCreatePageViewModel>();

            takePhoto.Clicked += async (sender, args) =>
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Sample",
                    Name = "test.jpg"
                });

                if (file == null)
                    return;

                await DisplayAlert("File Location", file.Path, "OK");

                image.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            };
        }

        private void AddComment_Button_Clicked(object sender, EventArgs e)
        {
            var trigger = new TapGestureRecognizer();
            trigger.Tapped += (s, ev) => OnLabelClicked();
            cancelLabel.GestureRecognizers.Add(trigger);

            addNewLogComment.IsVisible = true;
            btnAddComment.IsVisible = true;
            cancelLabel.IsVisible = true;
        }

        private void OnLabelClicked()
        {
            addNewLogComment.IsVisible = false;
            btnAddComment.IsVisible = false;
            cancelLabel.IsVisible = false;
        }
    }
}