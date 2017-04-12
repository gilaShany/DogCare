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
    public partial class LinkPage : ContentPage
    {
        public LinkPage(string name)
        {
            DisplayAlert("", "Reached link", "cancle");

            Title = name;
            Content = new StackLayout
            {
                Children = {
                new SubLink(name + ".1"),
                new SubLink(name + ".2"),
                new SubLink(name + ".3"),
            },
            };
        }
    }
}
