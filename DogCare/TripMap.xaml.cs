using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace DogCare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TripMap : ContentPage
    {
        public TripMap()
        {
            InitializeComponent();

            var map = new Map(MapSpan.FromCenterAndRadius(new Position(37, -122), Distance.FromMiles(10)));

            var pin = new Pin()
            {
                Position = new Position(37, -122),
                Label = "Some Pin!"
            };
            map.Pins.Add(pin);


            Content = map;

        }
    }
}
