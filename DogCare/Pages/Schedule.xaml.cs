using System;
using System.Collections.ObjectModel;
using Syncfusion.SfSchedule.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;

namespace DogCare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Schedule : ContentPage
    {
        SfSchedule schedule;
        //Appointment appointment;

        public Schedule()
        {
            InitializeComponent();
            InitializeSchedule();

        }
        public void InitializeSchedule()
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
            schedule.ScheduleCellTapped += schedule_ScheduleCellTapped;
        }
        
        void schedule_ScheduleCellTapped(object sender, ScheduleTappedEventArgs args)
        {
            if (args.selectedAppointment == null)
            {
                Appointment appointment = new Appointment((ScheduleAppointment)args.selectedAppointment);
                App.isNewAppointment = true;
                appointment.UpdateEditor((ScheduleAppointment)args.selectedAppointment, args.datetime, this.schedule);
                App.mainStack.Children.Add(appointment);
                App.mainStack.Children[0].IsVisible = false;
                appointment.IsVisible = true;
            }
            else
            {
                Appointment appointment = new Appointment((ScheduleAppointment)args.selectedAppointment);
                App.isNewAppointment = false;
                App.mainStack.Children[0].IsVisible = false;
                App.mainStack.Children[appointment.getIndexOfAppointment((ScheduleAppointment)args.selectedAppointment, App.AppointmentCollection) + 1].IsVisible = true;
                appointment.CreatingEditorSettingsLayout();
                appointment.UpdateEditor((ScheduleAppointment)args.selectedAppointment, args.datetime, this.schedule);
            }

        }
    }
}


