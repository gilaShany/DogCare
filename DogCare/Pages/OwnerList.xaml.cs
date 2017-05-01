using DogCare.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
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
        MemoryStream memStream;

        public OwnerList()
        {
            manager = OwnerManager.DefaultManager;
            dManager = DogManager.DefaultManager;
            memStream = null;
            InitializeComponent();


            pickPhoto.Clicked += async (sender, e) =>
            {
                var stream = await DependencyService.Get<IPicturePicker>().GetImageStreamAsync();
                memStream = Utils.ImageStream.ConvertStreamToMemoryStream(stream);
                if (memStream != null)
                {
                    image.Source = ImageSource.FromStream(() => { return new MemoryStream(memStream.ToArray()); });
                    image.HeightRequest = 200;
                    image.WidthRequest = 200;
                }

            };

        }
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
                            Password = password.Text,
                            ImageO = Utils.ImageStream.ConvertStreamToString(memStream)
                        };
                        memStream.Dispose();
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
