﻿using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace DogCare
{
    public class CustomMap : Map
    {
        public static readonly BindableProperty PinsPoopProperty =
        BindableProperty.Create<CustomMap, List<Pin>>(p => p.PinsPoop, new List<Pin>());

        public List<Position> lastPositions { get; set; }
        public Position? currentPosition{ get; set; }
        private int currentTime;

        public List<Pin> PinsPoop
        {
            get { return (List<Pin>)GetValue(PinsPoopProperty); }
            set { SetValue(PinsPoopProperty, value); }
        }


        public static readonly BindableProperty PinsPeeProperty =
        BindableProperty.Create<CustomMap, List<Pin>>(p => p.PinsPee, new List<Pin>());

        public List<Pin> PinsPee
        {
            get { return (List<Pin>)GetValue(PinsPeeProperty); }
            set { SetValue(PinsPeeProperty, value); }
        }

        public static readonly BindableProperty RouteCoordinatesProperty =
        BindableProperty.Create<CustomMap, List<Position>>(p => p.RouteCoordinates, new List<Position>());
       

        public List<Position> RouteCoordinates
        {
            get { return (List<Position>)GetValue(RouteCoordinatesProperty); }
            set { SetValue(RouteCoordinatesProperty, value); }
        }

        public CustomMap()
        {
            RouteCoordinates = new List<Position>();
            PinsPee = new List<Pin>();
            PinsPoop = new List<Pin>();
        }

        public void AddPoop(Position position)
        {
            this.PinsPoop = UpdatePinList(position, this.PinsPoop);
        }

        public void AddPee(Position position)
        {
            this.PinsPee = UpdatePinList(position, this.PinsPee);
        }

        private List<Pin> UpdatePinList(Position newPosition, List<Pin> currentPins)
        {
            var list = new List<Pin>(currentPins);
            var pin = new Pin();
            pin.Position = new Position(newPosition.Latitude, newPosition.Longitude);
            list.Add(pin);
            return list;
        }

        async public Task<Nullable<Position>> GetCurrentLocation(Plugin.Geolocator.Abstractions.IGeolocator locator)
        {
            // going to be parameters
            //  int n_last_locations = 3;
            //int angle_threshold = 90;

            // Maximum speed of trip
            int speedThresholdKMH = 6;
            try
            {
                /* Getting current location */
                //var currentSampledPosition = await GetMedeanPosition(locator, nSamples);
                var geoPosition = await locator.GetPositionAsync(10000);
                Position currentSampledPosition = new Position(geoPosition.Latitude, geoPosition.Longitude);

                this.UpdateCurrentPosition(currentSampledPosition, Utils.Utils.KPHtoMPS(speedThresholdKMH));
                return this.currentPosition;
            }
            catch
            {
                return null;
            }
        }
        private void UpdateCurrentPosition(Position currentSampledPosition, double speedThresholdInMeters)
        {
            int current_epoch_time = (int)(DateTime.UtcNow - (new DateTime(1970, 1, 1))).TotalSeconds;
            Debug.WriteLine("------------------------------------------------------- " + current_epoch_time);
            Debug.WriteLine("-------------------------------------------------------(" + currentSampledPosition.Longitude + ", " + currentSampledPosition.Latitude + ")");
            /* Camparing to last Locations */
            if (this.currentPosition == null)
            {
                this.currentTime = current_epoch_time;
                this.currentPosition = (Position?)(currentSampledPosition);
                this.lastPositions = new List<Position>();
            }
            else
            {
                double distance = Utils.Utils.GetDistanceInMeters(this.lastPositions.Last(), currentSampledPosition);
                int time_past = current_epoch_time - this.currentTime;
                Debug.WriteLine("DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD " + distance);
                Debug.WriteLine("TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT " + time_past);
                if (distance / time_past < speedThresholdInMeters)
                {
                    // this location cannot happen
                    this.currentPosition = (Position?)(currentSampledPosition);
                }
                /*if (this.lastPositions.Count + 1 >= n_last_locations)
                {
                    // Looking at the last 3 locations -TODO change to N locations

                    var last = current_sampled_position;
                    var last2 = this.lastPositions[this.lastPositions.Count - 1];
                    var last3 = this.lastPositions[this.lastPositions.Count - 2];
                    var angle = Utils.Utils.GetAngle(last, last2, last3);
                    Debug.WriteLine("Angle: " + angle);
                    if (angle > angle_threshold)
                    {
                        this.currentPosition = (Position?)(current_sampled_position);
                    }
                }*/

                this.currentTime = current_epoch_time;
            }

            this.lastPositions.Add(currentSampledPosition);
        }
        

    }
}
