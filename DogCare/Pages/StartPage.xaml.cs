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
    public partial class StartPage : ContentPage
    {
        OwnerManager manager;
        DogManager dManager;
        public StartPage()
        {
            InitializeComponent();
            SqliteConnectionSet._connection = DependencyService.Get<ISQLiteDb>().GetConnection();


        }
        protected override async void OnAppearing()
        {
            await SqliteConnectionSet._connection.CreateTableAsync<Owner>();

            var owner = await SqliteConnectionSet._connection.Table<Owner>().ToListAsync();
            SqliteConnectionSet._user = new ObservableCollection<Owner>(owner);
            manager = OwnerManager.DefaultManager;
            dManager = DogManager.DefaultManager;
            movePage();
            base.OnAppearing();
        }
        public async void movePage()
        {
            if (SqliteConnectionSet._user.Count == 0)
                await Navigation.PushModalAsync(new NavigationPage(new TabbedLogin()));
            else
            {
                
                var method = await (manager.CheckUserNameAndPassword(SqliteConnectionSet._user[0].UserName, SqliteConnectionSet._user[0].Password));
                App.currentOwner = method;
                await Navigation.PushModalAsync(new NavigationPage(new MyDogsPage()));

            }
        }
    }
}
