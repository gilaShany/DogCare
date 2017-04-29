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
    public partial class TabbedLogin : TabbedPage
    {
        public TabbedLogin()
        {
            InitializeComponent();
            SqliteConnectionSet._connection = DependencyService.Get<ISQLiteDb>().GetConnection();
        }
        protected override async void OnAppearing()
        {
            await SqliteConnectionSet._connection.CreateTableAsync<Owner>();

            var owner = await SqliteConnectionSet._connection.Table<Owner>().ToListAsync();
            SqliteConnectionSet._user = new ObservableCollection<Owner>(owner);
            base.OnAppearing();
        }

    }
}
