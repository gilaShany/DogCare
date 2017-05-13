using DogCare.Models;
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
    public partial class MyDogsPage : ContentPage
    {
        DogManager manager;
        async void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
           
            var dog = e.SelectedItem as DogAndImage;

            bool answer = await DisplayAlert("", string.Format("{0} is selected", dog.Dog.DogName),"OK","Cancel");
            if (answer)
            {
                MasterDetailSideMenucs.CreateMasterPage();
                App.currentDog = dog.Dog;
                await Navigation.PushModalAsync(MasterDetailSideMenucs.MasterDetailPage);
            }
            listView.SelectedItem = null;
        }
        
        public MyDogsPage()
        {
            InitializeComponent();
            manager = DogManager.DefaultManager;
            GetDogsByOwner(App.currentOwner.UserName);
        }
        public async void GetDogsByOwner(string owner)
        {
            if ((CrossConnectivity.Current.IsConnected == false))
            {
                var alertResult = await DisplayAlert(Constants.internetAlertTittle, Constants.internetAlertMessage, null, Constants.internetButton);
                if (!alertResult)
                {
                    MasterDetailSideMenucs.CreateMasterPage();
                    await Navigation.PushModalAsync(MasterDetailSideMenucs.MasterDetailPage);
                }
            }
            else
            {
                List<Dog> listOfDogs = await manager.CheckOwnerDogs(owner);
                List<DogAndImage> listOfDogsWithImage = new List<DogAndImage>();

                if (listOfDogs != null)
                {
                    foreach (Dog dog in listOfDogs)
                    {
                        DogAndImage d = new DogAndImage();
                        d.Dog = dog;
                        Image image = new Image();

                        if (dog.ImageD != null)
                        {
                            image.Source = ImageSource.FromStream(() => Utils.ImageStream.ConvertStringToStream(dog.ImageD));
                        }
                        else
                        {
                            image.Source = ImageSource.FromFile("Dog.png");
                        }
                        d.DogImage = image;

                        listOfDogsWithImage.Add(d);
                    }
                    BindingContext = listOfDogsWithImage;
                }
            }
        }
    }
}
