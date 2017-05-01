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
    public partial class EditMyProfile : ContentPage
    {
        OwnerManager manager;
        public EditMyProfile()
        {
            manager = OwnerManager.DefaultManager;
            InitializeComponent();
            BindingContext = App.currentOwner;
            if (App.currentOwner.ImageO != " ")
                image.Source = ImageSource.FromStream(() =>  Utils.ImageStream.ConvertStringToStream(App.currentOwner.ImageO));
            else
                image.Source = ImageSource.FromFile("User.png");

        }
        async private void Edit_Clicked(object sender, EventArgs e)
        {
                var owner = new Owner
                {
                    OwnerName = ownerName.Text,
                    UserName = App.currentOwner.UserName,
                    Password = App.currentOwner.Password
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
