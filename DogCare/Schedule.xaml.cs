﻿using System;
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
        SfSchedule schedule;
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
            base.OnAppearing();
        }
        public async void AddMeetingsToSchedule()
        {
            var appointments = await SqliteConnectionSet._connection.Table<Meeting>().ToListAsync();
            
            foreach(Meeting meet in appointments)
            {
                AddNewMeetingToSchedule(meet);
            }
        }
        public static void AddNewMeetingToSchedule(Meeting meet)
        {
            ScheduleAppointment appointment = new ScheduleAppointment();
            appointment.Location = meet.Location;
            appointment.Subject = meet.Subject;
            appointment.StartTime = Convert.ToDateTime(meet.From);
            appointment.EndTime = Convert.ToDateTime(meet.To);
            App.AppointmentCollection.Add(appointment);

        }
        public async void UpdateMeetingToSchedule(Meeting meet,ScheduleAppointment appointment)
        {
            meet.Location = appointment.Location ;
            meet.Subject = appointment.Subject ;
            meet.From = appointment.StartTime.ToString();
            meet.To = appointment.EndTime.ToString();
            await SqliteConnectionSet._connection.UpdateAsync(meet);

        }
        void InitializeSchedule()
        {
            schedule = new SfSchedule();
            App.AppointmentCollection = new ScheduleAppointmentCollection();
            schedule.VerticalOptions = LayoutOptions.FillAndExpand;
            schedule.ScheduleView = ScheduleView.WeekView;
            schedule.EnableNavigation = true;
            schedule.BackgroundColor = Color.White;
            if (!App.isNewCalendar)
            {

                App.mainStack.Children.Add(schedule);
                App.isNewCalendar = true;
            }

            this.Content = App.mainStack;

            schedule.DataSource = App.AppointmentCollection;
            schedule.ScheduleCellTapped += schedule_ScheduleCellTapped;
        }
        async Task<Meeting> GetMeeting(int index)
        {
            return await SqliteConnectionSet._connection.Table<Meeting>().Where(m => m.Id == index).FirstOrDefaultAsync();
        }

        async void schedule_ScheduleCellTapped(object sender, ScheduleTappedEventArgs args)
        {
            appointment = new Appointment((ScheduleAppointment)args.selectedAppointment);
            if (args.selectedAppointment == null)
            {
                App.isNewAppointment = true;
                App.mainStack.Children.Add(appointment);
                appointment.UpdateEditor((ScheduleAppointment)args.selectedAppointment, args.datetime, this.schedule);
                this.schedule.IsVisible = false;
                appointment.IsVisible = true;
            }
            else
            {

                int index = appointment.getIndexOfAppointment((ScheduleAppointment)args.selectedAppointment, App.AppointmentCollection);

                Meeting meet = await GetMeeting(index);
                await DisplayAlert("nooo", "crash" + index, "c");

                App.isNewAppointment = false;
                this.schedule.IsVisible = false;

                App.mainStack.Children.Add(appointment);
                App.mainStack.Children[App.mainStack.Children.Count - 1].IsVisible = true;
                //appointment.CreatingEditorSettingsLayout();

                appointment.UpdateEditor((ScheduleAppointment)args.selectedAppointment, args.datetime, this.schedule);

                UpdateMeetingToSchedule(meet, (ScheduleAppointment)args.selectedAppointment);

            }
        }
    }
}


