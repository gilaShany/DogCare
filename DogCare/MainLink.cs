using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DogCare
{
    public class MainLink : Button
    {
        public MainLink(string name)
        {

            Text = name;
            Command = new Command(o => {
                MasterDetailSideMenucs.MasterDetailPage.Detail = new NavigationPage(new LinkPage(name));
                MasterDetailSideMenucs.MasterDetailPage.IsPresented = false;
            });
        }
    }
}
