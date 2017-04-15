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
   
            InitLocation();

        }


        async void InitLocation()
        {
            locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 5;
            var nPosition = await map.GetCurrentLocation(locator);
            if (nPosition == null)
            {
                DisplayAlertGPS("DogCare Require location", "Please turn on location", "ok");
            }
            else
            {
                var position = (Position)nPosition;
                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMiles(0.05)));
            }
        }

        async private void ButtonStartClicked(object sender, EventArgs e)
        {
            var nPosition = await map.GetCurrentLocation(locator);
            if (nPosition == null)
            {
                DisplayAlertGPS("DogCare Require location", "Please turn on location", "ok");
            }
            else
            {
                var position = (Position)nPosition;
                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMiles(0.05)));
                map.RouteCoordinates.Add(new Position(position.Latitude, position.Longitude));
                await locator.StartListeningAsync(100,0.1);
                locator.PositionChanged += Current_PositionChanged;
            }
        }


        private void Current_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var nPosition = await map.GetCurrentLocation(locator);

                if (nPosition == null)
                {
                    int x = 1;
                    //DisplayAlertGPS("DogCare Require location", "Please turn on location", "ok");
                }
                else
                {
                    var newPosition = (Position)nPosition;
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(newPosition.Latitude, newPosition.Longitude), Distance.FromMiles(0.05)));
                    var list = new List<Position>(map.RouteCoordinates);
                    list.Add(new Position(newPosition.Latitude, newPosition.Longitude));
                    map.RouteCoordinates = list;
                }                
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
            var nPosition = await map.GetCurrentLocation(locator);
            if (nPosition == null)
            {
                DisplayAlertGPS("DogCare Require location", "Please turn on location", "ok");
            }
            else
            {
                var position = (Position)nPosition;
                map.AddPoop(position);
            }
        }

        async private void ButtonPeeClicked(object sender, EventArgs e)
        {
            var nPosition = await map.GetCurrentLocation(locator);

            if (nPosition == null)
            {
                DisplayAlertGPS("DogCare Require location", "Please turn on location", "ok");
            }
            else
            {
                var position = (Position)nPosition;
                map.AddPee(position);
            }
        }

        async private void ButtonFinishClicked(object sender, EventArgs e)
        {
            await locator.StopListeningAsync();
        }
    }
}