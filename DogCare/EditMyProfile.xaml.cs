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
            InitializeComponent();
            //BindingContext = App.typedFullName;
            manager = OwnerManager.DefaultManager;
        }
        async private void Edit_Clicked(object sender, EventArgs e)
        {
            Owner owner = await manager.CheckIfOwnerAlreadyExists(App.typedUserName);
            if (ownerName.Text != null)
            {
                owner.OwnerName = ownerName.Text;
                App.typedFullName = ownerName.Text;
            }
            await manager.SaveTaskAsync(owner);
            await DisplayAlert("", "Your profile updated succefully", "Ok");
        }

        async private void ChangePassword_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ChangePasswordPage());
        }
    }
}
