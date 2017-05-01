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
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
            Title = "Master";
            BackgroundColor = Color.Gray.WithLuminosity(0.9);
            Icon = Device.OS == TargetPlatform.Android ? "menu.png" : null;
        }

        async private void MyDogs_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new MyDogsPage()));
        }

        async private void EditDog_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new EditMyDogProfile()));
        }

        async private void EditProfile_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new EditMyProfile()));
        }

        async private void AddNewDog_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new DogList()));
        }

        async private void logout_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new logoutPage());

        }

        async private void ChangePassword_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ChangePasswordPage());

        }
    }
}
