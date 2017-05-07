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
            showMap(trip.RouteCoordinates, trip.PoopCoordinates, trip.PeeCoordinates);
        }

        public void showMap(string routCoordinates, string poopCoordinates, string peeCoordinates)
        {
            List<Position> rout = Utils.Utils.convertPositionsStringToList(routCoordinates);
            List<Position> poopPositions = Utils.Utils.convertPositionsStringToList(poopCoordinates);
            List<Position> peePositions = Utils.Utils.convertPositionsStringToList(peeCoordinates);
            Position start = rout[0];
            Position finish = rout[rout.Count - 1];
            if (rout != null)
            {
                map.RouteCoordinates = rout;
                map.MoveToRegion(MapSpan.FromCenterAndRadius(rout[0], Distance.FromMiles(0.05)));
            }
            if (poopPositions != null)
                map.PinsPoop = poopPositions;
            if (peePositions != null)
                map.PinsPee = peePositions;
            if (start != null)
                map.FlagStartCoordinate = start;
            if (finish != null)
                map.FlagFinishCoordinate = finish;
        }
    }
}
