using DogCare.Models;
using Plugin.Connectivity;
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
    public partial class EditMyProfile : ContentPage
    {
        OwnerManager manager;
        MemoryStream memStream;
        bool hasImageChanged;

        public EditMyProfile()
        { 
            InitializeComponent();
            BindingContext = App.currentOwner;
            manager = OwnerManager.DefaultManager;
            memStream = null;
            hasImageChanged = false;

            //Checking if the user has an image. If not, showing default image.
            if (App.currentOwner.ImageO != null)
                image.Source = ImageSource.FromStream(() =>  Utils.ImageStream.ConvertStringToStream(App.currentOwner.ImageO));
            else
                image.Source = ImageSource.FromFile("User.png");

            pickPhoto.Clicked += async (sender, e) =>
            {
                var stream = await DependencyService.Get<IPicturePicker>().GetImageStreamAsync();
                if (stream != null)
                {
                    memStream = Utils.ImageStream.ConvertStreamToMemoryStream(stream);
                    image.Source = ImageSource.FromStream(() => { return new MemoryStream(memStream.ToArray()); });
                    hasImageChanged = true;
                }
            };
        }
        async private void Edit_Clicked(object sender, EventArgs e)
        {
            if ((CrossConnectivity.Current.IsConnected == false))
            {
                await DisplayAlert(Constants.internetAlertTittle, Constants.internetAlertMessage, null, Constants.internetButton);
            }
            else
            {
                activity.IsVisible = true;
                activity.IsRunning = true;
                string name;
                string imageString;

                if(ownerName.Text != null)
                {
                    name = ownerName.Text;
                }
                else
                {
                    name = App.currentOwner.OwnerName;
                }

                if (hasImageChanged)
                {
                    imageString = Utils.ImageStream.ConvertStreamToString(memStream);
                }
                else
                {
                    imageString = App.currentOwner.ImageO;
                }

                var owner = new Owner
                {
                    OwnerName = name,
                    UserName = App.currentOwner.UserName,
                    Password = App.currentOwner.Password,
                    ImageO = imageString
                };

                manager.Delete(App.currentOwner);
                await manager.SaveTaskAsync(owner);
                App.currentOwner = owner;
                activity.IsVisible = false;
                activity.IsRunning = false;

                bool answer = await DisplayAlert("", "Your profile updated succefully", null,"OK");
                if (!answer)
                {
                    MasterDetailSideMenucs.CreateMasterPage();
                    await Navigation.PushModalAsync(MasterDetailSideMenucs.MasterDetailPage);
                }
            }
        }

    }
}
