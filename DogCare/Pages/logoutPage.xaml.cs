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
    public partial class logoutPage : ContentPage
    {
        public logoutPage()
        {
            InitializeComponent();
            Navigation.PushModalAsync(new NavigationPage(new TabbedLogin()));
        }
        protected override async void OnAppearing()
        {
            await SqliteConnectionSet._connection.DeleteAsync(SqliteConnectionSet._user[0]);
            SqliteConnectionSet._user.Remove(SqliteConnectionSet._user[0]);

            base.OnAppearing();
        }
    }
}
