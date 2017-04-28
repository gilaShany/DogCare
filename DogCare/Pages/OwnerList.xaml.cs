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
    public partial class OwnerList : ContentPage
    {
        OwnerManager manager;
        DogManager dManager;

        public OwnerList()
        {
            manager = OwnerManager.DefaultManager;
            dManager = DogManager.DefaultManager;
            InitializeComponent();

        }
        /*
        protected override async void OnAppearing()
        {
            if (SqliteConnectionSet._user.Count != 0)
            {
                ownerName.Text = SqliteConnectionSet._user[0].OwnerName;
                userName.Text = SqliteConnectionSet._user[0].UserName;
                password.Text = SqliteConnectionSet._user[0].Password;
                App.currentOwner = SqliteConnectionSet._user[0];
                //await Navigation.PushAsync(new DogMiddlePage());
            }
            base.OnAppearing();
        }
        */

        async Task AddItem(Owner item)
        {
            await manager.SaveTaskAsync(item);
        }

        async private void CreateNewAccount_Clicked(object sender, EventArgs e)
        {
            if ((ownerName.Text) == null || userName.Text == null || password.Text == null)
            {
                await DisplayAlert("Opps!", "Please enter all details", "OK");
            }
            else
            {
                bool sure = await DisplayAlert("Warning", "Are you sure?", "Yes", "No");
                if (sure)
                {
                    activity.IsVisible = true;
                    activity.IsRunning = true;
                    var method =  await (manager.CheckIfOwnerAlreadyExists(userName.Text));

                    if (method != null)
                    {
                        await DisplayAlert("Opps!", "Username is already taken", "OK");
                        userName.Text = string.Empty;
                        userName.Unfocus();
                        password.Text = string.Empty;
                        password.Unfocus();
                        activity.IsVisible = false;
                        activity.IsRunning = false;
                    }
                    else
                    {

                        activity.IsVisible = true;
                        activity.IsRunning = true;
                        var owner = new Owner
                        {
                            OwnerName = ownerName.Text,
                            UserName = userName.Text,
                            Password = password.Text

                        }; 
                        await AddItem(owner);
                        await SqliteConnectionSet._connection.InsertAsync(owner);
                        SqliteConnectionSet._user.Add(owner);
                        App.currentOwner = owner;

                        bool next =await DisplayAlert("", "Your account added succefully", "OK","Cancel");
                        activity.IsVisible = false;
                        activity.IsRunning = false;
                        if (next)
                        {
                            List<Dog> dogsList = await dManager.CheckOwnerDogs(App.currentOwner.OwnerName);
                            if (dogsList != null)
                            {
                                if (dogsList.Count > 1)
                                    await Navigation.PushAsync(new DogList());
                                else
                                    await Navigation.PushModalAsync(MasterDetailSideMenucs.MasterDetailPage);
                            }
                            else
                            {
                                await Navigation.PushAsync(new DogList());
                            }
                        }
                    }
                }
            }
        }

    }
}
