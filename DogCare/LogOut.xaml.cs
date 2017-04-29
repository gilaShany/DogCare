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
    public partial class LogOut : ContentPage
    {
        public LogOut()
        {
            InitializeComponent();
            Navigation.PushModalAsync(new NavigationPage(new TabbedLogin()));
        }
        protected override async void OnAppearing()
        {
            await SqliteConnectionSet._connection.DeleteAsync(App.currentOwner);
            SqliteConnectionSet._user.Remove(App.currentOwner);
            base.OnAppearing();
        }
    }
}
