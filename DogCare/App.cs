using System;

using Xamarin.Forms;

namespace DogCare
{
	public class App : Application
	{
        public static string typedUserName;
        public static string typedPassword;
        public static string typedFullName;

        public App()
        {
            // The root page of your application
            MainPage = new NavigationPage(new TabbedLogin());
        }
		protected override void OnStart ()
		{
			// Handle when your app starts
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

