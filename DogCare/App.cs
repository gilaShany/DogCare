using Syncfusion.SfSchedule.XForms;
using System;

using Xamarin.Forms;

namespace DogCare
{
	public class App : Application
	{

        public static Dog currentDog;
        public static Owner currentOwner;
        
        public App()
        {

             MainPage = new NavigationPage(new TabbedLogin());
        }

		protected override void OnStart ()
		{
            
        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

