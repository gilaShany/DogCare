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
    public partial class EditMyDogProfile : ContentPage
    {
        DogManager manager;
        MemoryStream memStream;

        public EditMyDogProfile()
        {
            InitializeComponent();
            BindingContext = App.currentDog;
            manager = DogManager.DefaultManager;
            memStream = null;

            //Checking if the dog has an image. If not, showing default image.
            if (App.currentDog.ImageD != null)
                image.Source = ImageSource.FromStream(() => Utils.ImageStream.ConvertStringToStream(App.currentDog.ImageD));
            else
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

        async private void Edit_Clicked(object sender, EventArgs e)
        {
            if ((CrossConnectivity.Current.IsConnected == false))
            {
                await DisplayAlert(Constants.internetAlertTittle, Constants.internetAlertMessage, null, Constants.internetButton);
            }
            else
            {
                if (newDog.Text != null)
                {
                    App.currentDog.DogName = newDog.Text;
                }
                if (raceS.Text != null)
                {
                    App.currentDog.Race = raceS.Text;
                }
                if (genderS.Text != null)
                {
                    App.currentDog.Gender = genderS.Text;
                }
                if (image.Source != ImageSource.FromFile("Dog.png"))
                {
                    App.currentDog.ImageD = Utils.ImageStream.ConvertStreamToString(memStream);
                }
                await manager.SaveTaskAsync(App.currentDog);
                await DisplayAlert("", string.Format("{0} updated succefully", App.currentDog.DogName), "Ok");
            }
        }
    }
}
