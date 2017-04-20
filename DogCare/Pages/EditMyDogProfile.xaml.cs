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
    public partial class EditMyDogProfile : ContentPage
    {
        DogManager manager;
        public EditMyDogProfile()
        {
            InitializeComponent();
            BindingContext = App.currentDog;
            manager = DogManager.DefaultManager;
        }

        async private void Edit_Clicked(object sender, EventArgs e)
        {
           
            if(newDog.Text != null)
            {
                App.currentDog.DogName = newDog.Text;
            }
            if (raceS.Text !=null)
            {
                App.currentDog.Race = raceS.Text;
            }
            if (genderS.Text != null)
            {
                App.currentDog.Gender = genderS.Text;
            }
            await manager.SaveTaskAsync(App.currentDog);
            await DisplayAlert("", string.Format("{0} updated succefully", App.currentDog.DogName),"Ok");
        }
    }
}
