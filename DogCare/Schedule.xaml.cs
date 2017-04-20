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
        Appointment appointment;

        public Schedule()
        {
            /*
            //InitializeComponent();
            fschedule = new SfSchedule();
            //editor = new EditorLayout();
            fschedule.ScheduleView = ScheduleView.WeekView;

            ScheduleAppointmentCollection appointmentCollection;

            appointmentCollection = new ScheduleAppointmentCollection();

            //Creating new event
            ScheduleAppointment clientMeeting = new ScheduleAppointment();

            DateTime currentDate = DateTime.Now;
            DateTime startTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 10, 0, 0);
            DateTime endTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 12, 0, 0);

            clientMeeting.StartTime = startTime;
            clientMeeting.EndTime = endTime;
            clientMeeting.Color = Color.Lime;
            clientMeeting.Subject = "ClientMeeting";

            appointmentCollection.Add(clientMeeting);
            fschedule.DataSource = appointmentCollection;
            //fschedule.ScheduleView = ScheduleView.MonthView;

            //fschedule.ShowAppointmentsInline = true;
            viewModel = new ScheduleViewModel();
            meetings = new ObservableCollection<Meeting>();

            HookEvents();
            //scheduleview_list.ItemsSource = Enum.GetValues(typeof(ScheduleView));
            //scheduleview_list.SelectedItem = 0;
            DayViewSettings dayViewSettings = new DayViewSettings();
            dayViewSettings.WorkStartHour = 7;
            dayViewSettings.WorkEndHour = 18;
            fschedule.DayViewSettings = dayViewSettings;
            fschedule.BindingContext = viewModel;
            
            dataMapping = new ScheduleAppointmentMapping();
            dataMapping.SubjectMapping = "EventName";
            dataMapping.StartTimeMapping = "From";
            dataMapping.EndTimeMapping = "To";
            dataMapping.ColorMapping = "color";
            fschedule.AppointmentMapping = dataMapping;
            //meetings = viewModel.ListOfMeeting;
            //fschedule.DataSource = meetings;
            //editor.AppointmentModified += Editor_AppointmentModified;
            this.Content = fschedule;
            */
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
        void schedule_ScheduleCellTapped(object sender, ScheduleTappedEventArgs args)
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
                App.isNewAppointment = false;
                this.schedule.IsVisible = false;
                App.mainStack.Children[appointment.getIndexOfAppointment((ScheduleAppointment)args.selectedAppointment, App.AppointmentCollection)+1].IsVisible = true;
                appointment.CreatingEditorSettingsLayout();
                appointment.UpdateEditor((ScheduleAppointment)args.selectedAppointment, args.datetime, this.schedule);
                
            }

            //appointment.saveNewAppintment(appointment.selected_appointment);
            //AppointmentCollection.Add(appointment.selected_appointment);
            //this.Content = mainStack.Children[1];

            /*
            if (args.selectedAppointment == null)
            {
                var dateTime = args.datetime;
                ScheduleAppointmentCollection scheduleAppointmentCollection = new ScheduleAppointmentCollection();
                ScheduleAppointment scheduleAppointment = (ScheduleAppointment)args.selectedAppointment;
                //Appointment appointment = new Appointment();

                scheduleAppointmentCollection.Add(scheduleAppointment);

                //scheduleview_list.IsVisible = false;
                if (fschedule.ScheduleView == ScheduleView.MonthView)
                {

                    fschedule.ScheduleView = ScheduleView.DayView;
                    //scheduleview_list.SelectedItem = 0;
                    fschedule.NavigateTo(args.datetime);

                }
                else
                {
                    //< local:Appointment x:Name = "editor" Grid.RowSpan = "1" />
                    ap = new Appointment();
                    fschedule.IsVisible = false;
                    ap.EditorAppear();
                    //await Navigation.PushModalAsync(new NewAppointment((ScheduleAppointment)args.selectedAppointment));

                    await DisplayAlert("HHH", "Got here", "c");

                    //ap.IsVisible = true;

                    ap.UpdateEditor((ScheduleAppointment)args.selectedAppointment, args.datetime, fschedule);
                    */
            /*
            if (args.selectedAppointment != null)
            {
                //var action = await DisplayActionSheet("Choose subject", "Cancel", "Delete", "Vet appointment", "Buy dogs food");

                ObservableCollection<Meeting> appointment = new ObservableCollection<Meeting>();
                appointment = (ObservableCollection<Meeting>)fschedule.DataSource;
                //indexOfAppointment = appointment.IndexOf((Meeting)args.selectedAppointment);
                //editor.OpenEditor((Meeting)args.selectedAppointment, args.datetime);
                editor.CreatingEditorSettingsLayout();
                editor.EditorAppear();
                //isNewAppointment = false;
            }
            else
            {

            //create Apppointmentt
            //editor = new EditorLayout();
            //Navigation.PushModalAsync(new NewAppointment((ScheduleAppointment)args.selectedAppointment));
            //editor.CreatingEditorSettingsLayout();
            //editor.EditorAppear();
            //editor.UpdateEditor((ScheduleAppointment)args.selectedAppointment, args.datetime, fschedule);
            //editor.OpenEditor(null, args.datetime);
            //isNewAppointment = true;

        }
    }
*/
        }
        /*
        void Editor_AppointmentModified(object sender, DogCare.ScheduleAppointmentModifiedEventArgs e)
        {
            fschedule.IsVisible = true;

            if (e.IsModified)
            {
                (fschedule.DataSource as ObservableCollection<Meeting>).Add(e.Appointment);
/*
                if (isNewAppointment)
                {
                    (fschedule.DataSource as ObservableCollection<Meeting>).Add(e.Appointment);
                }
                else
                {
                    (fschedule.DataSource as ObservableCollection<Meeting>).RemoveAt(indexOfAppointment);
                    (fschedule.DataSource as ObservableCollection<Meeting>).Insert(indexOfAppointment, e.Appointment);
                }
                
            }
        }
        */

    }
    }


