
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Syncfusion.SfSchedule.XForms;

namespace DogCare
{
    public partial class EditorLayout : StackLayout
    {
        private Meeting selectedAppointment;
        private DateTime selectedDate;


        public EditorLayout()
        {
            InitializeComponent();
            saveButton.Clicked += SaveButton_Clicked;
            cancelButton.Clicked += CancelButton_Clicked;
            if ( Device.Idiom == TargetIdiom.Phone)
            {
                StartdateTimePicker_layout.ColumnDefinitions.Clear();
                Grid.SetColumn(start_datepicker_layout, 0);
                Grid.SetRow(start_datepicker_layout, 0);
                Grid.SetColumn(start_timepicker_layout, 0);
                Grid.SetRow(start_timepicker_layout, 1);
                StartdateTimePicker_layout.HeightRequest = 80;
                eventName_layout.Padding = new Thickness(0, 10, 0, 0);
                organizer_layout.Padding = new Thickness(0, 20, 0, 0);
                startTimeLabel_layout.Padding = new Thickness(0, 20, 0, 0);
                StartdateTimePicker_layout.Padding = new Thickness(0, -20, 0, 0);
                StartdateTimePicker_layout.RowDefinitions = new RowDefinitionCollection()
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                };
                EndDateTimePicker_layout.ColumnDefinitions.Clear();
                Grid.SetColumn(end_datepicker_layout, 0);
                Grid.SetRow(end_datepicker_layout, 0);
                Grid.SetColumn(end_timepicker_layout, 0);
                Grid.SetRow(end_timepicker_layout, 1);
                EndDateTimePicker_layout.HeightRequest = 80;
                endTimeLabel_layout.Padding = new Thickness(0, 20, 0, 0);
                EndDateTimePicker_layout.Padding = new Thickness(0, -20, 0, 0);
                EndDateTimePicker_layout.RowDefinitions = new RowDefinitionCollection()
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                };
            }

        }
        public void OpenEditor(Meeting appointment, DateTime date)
        {
            //button_layout.BackgroundColor = Color.FromHex("#e6e6e6");
            cancelButton.BackgroundColor = Color.FromHex("#e6e6e6");
            saveButton.BackgroundColor = Color.FromHex("#e6e6e6");
            eventNameText.Placeholder = "Event Name";
            organizerText.Placeholder = "Organizer";
            selectedAppointment = null;
            if (appointment != null)
            {
                selectedAppointment = appointment;
                selectedDate = date;
            }
            else
            {
                selectedDate = date;
            }
            UpdateEditor();


        }

        public void CancelButton_Clicked(object sender, EventArgs e)
        {
            ScheduleAppointmentModifiedEventArgs args = new ScheduleAppointmentModifiedEventArgs();
            args.Appointment = null;
            args.IsModified = false;
            OnAppointmentModified(args);
            this.IsVisible = false;
        }

        public void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (selectedAppointment == null)
            {
                selectedAppointment = new Meeting();
                //selectedAppointment.color = Color.Accent;
            }
            if (eventNameText.Text != null && eventNameText != null)
            {
                selectedAppointment.Subject = eventNameText.Text.ToString();
            }
            if (organizerText.Text != null && organizerText != null)
            {
                selectedAppointment.Location = organizerText.Text.ToString();
            }
            selectedAppointment.From = startDate_picker.Date.Add(startTime_picker.Time);
            selectedAppointment.To = endDate_picker.Date.Add(endTime_picker.Time);


            ScheduleAppointmentModifiedEventArgs args = new ScheduleAppointmentModifiedEventArgs();
            args.Appointment = selectedAppointment;
            args.IsModified = true;
            OnAppointmentModified(args);

            this.IsVisible = false;

        }


        protected virtual void OnAppointmentModified(ScheduleAppointmentModifiedEventArgs e)
        {
            EventHandler<ScheduleAppointmentModifiedEventArgs> handler = AppointmentModified;
            if (handler != null)
            {
                handler(this, e);
            }

        }
        private void UpdateEditor()
        {
            if (selectedAppointment != null)
            {
                eventNameText.Text = selectedAppointment.Subject.ToString();
                organizerText.Text = selectedAppointment.Location;
                startDate_picker.Date = selectedAppointment.From;
                startTime_picker.Time = new TimeSpan(selectedAppointment.From.Hour, selectedAppointment.From.Minute, selectedAppointment.From.Second);
                endDate_picker.Date = selectedAppointment.To;
                endTime_picker.Time = new TimeSpan(selectedAppointment.To.Hour, selectedAppointment.To.Minute, selectedAppointment.To.Second);

            }
            else
            {
                eventNameText.Text = "";
                organizerText.Text = "";
                startDate_picker.Date = selectedDate;
                startTime_picker.Time = new TimeSpan(selectedDate.Hour, selectedDate.Minute, selectedDate.Second);
                endDate_picker.Date = selectedDate;
                endTime_picker.Time = new TimeSpan(selectedDate.Hour + 1, selectedDate.Minute, selectedDate.Second);
            }


        }
        public event EventHandler<ScheduleAppointmentModifiedEventArgs> AppointmentModified;
    }
    public class ScheduleAppointmentModifiedEventArgs : EventArgs
    {
        public Meeting Appointment { get; set; }
        public bool IsModified { get; set; }
    }
}

