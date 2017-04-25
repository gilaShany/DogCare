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
        /* --------------------------------------------------------- */
        /* ------------------------ Variables ---------------------- */
        /* --------------------------------------------------------- */

        // Poop + Pee lists. Every time new poop/pee added. OnElementPropertyChanged called and adds pins
        public static readonly BindableProperty PinsPoopProperty =
        BindableProperty.Create<CustomMap, List<Pin>>(p => p.PinsPoop, new List<Pin>());
        public static readonly BindableProperty PinsPeeProperty =
        BindableProperty.Create<CustomMap, List<Pin>>(p => p.PinsPee, new List<Pin>());

        public List<Pin> PinsPoop
        {
            get { return (List<Pin>)GetValue(PinsPoopProperty); }
            set { SetValue(PinsPoopProperty, value); }
        }

        public List<Pin> PinsPee
        {
            get { return (List<Pin>)GetValue(PinsPeeProperty); }
            set { SetValue(PinsPeeProperty, value); }
        }

        // All sampled positions. Including the unvalids.
        public List<Position> lastPositions { get; set; }
        // The last position that is valid
        public Position? currentPosition{ get; set; }
        // The time of the last sample (even if it was unvalid).
        private int currentTime;
        // The maximum expected speed of the dog walker, Used to fix location variance
        // currently set to 8 kilometers per hour
        public double speedThresholdKMH { get; set; }
        //The total distance that the dog walker went in meters. 
        public double totalDistance { get; set; }

        // The coordinates we add to the polyline. Every time it is updated, changing the polyline.
        public static readonly BindableProperty RouteCoordinatesProperty =
        BindableProperty.Create<CustomMap, List<Position>>(p => p.RouteCoordinates, new List<Position>());
        public List<Position> RouteCoordinates
        {
            get { return (List<Position>)GetValue(RouteCoordinatesProperty); }
            set { SetValue(RouteCoordinatesProperty, value); }
        }

        /* --------------------------------------------------------- */
        /* ---------------- Public Functions ----------------------- */
        /* --------------------------------------------------------- */

        public CustomMap()
        {
            this.RouteCoordinates = new List<Position>();
            this.PinsPee = new List<Pin>();
            this.PinsPoop = new List<Pin>();
            this.lastPositions = new List<Position>();
            this.totalDistance = 0;
        }

        public void AddPoop(Position position)
        {
            this.PinsPoop = UpdatePinList(position, this.PinsPoop);
        }

        public void AddPee(Position position)
        {
            this.PinsPee = UpdatePinList(position, this.PinsPee);
        }

        async public Task<Nullable<Position>> GetCurrentPosition(Plugin.Geolocator.Abstractions.IGeolocator locator)
        {
            try
            {
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


        /* --------------------------------------------------------- */
        /* ---------------- Private Functions ---------------------- */
        /* --------------------------------------------------------- */


        private List<Pin> UpdatePinList(Position newPosition, List<Pin> currentPins)
        {
            var list = new List<Pin>(currentPins);
            var pin = new Pin();
            pin.Position = new Position(newPosition.Latitude, newPosition.Longitude);
            list.Add(pin);
            return list;
        }

        private void UpdateCurrentPosition(Position currentSampledPosition, double speedThresholdInMeters)
        {
            int currentEpochTime = (int)(DateTime.UtcNow - (new DateTime(1970, 1, 1))).TotalSeconds;
            if (this.currentPosition == null)
            {
                // since there wasn't any location samples before, 
                // sampled location is valid by default 
                this.currentPosition = (Position?)(currentSampledPosition);
            }
            else
            {
                if (CheckChangePositionSpeed(currentEpochTime, currentSampledPosition, speedThresholdInMeters))
                {
                    // the speed of the dog walker is reasonable. Update current position
                    this.currentPosition = (Position?)(currentSampledPosition);
                }
            }
            this.currentTime = currentEpochTime;
            // Updating the list of last positions sampled
            this.lastPositions.Add(currentSampledPosition);
        }

        private bool CheckChangePositionSpeed(int currentEpochTime, Position currentSampledPosition, double speedThresholdInMeters)
        {
            double distance = Utils.Utils.GetDistanceInMeters(this.lastPositions.Last(), currentSampledPosition);
            int time_past = currentEpochTime - this.currentTime;
            if (distance / time_past < speedThresholdInMeters)
                return true;
            else
                return false;
         }
    }
}
