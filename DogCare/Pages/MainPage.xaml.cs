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

        }

        async private void Calendar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Schedule());
        }

        async private void Walk_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new TripMap());
        }
    }
}
