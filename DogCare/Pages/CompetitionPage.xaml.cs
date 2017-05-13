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
    public partial class CompetitionPage : ContentPage
    {
        DogManager dogManager;
        
        public CompetitionPage()
        {
            InitializeComponent();
            dogManager = DogManager.DefaultManager;
            ShowCompetitionList();
            UpdateYourPositionInCompetition();
        }

        async private void ShowCompetitionList()
        {
            if ((CrossConnectivity.Current.IsConnected == false))
            {
                var alertResult = await DisplayAlert(Constants.internetAlertTittle, Constants.internetAlertMessage, null,Constants.internetButton);
                if (!alertResult)
                {
                    MasterDetailSideMenucs.CreateMasterPage();
                    await Navigation.PushModalAsync(MasterDetailSideMenucs.MasterDetailPage);
                }
            }
            else
            {
                List<Dog> dogsList = await dogManager.GetTopThreeDogsByTotalWalk();
                List<Competition> topThreeList = new List<Competition>();
                int counter = 1;
                foreach (Dog dog in dogsList)
                {
                    Competition c = new Competition();
                    c.Dog = dog;
                    Image medal = new Image();

                    if (counter == 1)
                    {
                        medal.Source = ImageSource.FromFile("firstPlace.png");
                    }
                    else if (counter == 2)
                    {
                        medal.Source = ImageSource.FromFile("secondPlace.png");
                    }
                    else
                    {
                        medal.Source = ImageSource.FromFile("thirdPlace.png");
                    }
                    c.Index = medal;
                    counter++;

                    Image image = new Image();

                    if (dog.ImageD != null)
                    {
                        image.Source = ImageSource.FromStream(() => Utils.ImageStream.ConvertStringToStream(dog.ImageD));
                    }
                    else
                    {
                        image.Source = ImageSource.FromFile("Dog.png");
                    }
                    c.DogImage = image;

                    topThreeList.Add(c);
                }
                BindingContext = topThreeList;
            }
        }

        private void ListView_Refreshing(object sender, EventArgs e)
        {
            ShowCompetitionList();
            UpdateYourPositionInCompetition();
            listView.EndRefresh();
        }

        async private void UpdateYourPositionInCompetition()
        {
            if ((CrossConnectivity.Current.IsConnected == true))
            {
                List<Dog> allDogs = new List<Dog>(await dogManager.GetDogItemsAsync());
                allDogs = allDogs.OrderByDescending(dog => dog.Walk).ToList();
                var position = allDogs.FindIndex(d => d.DogName == App.currentDog.DogName && d.Owner == App.currentOwner.UserName);
                placeInCompetition.Text = (position + 1).ToString();
                numOfParticipants.Text = allDogs.Count.ToString();
                myTotalDistance.Text = allDogs[position].Walk.ToString();

                indicator.IsVisible = false;
                indicator.IsRunning = false;
                title.IsVisible = true;
                listView.IsVisible = true;
                yourPosition.IsVisible = true;
            }
        }

        private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            listView.SelectedItem = null;
        }
    }
}
