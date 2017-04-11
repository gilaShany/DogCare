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
    public partial class Login : ContentPage
    {
        OwnerManager manager;
        public Login()
        {
            manager = OwnerManager.DefaultManager;
            InitializeComponent();
        }

        async private void Button_Clicked(object sender, EventArgs e)
        {
            if ( userName.Text == null || password.Text == null)
            {
                await DisplayAlert("Opps!", "Please enter Username & password", "OK");
            }
            else
            {
                var method = await (manager.CheckUserNameAndPassword(userName.Text,password.Text));

                if (method == null)
                {
                    await DisplayAlert("Opps!", "The username or password are not valid", "OK");
                }
                else
                {
                    App.typedPassword = userName.Text;
                    App.typedPassword = password.Text;
                    App.typedFullName = method.OwnerName;
                    await Navigation.PushAsync(new DogMiddlePage());

                }
            }
        }
    }
}
