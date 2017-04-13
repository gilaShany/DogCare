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

        public object LabelGeolocation { get; private set; }

        public TripMap()
        {
            InitializeComponent();
            map.RouteCoordinates.Add(new Position(37.797534, -122.401827));
            map.RouteCoordinates.Add(new Position(37.797510, -122.402060));
            map.RouteCoordinates.Add(new Position(37.790269, -122.400589));
            map.RouteCoordinates.Add(new Position(37.790265, -122.400474));
            map.RouteCoordinates.Add(new Position(37.790228, -122.400391));
            map.RouteCoordinates.Add(new Position(37.790126, -122.400360));
            map.RouteCoordinates.Add(new Position(37.789250, -122.401451));
            map.RouteCoordinates.Add(new Position(37.788440, -122.400396));
            map.RouteCoordinates.Add(new Position(37.787999, -122.399780));
            map.RouteCoordinates.Add(new Position(37.786736, -122.398202));
            map.RouteCoordinates.Add(new Position(37.786345, -122.397722));
            map.RouteCoordinates.Add(new Position(37.785983, -122.397295));
            map.RouteCoordinates.Add(new Position(37.785559, -122.396728));
            map.RouteCoordinates.Add(new Position(37.780624, -122.390541));
            map.RouteCoordinates.Add(new Position(37.777113, -122.394983));
            map.RouteCoordinates.Add(new Position(37.776831, -122.394627));

            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(37.79752, -122.40183), Distance.FromMiles(0.2)));
            //currentLocation();

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

        private void ButtonStartClicked(object sender, EventArgs e)
        {
            currentLocation();
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