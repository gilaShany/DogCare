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
    public partial class MyDogsPage : ContentPage
    {
        DogManager manager;
        async void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
           
            var dog = e.SelectedItem as Dog;

            bool answer = await DisplayAlert("", string.Format("{0} is selected", dog.DogName),"OK","Cancel");
            if (answer)
            {
                MasterDetailSideMenucs.MasterDetailPage = new MasterDetailPage
                {
                    Master = new MenuPage(),
                    Detail = new NavigationPage(new MainPage())

                };
                App.currentDog = dog;
                await Navigation.PushModalAsync(MasterDetailSideMenucs.MasterDetailPage);

            }
            listView.SelectedItem = null;
        }
        
        public MyDogsPage()
        {
            
            InitializeComponent();
            manager = DogManager.DefaultManager;
            GetDogsByOwner(App.typedUserName);
        }
        public async void GetDogsByOwner(string owner)
        {
            List<Dog> listOfDogs = await manager.CheckOwnerDogs(owner);
            if (listOfDogs != null)
            {
                listView.ItemsSource = new List<Dog>();
                listView.ItemsSource = await manager.CheckOwnerDogs(owner);

            }
        }
    }
}
