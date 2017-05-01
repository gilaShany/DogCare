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
    public partial class DogList : ContentPage
    {
        DogManager manager;
        MemoryStream memStream;
        
        public DogList()
        {
            manager = DogManager.DefaultManager;
            InitializeComponent();
            memStream = null;
            image.Source = ImageSource.FromFile("Dog.png");
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

        async Task AddItem(Dog item)
        {
            await manager.SaveTaskAsync(item);
        }

        async private void Button_Clicked(object sender, EventArgs e)
        {
            activity.IsVisible = true;
            activity.IsRunning = true;
            var dog = new Dog
            {
                DogName = newDog.Text,
                Owner = App.currentOwner.UserName,
                Gender = genderS.Text,
                Race = raceS.Text,
                Walk = 0,
                ImageD = Utils.ImageStream.ConvertStreamToString(memStream)
            };
            await AddItem(dog);
            await DisplayAlert("", string.Format("{0} added successfully",dog.DogName), "OK");
            App.currentDog = dog;

            MasterDetailSideMenucs.CreateMasterPage();
            await Navigation.PushModalAsync(MasterDetailSideMenucs.MasterDetailPage);
        }
        public async Task<List<Dog>> GetDogsByOwner(string owner)
        {
            
            List < Dog > listOfDogs = await manager.CheckOwnerDogs(owner);

            if(listOfDogs == null)
            {
                return null;
            }
            return listOfDogs;
        }
        }
    
}
