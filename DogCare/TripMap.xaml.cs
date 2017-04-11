using Android;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

using Android.Content.PM;


namespace DogCare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TripMap : ContentPage
    {
        Map map;

        public object LabelGeolocation { get; private set; }

        public TripMap()
        {
            InitializeComponent();

            map = new Map
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            currentLocation();

            stack.Children.Add(map);
            Content = stack;
        }


        async void currentLocation()
        {           
            if (CrossGeolocator.Current.IsGeolocationEnabled == true)
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 5;
                var position = await locator.GetPositionAsync(10000);
                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMiles(0.05)));
            }
            else
            {

                DisplayAlertGPS("DogCare Require location", "Please turn on location", "ok");
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            currentLocation();
        }



        public void DisplayAlertGPS(string message, string title, string button1)
        {
            Device.BeginInvokeOnMainThread(() => {
                DisplayAlert(message, title, button1);
            });
        }
    }
}