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
            List<Dog> dogsList = await dogManager.GetTopThreeDogsByTotalWalk();
            BindingContext = dogsList;
        }

        private void ListView_Refreshing(object sender, EventArgs e)
        {
            ShowCompetitionList();
            UpdateYourPositionInCompetition();
            listView.EndRefresh();
        }

        async private void UpdateYourPositionInCompetition()
        {
            List<Dog> allDogs = new List<Dog>(await dogManager.GetDogItemsAsync());
            allDogs = allDogs.OrderByDescending(dog => dog.Walk).ToList();
            var position = allDogs.FindIndex(d => d.DogName == App.currentDog.DogName && d.Owner == App.currentOwner.UserName);
            placeInCompetition.Text = (position + 1).ToString();
            numOfParticipants.Text = allDogs.Count.ToString();
            myTotalDistance.Text = allDogs[position].Walk.ToString();
        }

    }
}
