using DogCare.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            BackgroundColor = Color.White;
            manager = DogManager.DefaultManager;
            GetDogsByOwner(App.currentOwner.UserName, null);
        }
        public async void GetDogsByOwner(string owner, string search)
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
                var groupedData =
                        listOfDogsWithImage.OrderBy(dog => dog.Dog.DogName)
                            .GroupBy(dog => dog.Dog.DogName)
                             .Select(dog => new ObservableGroupCollection<string, DogAndImage>(dog))
                            .ToList();

                if (String.IsNullOrWhiteSpace(search))
                    BindingContext = new ObservableCollection<ObservableGroupCollection<string, DogAndImage>>(groupedData);
                else
                    BindingContext = groupedData.Where(c => c.Key.StartsWith(search));
            }
        }

        async private void Add_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new DogList()));
        }

        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetDogsByOwner(App.currentOwner.UserName, e.NewTextValue);
        }
    }
}
