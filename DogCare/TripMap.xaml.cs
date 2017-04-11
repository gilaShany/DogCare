using Plugin.Geolocator;
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
        Map map;

        public TripMap()
        {
            InitializeComponent();

            map = new Map
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            currentLocation();
           


            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);
            Content = stack;

        }


        async void currentLocation()
        {
           if (CrossGeolocator.Current.IsGeolocationEnabled == true)
            {
           
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 5;

                var position = await locator.GetPositionAsync(10000);
                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude),
                                                 Distance.FromMiles(1)));
            }
        }
      
    }
}
