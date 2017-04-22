using DogCare.Managers;
using DogCare.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DogCare.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyWalksPage : ContentPage
    {
        TripsManager tripsManager;

        public MyWalksPage()
        {
            InitializeComponent();
            tripsManager = TripsManager.DefaultManager;
            GetTripsList(App.currentOwner.UserName, App.currentDog.DogName);
        }

        async private void GetTripsList (string owner, string dogName)
        {
            List<Trips> listOfTrips = await tripsManager.GetTripsByDogAndOwner(owner, dogName);
            if (listOfTrips != null)
            {
                //grouping the list according to date
                var groupedData =
                        listOfTrips.OrderBy(trip => trip.Date)
                            .GroupBy(trip => trip.Date)
                            .Select(trip => new ObservableGroupCollection<string, Trips>(trip))
                            .ToList();

                BindingContext = new ObservableCollection<ObservableGroupCollection<string, Trips>>(groupedData);
            }
            else
            {
                emptyList.IsVisible = true;
            }
        }
    }
}
