using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DogCare
{
    class MasterDetailSideMenucs
    {
        public static MasterDetailPage MasterDetailPage;

        public static void CreateMasterPage()
        {
            MasterDetailPage = new MasterDetailPage
            {
                Master = new MenuPage(),
                Detail = new NavigationPage(new MainPage())

            };
        }
}
}
