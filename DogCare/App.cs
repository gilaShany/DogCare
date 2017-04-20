using Syncfusion.SfSchedule.XForms;
using System;

using Xamarin.Forms;

namespace DogCare
{
	public class App : Application
	{

        public static Dog currentDog;
        public static Owner currentOwner;
        public static StackLayout mainStack = new StackLayout();
        public static ScheduleAppointmentCollection AppointmentCollection;
        public static bool isNewAppointment;
        public static bool isNewCalendar = false;


        public App()
        {
            // The root page of your application
            MainPage = new NavigationPage(new TabbedLogin());
            //MainPage = new Schedule();
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

