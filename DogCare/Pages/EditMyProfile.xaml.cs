using DogCare.Models;
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

        public EditMyProfile()
        {
            manager = OwnerManager.DefaultManager;
            InitializeComponent();
            BindingContext = App.currentOwner;
            memStream = null;

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
                }
            };
        }
        async private void Edit_Clicked(object sender, EventArgs e)
        {
                var owner = new Owner
                {
                    OwnerName = ownerName.Text,
                    UserName = App.currentOwner.UserName,
                    Password = App.currentOwner.Password,
                    ImageO = Utils.ImageStream.ConvertStreamToString(memStream)
                };

            manager.Delete(App.currentOwner);
            await manager.SaveTaskAsync(owner);
            App.currentOwner = owner;
            await DisplayAlert("", "Your profile updated succefully", "Ok");
        }

        async private void ChangePassword_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ChangePasswordPage());
        }
    }
}
