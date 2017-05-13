using DogCare.Managers;
using DogCare.Models;
using Plugin.Connectivity;
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

        async private void GetTripsList(string owner, string dogName, string searchText = null)
        {
            if ((CrossConnectivity.Current.IsConnected == false))
            {
                var alertResult = await DisplayAlert(Constants.internetAlertTittle, Constants.internetAlertMessage, null, Constants.internetButton);
                if (!alertResult)
                {
                    MasterDetailSideMenucs.CreateMasterPage();
                    await Navigation.PushModalAsync(MasterDetailSideMenucs.MasterDetailPage);
                }
            }
            else
            {
                List<Trips> listOfTrips = await tripsManager.GetTripsByDogAndOwner(owner, dogName);
                if (listOfTrips != null)
                {
                    searchBar.IsVisible = true;
                    //grouping the list according to date
                    var groupedData =
                            listOfTrips.OrderByDescending(trip => trip.Date)
                                .GroupBy(trip => trip.Date)
                                .Select(trip => new ObservableGroupCollection<string, Trips>(trip))
                                .ToList();

                    if (String.IsNullOrWhiteSpace(searchText))
                        BindingContext = new ObservableCollection<ObservableGroupCollection<string, Trips>>(groupedData);
                    else
                        BindingContext = groupedData.Where(c => c.Key.StartsWith(searchText));

                    indicator.IsVisible = false;
                    indicator.IsRunning = false;
                }
                else
                {
                    indicator.IsVisible = false;
                    indicator.IsRunning = false;
                    var alertResult = await DisplayAlert("Oops!", "There are no recorded walks.", null, "Ok");
                    if (!alertResult)
                    {
                        MasterDetailSideMenucs.CreateMasterPage();
                        await Navigation.PushModalAsync(MasterDetailSideMenucs.MasterDetailPage);
                    }
                }
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetTripsList(App.currentOwner.UserName, App.currentDog.DogName,e.NewTextValue);
        }

        async private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            
            var trip = e.SelectedItem as Trips;
            await Navigation.PushAsync(new WalkDetailPage(trip));
            listView.SelectedItem = null;
        }
    }
}
