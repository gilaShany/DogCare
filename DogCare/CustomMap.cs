using Plugin.Geolocator;
using System;
using System.Collections.Generic;
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

        public void AddPoop (Position position)
        {
            this.PinsPoop = UpdatePinList(position, this.PinsPoop);
        }

        public void AddPee (Position position)
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
            if (locator.IsGeolocationEnabled == false)
            {
                return null;
            }
            else
            {
                var position = await locator.GetPositionAsync(10000);
                return new Position(position.Latitude, position.Longitude);     
            }
        }

    }
}
