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
using System.Diagnostics;
using DogCare.Models;
using DogCare.Managers;

namespace DogCare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TripMap : ContentPage
    {
        Plugin.Geolocator.Abstractions.IGeolocator locator;
        Distance distanceFromMapInMiles;
        int locatorDesiredAccuracy;
        bool isListening;
        bool hasGPS;
        bool isPoopClicked;
        bool isPeeClicked;
        DateTime currentDateTime;
        DogManager dogManager;
        TripsManager tripsManager;

        public TripMap()
        {
            // Consts
            locatorDesiredAccuracy = 5;
            distanceFromMapInMiles = Distance.FromMiles(0.05);
            double speedThresholdKMH = 8;

            InitializeComponent();
            dogManager = DogManager.DefaultManager;
            tripsManager = TripsManager.DefaultManager;
            InitMap(speedThresholdKMH);
        }

        async private void InitMap(double speedThresholdKMH)
        {
            map.speedThresholdKMH = speedThresholdKMH;
            isListening = false;
            hasGPS = false;
            isPeeClicked = false;
            isPoopClicked = false;
            locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = locatorDesiredAccuracy;
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(31.771959, 34.87018), Distance.FromMiles(50)));

            var nPosition = await map.GetCurrentPosition(locator);
            if (nPosition == null)
            {
                DisplayAlertGPS();
            }
            else
            {
                var position = (Position)nPosition;
                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), distanceFromMapInMiles));
            }

            //dealing with GPS that was turned off and then start working 
            TimeSpan t = new TimeSpan(0, 0, 1);
            Device.StartTimer(t, () => {
                if (locator.IsGeolocationEnabled == false)
                {
                    hasGPS = false;
                }
                else
                {
                    if (hasGPS == false && isListening == true)
                    {
                        Device.BeginInvokeOnMainThread(() => {
                            DisplayAlert("GPS", "Retriving GPS location", "OK");
                        });
                        locator.StartListeningAsync(100, 0.1);
                        locator.PositionChanged += Current_PositionChanged;
                    }
                    hasGPS = true;
                }
                return true;
            });
        }

        private void Current_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var nPosition = await map.GetCurrentPosition(locator);
                if (nPosition == null)
                {
                    DisplayAlertGPS();
                }
                else
                {
                    var newPosition = (Position)nPosition;
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(newPosition.Latitude, newPosition.Longitude), distanceFromMapInMiles));
                    var list = new List<Position>(map.RouteCoordinates);
                    list.Add(new Position(newPosition.Latitude, newPosition.Longitude));
                    map.RouteCoordinates = list;
                    distance.Text = System.Convert.ToString((int)map.totalDistance + " m");
                }                
            });
        }

        public void DisplayAlertGPS()
        {
            string title = "DogCare Require location";
            string button1 = "ok";
            string message = "Please turn on location";
            Device.BeginInvokeOnMainThread(() => {
                DisplayAlert(message, title, button1);
            });
        }
        
        /* ------------------------------------------------------ */
        // --------------------- Buttoms ------------------------ */
        /* ------------------------------------------------------ */

        async private void ButtonStartClicked(object sender, EventArgs e)
        {
            startButton.IsEnabled = false;
            var nPosition = await map.GetCurrentPosition(locator);
            if (nPosition == null)
            {
                DisplayAlertGPS();
                startButton.IsEnabled = true;
            }
            else
            {
                poopButton.IsEnabled = true;
                peeButton.IsEnabled = true;
                FinishButton.IsEnabled = true;

                var position = (Position)nPosition;
                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), distanceFromMapInMiles));
                map.RouteCoordinates.Add(new Position(position.Latitude, position.Longitude));
                await locator.StartListeningAsync(100, 0.1);
                locator.PositionChanged += Current_PositionChanged;
                hasGPS = true;
                isListening = true;
                currentDateTime = DateTime.Now;
            }
        }

        async private void ButtonPoopClicked(object sender, EventArgs e)
        {
            var nPosition = await map.GetCurrentPosition(locator);
            if (nPosition == null)
            {
                DisplayAlertGPS();
            }
            else
            {
                var position = (Position)nPosition;
                map.AddPoop(position);
                isPoopClicked = true;
            }
        }

        async private void ButtonPeeClicked(object sender, EventArgs e)
        {
            var nPosition = await map.GetCurrentPosition(locator);

            if (nPosition == null)
            {
                DisplayAlertGPS();
            }
            else
            {
                var position = (Position)nPosition;
                map.AddPee(position);
                isPeeClicked = true;
            }
        }

        async private void ButtonFinishClicked(object sender, EventArgs e)
        {
            await locator.StopListeningAsync();
            isListening = false;
            poopButton.IsEnabled = false;
            peeButton.IsEnabled = false;
            FinishButton.IsEnabled = false;
            AddDistanceToDogTatalWalk();
            UpdateMyTrips();
        }

       async private void AddDistanceToDogTatalWalk()
        {
           App.currentDog.Walk += (int)map.totalDistance;
           await dogManager.SaveTaskAsync(App.currentDog);
        }

        async private void UpdateMyTrips()
        {
            var trip = new Trips
            {
                Date = this.currentDateTime.ToString("MM/dd/yyyy"),
                Time = this.currentDateTime.ToString("HH:mm"),
                Distance = (int)map.totalDistance,
                DogName = App.currentDog.DogName,
                Owner = App.currentOwner.UserName,
                Pee = isPeeClicked,
                Poop = isPoopClicked,
                RouteCoordinates = Utils.Utils.ConvertPositionsListToString(map.RouteCoordinates)
            };
           
            await tripsManager.SaveTaskAsync(trip);
        }
    }
}