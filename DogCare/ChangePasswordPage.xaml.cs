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
    public partial class ChangePasswordPage : ContentPage
    {
        OwnerManager manager;
        public ChangePasswordPage()
        {
            InitializeComponent();
            manager = OwnerManager.DefaultManager;
        }

        async private void ChangePassword_Clicked(object sender, EventArgs e)
        {
            Owner owner = await manager.CheckIfOwnerAlreadyExists(App.typedUserName);

            bool wrongOldPassword = false;
            bool noMatch = false;
            if(App.typedPassword != old.Text)
            {
                await DisplayAlert("Opps!", "Wrong old password", "OK");
                wrongOldPassword = true;
                old.Text = string.Empty;
                old.Unfocus();
            }
            if(newP.Text != confirm.Text)
            {
                await DisplayAlert("Opps!", "No match between the passwords", "OK");
                noMatch = true;
                newP.Text = string.Empty;
                newP.Unfocus();
                wrongOldPassword = true;
                confirm.Text = string.Empty;
                confirm.Unfocus();
            }
            if(!noMatch && !wrongOldPassword)
            {
                owner.Password = newP.Text;
                App.typedPassword = newP.Text;
                await manager.SaveTaskAsync(owner);
                await DisplayAlert("", owner.Password, "Ok");

                await DisplayAlert("", "Your password updated succefully", "Ok");
            }
            wrongOldPassword = false;
            noMatch = false;
        }
    }
}
