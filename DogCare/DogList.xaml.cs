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
    public partial class DogList : ContentPage
    {
        DogManager manager;

        public DogList()
        {
            manager = DogManager.DefaultManager;
            InitializeComponent();
        }

        async Task AddItem(Dog item)
        {
            await manager.SaveTaskAsync(item);
        }

        async private void Button_Clicked(object sender, EventArgs e)
        {

            var dog = new Dog
            {
                DogName = newDog.Text,
                Owner = App.typedUserName,
                Gender = genderS.Text,
                Race = raceS.Text

                
            };
            await AddItem(dog);

            newDog.Text = string.Empty;
            newDog.Unfocus();

        }
        }
    
}
