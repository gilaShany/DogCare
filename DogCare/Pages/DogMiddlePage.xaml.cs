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
    public partial class DogMiddlePage : ContentPage
    {
        public DogMiddlePage()
        {
            InitializeComponent();
        }

        async private void AddNewDog_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new DogList()));
        }

        async private void Mydogs_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new MyDogsPage()));
        }
    }
}
