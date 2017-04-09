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
        string typedUserName;
        string typedPassword;
        public OwnerList()
        {
            manager = OwnerManager.DefaultManager;
            InitializeComponent();
        }

        async Task AddItem(Owner item)
        {
            await manager.SaveTaskAsync(item);
        }

        async private void Button_Clicked(object sender, EventArgs e)
        {
            var owner = new Owner
            {
                OwnerName = ownerName.Text,
                UserName = userName.Text,
                Password = password.Text

            };
            await AddItem(owner);

        }

        async private void Button_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DogList());
        }
    }
}
