using System;
using System.Collections.Generic;
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
            GetCompetitionList();
        }

        async private void GetCompetitionList()
        {
            List<Dog> dogsList = await dogManager.GetTopThreeDogsByTotalWalk();
            BindingContext = dogsList;
        }

        private void ListView_Refreshing(object sender, EventArgs e)
        {
            GetCompetitionList();
            listView.EndRefresh();
        }
    }
}
