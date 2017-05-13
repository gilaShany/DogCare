using Plugin.Connectivity;
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
            if ((CrossConnectivity.Current.IsConnected == false))
            {
                await DisplayAlert(Constants.internetAlertTittle, Constants.internetAlertMessage, Constants.internetButton);
            }
            else
            {
                bool wrongOldPassword = false;
                bool noMatch = false;
                if (App.currentOwner.Password != old.Text)
                {
                    await DisplayAlert("Oops!", "Wrong old password", "OK");
                    wrongOldPassword = true;
                    old.Text = string.Empty;
                    old.Unfocus();
                }
                if (newP.Text != confirm.Text)
                {
                    await DisplayAlert("Oops!", "No match between the passwords", "OK");
                    noMatch = true;
                    newP.Text = string.Empty;
                    newP.Unfocus();
                    wrongOldPassword = true;
                    confirm.Text = string.Empty;
                    confirm.Unfocus();
                }
                if (!noMatch && !wrongOldPassword)
                {
                    var owner = new Owner
                    {
                        OwnerName = App.currentOwner.OwnerName,
                        UserName = App.currentOwner.UserName,
                        Password = newP.Text
                    };

                    manager.Delete(App.currentOwner);
                    await manager.SaveTaskAsync(owner);
                    App.currentOwner = owner;
                    SqliteConnectionSet._user[0].Password = owner.Password;
                    await SqliteConnectionSet._connection.UpdateAsync(SqliteConnectionSet._user[0]);
                    await DisplayAlert("", "Your password updated succefully", "Ok");
                }
                wrongOldPassword = false;
                noMatch = false;
            }
        }
    }
}
