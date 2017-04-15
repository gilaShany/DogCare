using Plugin.Geolocator;
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
                Debug.WriteLine("-------------------------------------------------------(" + currentSampledPosition.Longitude + ", " + currentSampledPosition.Latitude + ")");
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
            int currentEpochTime = (int)(DateTime.UtcNow - (new DateTime(1970, 1, 1))).TotalSeconds;
            Debug.WriteLine("------------------------------------------------------- " + currentEpochTime);

            /* Camparing to last Locations */
            if (this.currentPosition == null)
            {
                this.currentPosition = (Position?)(currentSampledPosition);
                this.lastPositions = new List<Position>();
            }
            else
            {
                if (CheckPositionSpeed(currentEpochTime, currentSampledPosition, speedThresholdInMeters))
                {
                    // the speed of the dog walker is reasonable
                    // we can update the coordinates accordingly
                    this.currentPosition = (Position?)(currentSampledPosition);
                }
            }
            this.currentTime = currentEpochTime;
            // Updating the list of last positions - the one that were used, and the one that weren't
            this.lastPositions.Add(currentSampledPosition);
        }

        private bool CheckPositionAngle(int nLastLocations, Position currentSampledPosition, double angleThreshold)
        {
            // Checking if the angle of the samples from GPS is high -> going in the same direction.
            // return true if the answer is yes
            if (this.lastPositions.Count + 1 >= nLastLocations)
            {
                // Looking at the last 3 locations -TODO change to N locations
                var last = currentSampledPosition;
                var last2 = this.lastPositions[this.lastPositions.Count - 1];
                var last3 = this.lastPositions[this.lastPositions.Count - 2];
                var angle = Utils.Utils.GetAngle(last, last2, last3);
                Debug.WriteLine("Angle: " + angle);
                if (angle < angleThreshold)
                {
                    // the 3 positions created a little angle. probably sample mistake. 
                    return false;
                }
            }
            return true;
        }

        private bool CheckPositionSpeed(int currentEpochTime, Position currentSampledPosition, double speedThresholdInMeters)
        {
            double distance = Utils.Utils.GetDistanceInMeters(this.lastPositions.Last(), currentSampledPosition);
            int time_past = currentEpochTime - this.currentTime;
            Debug.WriteLine("DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD " + distance);
            Debug.WriteLine("TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT " + time_past);
            if (distance / time_past < speedThresholdInMeters)
                return true;
            else
                return false;
         }
    }
}
