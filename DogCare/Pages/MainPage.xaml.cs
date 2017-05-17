using DogCare.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DogCare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            if (App.currentDog.ImageD != null)
            {
                image.Source = ImageSource.FromStream(() => Utils.ImageStream.ConvertStringToStream(App.currentDog.ImageD));
            }
            else
            {
                image.Source = ImageSource.FromFile("Dog.png");
            }

        }

        async private void Calendar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Schedule());
        }


        async private void Walk_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TripMap());
        }

        async private void MyWalks_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyWalksPage());
        }

        async private void Competition_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CompetitionPage());
        }
    }
}
