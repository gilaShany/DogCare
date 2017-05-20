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
            BindingContext = App.currentDog;
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
            calendar.BackgroundColor = Color.FromHex("#e6e6e6");
            await Navigation.PushAsync(new Schedule());
            calendar.BackgroundColor = Color.Transparent;
        }


        async private void Walk_Clicked(object sender, EventArgs e)
        {
            trip.BackgroundColor = Color.FromHex("#e6e6e6");
            await Navigation.PushAsync(new TripMap());
            trip.BackgroundColor = Color.Transparent;
        }

        async private void MyWalks_Clicked(object sender, EventArgs e)
        {
            map.BackgroundColor = Color.FromHex("#e6e6e6");
            await Navigation.PushAsync(new MyWalksPage());
            map.BackgroundColor = Color.Transparent;
        }

        async private void Competition_Clicked(object sender, EventArgs e)
        {
            trophy.BackgroundColor = Color.FromHex("#e6e6e6");
            await Navigation.PushAsync(new CompetitionPage());
            trophy.BackgroundColor = Color.Transparent;
        }
    }
}
