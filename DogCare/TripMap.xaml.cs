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
        Plugin.Geolocator.Abstractions.IGeolocator locator;

        public TripMap()
        {
            InitializeComponent();
   
            currentLocation();

        }


        async void currentLocation()
        {           
            if (CrossGeolocator.Current.IsGeolocationEnabled == true)
            {
                locator = CrossGeolocator.Current;
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
                locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 5;
                var position = await locator.GetPositionAsync(10000);
                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMiles(0.05)));
                map.RouteCoordinates.Add(new Position(position.Latitude, position.Longitude));
                await locator.StartListeningAsync(100,0.1);
                locator.PositionChanged += Current_PositionChanged;
            }
        }


        private void Current_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var newPosition = e.Position;
                var list = new List<Position>(map.RouteCoordinates);
                list.Add(new Position(newPosition.Latitude, newPosition.Longitude));
                map.RouteCoordinates = list;
            });
        }




        public void DisplayAlertGPS(string message, string title, string button1)
        {
            Device.BeginInvokeOnMainThread(() => {
                DisplayAlert(message, title, button1);
            });
        }

        async private void ButtonPoopClicked(object sender, EventArgs e)
        {
            var poopPosition = CrossGeolocator.Current;
            poopPosition.DesiredAccuracy = 5;
            var position = await poopPosition.GetPositionAsync(10000);
            var list = new List<Pin>(map.PinsPoop);
            var pin = new Pin();
            pin.Position = new Position(position.Latitude, position.Longitude);
            list.Add(pin);
            map.PinsPoop = list;
        }

        async private void ButtonPeeClicked(object sender, EventArgs e)
        {
            var peePosition = CrossGeolocator.Current;
            peePosition.DesiredAccuracy = 5;
            var position = await peePosition.GetPositionAsync(10000);
            var list = new List<Pin>(map.PinsPee);
            var pin = new Pin();
            pin.Position = new Position(position.Latitude, position.Longitude);
            list.Add(pin);
            map.PinsPee = list;
        }

        async private void ButtonFinishClicked(object sender, EventArgs e)
        {
            await locator.StopListeningAsync();
        }
    }
}