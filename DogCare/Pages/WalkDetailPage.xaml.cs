using DogCare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace DogCare.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WalkDetailPage : ContentPage
    {
        public WalkDetailPage(Trips trip)
        {
            if (trip == null)
                throw new ArgumentNullException();

            InitializeComponent();
            showMap(trip.RouteCoordinates);
        }

        public void showMap(string routCoordinates)
        {
            List<Position> rout = Utils.Utils.convertPositionsStringToList(routCoordinates);
            map.RouteCoordinates = rout;
            map.MoveToRegion(MapSpan.FromCenterAndRadius(rout[0], Distance.FromMiles(0.05)));
        }
    }
}
