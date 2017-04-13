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
        }

    }
}
