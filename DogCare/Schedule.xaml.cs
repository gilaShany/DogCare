using System;
using System.Collections.ObjectModel;
using Syncfusion.SfSchedule.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using SQLite;
using System.Threading.Tasks;

namespace DogCare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Schedule : ContentPage
    {
        public static SfSchedule schedule;
        Appointment appointment;

        public Schedule()
        {
            InitializeComponent();
            InitializeSchedule();
            SqliteConnectionSet._connection = DependencyService.Get<ISQLiteDb>().GetConnection();

        }
        async protected override void OnAppearing()
        {
            await SqliteConnectionSet._connection.CreateTableAsync<Meeting>();

            var appointments = await SqliteConnectionSet._connection.Table<Meeting>().ToListAsync();
            SqliteConnectionSet._appointments = new ObservableCollection<Meeting>(appointments);
            AddMeetingsToSchedule();

            //RemoveAll();

            base.OnAppearing();
        }
        void InitializeSchedule()
        {
            schedule = new SfSchedule();
            SqliteConnectionSet.AppointmentCollection = new ScheduleAppointmentCollection();
            schedule.VerticalOptions = LayoutOptions.FillAndExpand;
            schedule.ScheduleView = ScheduleView.WeekView;
            schedule.EnableNavigation = true;
            schedule.BackgroundColor = Color.White;

            if (SqliteConnectionSet.isNewCalendar)
            {
                SqliteConnectionSet.mainStack.Children.Clear();
                SqliteConnectionSet.mainStack.Children.Add(schedule);
                //AddMeetingsToSchedule();
            }
            else
            {
                SqliteConnectionSet.mainStack.Children.Add(schedule);
                SqliteConnectionSet.isNewCalendar = true;

            }


            this.Content = SqliteConnectionSet.mainStack;

            schedule.DataSource = SqliteConnectionSet.AppointmentCollection;
            schedule.ScheduleCellTapped += schedule_ScheduleCellTapped;
        }

        async void schedule_ScheduleCellTapped(object sender, ScheduleTappedEventArgs args)
        {
            appointment = new Appointment((ScheduleAppointment)args.selectedAppointment);

            if (args.selectedAppointment == null)
            {
                SqliteConnectionSet.isNewAppointment = true;
                SqliteConnectionSet.mainStack.Children.Add(appointment);
                appointment.UpdateEditor((ScheduleAppointment)args.selectedAppointment, args.datetime, schedule);
                schedule.IsVisible = false;
                appointment.IsVisible = true;
            }
            else
            {

                Meeting meet = await FindAppointment((ScheduleAppointment)args.selectedAppointment);
                //await DisplayAlert("v", "here" + SqliteConnectionSet.mainStack.Children.Count , "c");

                SqliteConnectionSet.isNewAppointment = false;
                schedule.IsVisible = false;

                SqliteConnectionSet.mainStack.Children.Add(appointment);
                SqliteConnectionSet.mainStack.Children[SqliteConnectionSet.mainStack.Children.Count - 1].IsVisible = true;
                // await DisplayAlert("v", "here" + SqliteConnectionSet.mainStack.Children.Count , "c");

                appointment.UpdateEditor((ScheduleAppointment)args.selectedAppointment, args.datetime, schedule);
                //UpdateMeetingToSchedule(meet, (ScheduleAppointment)args.selectedAppointment);

            }

        }

        public async static void AddMeetingsToSchedule()
        {
            var appointments = await SqliteConnectionSet._connection.Table<Meeting>().ToListAsync();
            foreach(Meeting meet in appointments)
            {
                AddNewMeetingToSchedule(meet);
            }
        }

        public async static void RemoveAll()
        {
            var appointments = await SqliteConnectionSet._connection.Table<Meeting>().ToListAsync();

            foreach (Meeting meet in appointments)
            {
                await SqliteConnectionSet._connection.DeleteAsync(meet);
                SqliteConnectionSet._appointments.Remove(meet);
            }
        }
        public async static Task<Meeting> FindAppointment(ScheduleAppointment appointment)
        {
            var appointments = await SqliteConnectionSet._connection.Table<Meeting>().ToListAsync();
           
            foreach (Meeting meet in appointments)
            {
                if (meet.Subject == appointment.Subject && meet.From == appointment.StartTime.ToString() && meet.To == appointment.EndTime.ToString())
                {
                    return meet;
                }
            }
            return null;
        }
        public static void AddNewMeetingToSchedule(Meeting meet)
        {
            ScheduleAppointment appointment1 = new ScheduleAppointment();
            appointment1.Location = meet.Location;
            appointment1.Subject = meet.Subject;
            appointment1.StartTime = Convert.ToDateTime(meet.From);
            appointment1.EndTime = Convert.ToDateTime(meet.To);
            SqliteConnectionSet.AppointmentCollection.Add(appointment1);
        }
        public async void UpdateMeetingToSchedule(Meeting meet,ScheduleAppointment appointment)
        {
            meet.Location = appointment.Location;
            meet.Subject = appointment.Subject;
            meet.From = appointment.StartTime.ToString();
            meet.To = appointment.EndTime.ToString();
            await DisplayAlert("b", "here", "c");
            await SqliteConnectionSet._connection.UpdateAsync(meet);

        }
        


    }
}


