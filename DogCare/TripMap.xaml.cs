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

        public TripMap()
        {
            InitializeComponent();
   
            currentLocation();

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

        async private void ButtonStartClicked(object sender, EventArgs e)
        {
            if (CrossGeolocator.Current.IsGeolocationEnabled == false)
            {
                DisplayAlertGPS("DogCare Require location", "Please turn on location", "ok");
            }
            else
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 5;
                var position = await locator.GetPositionAsync(10000);
                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMiles(0.05)));
                map.RouteCoordinates.Add(new Position(position.Latitude, position.Longitude));
                System.Diagnostics.Debug.WriteLine("111111111111111111111111111111111111111111111111111                          " + map.RouteCoordinates[0]);
                await locator.StartListeningAsync(100,0.1);
                locator.PositionChanged += Current_PositionChanged;
            }
        }


        private void Current_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var newPosition = e.Position;
                var list = new List<Position>();
                list = map.RouteCoordinates;
                list.Add(new Position(newPosition.Latitude, newPosition.Longitude));
                map.RouteCoordinates = list;
                System.Diagnostics.Debug.WriteLine("22222222222222222222222222222222222222222222222                          " + map.RouteCoordinates[1]);
            });
        }




        public void DisplayAlertGPS(string message, string title, string button1)
        {
            Device.BeginInvokeOnMainThread(() => {
                DisplayAlert(message, title, button1);
            });
        }

        private void ButtonPoopClicked(object sender, EventArgs e)
        {

        }

        private void ButtonPeeClicked(object sender, EventArgs e)
        {

        }

        private void ButtonFinishClicked(object sender, EventArgs e)
        {

        }
    }
}