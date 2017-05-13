using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public static OwnerManager manager;
        public static DogManager dManager;
        public Login()
        {
            manager = OwnerManager.DefaultManager;
            dManager = DogManager.DefaultManager;

            InitializeComponent();

        }

        protected override async void OnAppearing()
        {
            if (SqliteConnectionSet._user.Count != 0)
            {
                userName.Text = SqliteConnectionSet._user[0].UserName;
                password.Text = SqliteConnectionSet._user[0].Password;
                App.currentOwner = SqliteConnectionSet._user[0];
            }
            base.OnAppearing();
        }
        public async static Task click()
        {
            var method = await (manager.CheckUserNameAndPassword(SqliteConnectionSet._user[0].UserName, SqliteConnectionSet._user[0].Password));
            App.currentOwner = method;
        }

        public async void Button_Clicked(object sender, EventArgs e)
        {
            if ((CrossConnectivity.Current.IsConnected == false))
            {
                await DisplayAlert(Constants.internetAlertTittle, Constants.internetAlertMessage, Constants.internetButton);
            }
            else
            {
                if (userName.Text == null || password.Text == null)
                {
                    await DisplayAlert("Oops!", "Please enter Username & password", "OK");
                }
                else
                {
                    activity.IsVisible = true;
                    activity.IsRunning = true;
                    var method = await (manager.CheckUserNameAndPassword(userName.Text, password.Text));

                    if (method == null)
                    {
                        await DisplayAlert("Oops!", "The username or password are not valid", "OK");
                        activity.IsVisible = false;
                        activity.IsRunning = false;
                        userName.Text = string.Empty;
                        userName.Unfocus();
                        password.Text = string.Empty;
                        password.Unfocus();
                    }
                    else
                    {

                        App.currentOwner = method;
                        activity.IsVisible = true;
                        activity.IsRunning = true;
                        List<Dog> dogsList = await dManager.CheckOwnerDogs(App.currentOwner.OwnerName);
                        if (SqliteConnectionSet._user.Count == 0)
                        {
                            await SqliteConnectionSet._connection.InsertAsync(method);

                            SqliteConnectionSet._user.Add(method);
                        }

                        if (dogsList != null)
                        {
                            if (dogsList.Count > 1)
                                await Navigation.PushAsync(new DogMiddlePage());
                            else
                            {
                                MasterDetailSideMenucs.CreateMasterPage();
                                await Navigation.PushModalAsync(MasterDetailSideMenucs.MasterDetailPage);
                            }
                        }
                        else
                        {
                            await Navigation.PushAsync(new DogMiddlePage());
                        }

                    }
                }
            }
        }
    }
}
