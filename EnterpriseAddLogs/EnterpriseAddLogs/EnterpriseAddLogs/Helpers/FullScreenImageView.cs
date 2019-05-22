using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using EnterpriseAddLogs.Models;
using Stormlion.PhotoBrowser;
using Xamarin.Forms;

namespace EnterpriseAddLogs.Helpers
{
    public static class FullScreenImageView
    {
        public static void Show(IEnumerable<FileSource> files, int startIndex = 0)
        {
            var photos = files.Where(f => f.Image.GetValue(UriImageSource.UriProperty) != null)
                .Select(f => new Photo
                {
                    URL = f.Image.GetValue(UriImageSource.UriProperty)?.ToString(),
                    Title = f.Text
                }).ToList();

            new PhotoBrowser
            {
                Photos = photos,
                StartIndex = startIndex,
                ActionButtonPressed = index =>
                {
                    UserDialogs.Instance
                        .ActionSheet(new ActionSheetConfig()
                        .Add("Close", PhotoBrowser.Close));
                },
                EnableGrid = true
            }.Show();
        }
    }
}


