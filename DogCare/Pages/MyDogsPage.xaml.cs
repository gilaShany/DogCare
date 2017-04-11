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
        
         void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var dog = e.SelectedItem as Dog;
           // await Navigation.PushAsync(new MyDogsDetailPage(dog));
          //  listView.SelectedItem = null;
        }
        
        public MyDogsPage()
        {
            
            InitializeComponent();
            listView.ItemsSource = new List<Dog> {
                new Dog { DogName = "Mosh", Image = "http://lorempixel.com/100/100/people/1" },
                new Dog { DogName = "John", Image = "http://lorempixel.com/100/100/people/2" }
            };
        }
    }
}
