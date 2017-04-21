using Syncfusion.SfSchedule.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DogCare
{
    public class Appointment : StackLayout
    {

        #region Properties
        public ScheduleAppointment selected_appointment;
        public int index_of_appointment = 1;
        public StackLayout editor_layout = new StackLayout();
        public StackLayout appointmenteditor_main_layout = new StackLayout();
        public Entry subject_text, location_text;
        public DatePicker start_date_picker, end_date_picker;
        public TimePicker start_time_picker, end_time_picker;
        public Label start_date_label = new Label();
        public Label end_date_label = new Label();
        public Label start_time_label = new Label();
        public Label end_time_label = new Label();
        public Button cancel_button { get; set; }
        public Button save_button { get; set; }

        public SfSchedule Schedule;
        #endregion

        public Appointment(ScheduleAppointment appointment)
        {
            CreatingEditorSettingsLayout();
            //selected_appointment = appointment;
        }

        #region Methods

        #region Editor Layout Appear

        // Animation Appear      
        public async void EditorAppear()
        {
            this.IsVisible = true;

            var editor_initial_bounds = new Rectangle(editor_layout.X, 1000, editor_layout.Width, editor_layout.Height);
                var editor_final_bounds = new Rectangle(editor_layout.X, 0, editor_layout.Width, editor_layout.Height);
                await this.editor_layout.LayoutTo(editor_initial_bounds, 0, Easing.Linear);
                await this.editor_layout.LayoutTo(editor_final_bounds, 500, Easing.Linear);

        }

        #endregion Editor Layout Appear

        #region StartTime and EndTime Pickers

        //StartTimePicker
        public void StartTimePickerPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (start_time_picker.Time.Hours / 12 <= 0)
            {
                start_time_label.Text = (sender as TimePicker).Time.ToString("") + " AM";
            }
            else
            {
                var time_picker = (sender as TimePicker).Time;
                time_picker = new TimeSpan((sender as TimePicker).Time.Hours - 12, (sender as TimePicker).Time.Minutes, (sender as TimePicker).Time.Seconds);
                start_time_label.Text = time_picker.ToString("") + " PM";
            }
        }

        //EndTimePicker
        public void EndTimePickerPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (end_time_picker.Time.Hours / 12 <= 0)
            {
                end_time_label.Text = (sender as TimePicker).Time.ToString("") + " AM";
            }
            else
            {
                var timePicker = (sender as TimePicker).Time;
                timePicker = new TimeSpan((sender as TimePicker).Time.Hours - 12, (sender as TimePicker).Time.Minutes, (sender as TimePicker).Time.Seconds);
                end_time_label.Text = timePicker.ToString("") + " PM";
            }
        }

        //EndDatePicker
        public void EndDatePickerDateSelected(object sender, DateChangedEventArgs e)
        {
            end_date_label.Text = (sender as DatePicker).Date.ToString("MMMM dd, yyyy");
        }

        //StartDatePicker
        public void StartDatePickerDateSelected(object sender, DateChangedEventArgs e)
        {
            start_date_label.Text = (sender as DatePicker).Date.ToString("MMMM dd, yyyy");
        }

        #endregion StartTime and EndTime Pickers

        #region get Index of Appointment

        //Index of Appointment
        public int getIndexOfAppointment(ScheduleAppointment appointment, ScheduleAppointmentCollection appointmentCollection)
        {
            int index = -1;
            for (int i = 0; i < appointmentCollection.Count; i++)
            {
                if (appointmentCollection[i].Subject == appointment.Subject && appointmentCollection[i].StartTime == appointment.StartTime && appointmentCollection[i].EndTime == appointment.EndTime)
                {
                    index = i;
                }
            }

            return index;
        }

        #endregion get Index of Appointment

        #region Editor

        //Creating Appointment Editor Settings Layout
        async public void CreatingEditorSettingsLayout()
        {
            editor_layout.BackgroundColor = Color.White;
            editor_layout.Padding = new Thickness(10);

            start_date_picker = new DatePicker();
            start_time_picker = new TimePicker();
            end_date_picker = new DatePicker();
            end_time_picker = new TimePicker();

            //Adding subject 
            StackLayout subject_layout = new StackLayout();
            StackLayout textViewLayout = new StackLayout();
            textViewLayout.Orientation = StackOrientation.Vertical;

            Label subject = new Label();
            subject.Text = "Subject";
            subject.FontSize = 20;
            subject_layout.Children.Add(subject);

            subject_text = new Entry();
            subject_text.WidthRequest = 30;
            subject_layout.Children.Add(subject_text);

            textViewLayout.Children.Add(subject_layout);

            //Adding Location
            StackLayout location_layout = new StackLayout();
            Label Location = new Label();
            Location.Text = "Location";
            Location.FontSize = 20;
            location_layout.Children.Add(Location);

            location_text = new Entry();
            location_layout.Children.Add(location_text);
            textViewLayout.Children.Add(location_layout);

            editor_layout.Children.Add(textViewLayout);

            //Adding Start Date Picker
            StackLayout start_time_layout = new StackLayout();
            start_time_layout.Orientation = StackOrientation.Horizontal;

            Label StartTime = new Label();
            StartTime.Text = "StartTime";
            StartTime.FontSize = 20;
            start_time_layout.Children.Add(StartTime);

            StackLayout start_date_label_layout = new StackLayout();
            start_date_label_layout.Orientation = StackOrientation.Horizontal;

            start_date_label.FontSize = 15;
            start_date_label.TextColor = Color.FromHex("FF6A00");

            start_time_label.FontSize = 15;
            start_time_label.TextColor = Color.FromHex("FF6A00");

            start_date_label_layout.Children.Add(start_time_label);

            start_time_layout.Children.Add(start_date_label_layout);
            editor_layout.Children.Add(start_time_layout);

            StackLayout start_date_picker_layout = new StackLayout();

            StackLayout starting_date_layout = new StackLayout();
            starting_date_layout.Orientation = StackOrientation.Horizontal;
            starting_date_layout.Children.Add(start_date_picker);

            StackLayout start_Time_picker_layout = new StackLayout();
            start_Time_picker_layout.Orientation = StackOrientation.Horizontal;

            start_date_picker_layout.Children.Add(starting_date_layout);
            start_date_picker_layout.Children.Add(start_Time_picker_layout);
            start_date_picker_layout.Children.Add(start_date_picker);
            start_date_picker_layout.Children.Add(start_time_picker);


            editor_layout.Children.Add(start_date_picker_layout);

            //Adding End Date Picker 
            StackLayout end_time_layout = new StackLayout();
            end_time_layout.Orientation = StackOrientation.Horizontal;

            Label endTime = new Label();
            endTime.Text = "EndTime";
            endTime.FontSize = 20;
            end_time_layout.Children.Add(endTime);

            StackLayout end_date_layout = new StackLayout();
            end_date_layout.Orientation = StackOrientation.Horizontal;

            end_date_label.FontSize = 15;
            end_date_label.TextColor = Color.FromHex("FF6A00");

            end_time_label.FontSize = 15;
            end_time_label.TextColor = Color.FromHex("FF6A00");

            end_time_layout.Children.Add(end_date_layout);
            editor_layout.Children.Add(end_time_layout);

            StackLayout end_date_picker_layout = new StackLayout();

            StackLayout ending_date_picker_layout = new StackLayout();
            ending_date_picker_layout.Orientation = StackOrientation.Horizontal;
            ending_date_picker_layout.Children.Add(end_date_picker);

            StackLayout end_Time_picker_layout = new StackLayout();
            end_Time_picker_layout.Orientation = StackOrientation.Horizontal;
            end_Time_picker_layout.Children.Add(end_time_picker);

            end_date_picker_layout.Children.Add(ending_date_picker_layout);
            end_date_picker_layout.Children.Add(end_Time_picker_layout);


            editor_layout.Children.Add(end_date_picker_layout);


            StackLayout buttons_layout = new StackLayout();
            buttons_layout.Orientation = StackOrientation.Horizontal;

            cancel_button = new Button();
            cancel_button.Clicked += CancelButton_Clicked;
            cancel_button.Text = "cancel";

            save_button = new Button();
            save_button.Clicked += SaveButton_Clicked;
            save_button.Text = "Save";

            buttons_layout.Children.Add(cancel_button);
            buttons_layout.Children.Add(save_button);

            editor_layout.Children.Add(buttons_layout);


            this.Padding = 20;
            this.Children.Add(editor_layout);

        }

        public void CancelButton_Clicked(object sender, EventArgs e)
        {

            this.IsVisible = false;
            App.mainStack.Children[0].IsVisible = true;

        }
        public void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (selected_appointment == null)
            {
                selected_appointment = new ScheduleAppointment();
                saveNewAppintment(selected_appointment);
                //selectedAppointment.color = Color.Accent;
            }
            if (location_text.Text != null )
            {
                selected_appointment.Location = location_text.Text.ToString();
            }
            if (subject_text.Text != null )
            {
                selected_appointment.Subject = subject_text.Text.ToString() + " for " + App.currentDog.DogName;
            }
            selected_appointment.StartTime = start_date_picker.Date.Add(start_time_picker.Time);
            selected_appointment.EndTime = end_date_picker.Date.Add(end_time_picker.Time);
            if (App.isNewAppointment)
            {
                App.AppointmentCollection.Add(selected_appointment);
                //App._connection.InsertAsync(selected_appointment);
               // App._ScheduleAppointments.Add(selected_appointment);
                index_of_appointment++;
            }

            this.IsVisible = false;
            App.mainStack.Children[0].IsVisible = true;
        }
        #endregion Editor

        #region Editor layout Disappear

        // Animation Disappear
        public async void EditorDisappear()
        {
            var editorBounds = new Rectangle(editor_layout.X, 1000, editor_layout.Width, editor_layout.Height);
            await this.editor_layout.LayoutTo(editorBounds, 500, Easing.Linear);
        }

        #endregion Editor layout Disappear

        #region UpdateEditor
        
        public void UpdateEditor(ScheduleAppointment selectedAppointment, DateTime dateTime, SfSchedule schedule)
        {
            selected_appointment = null;
            if (selectedAppointment != null)
            {
                selected_appointment = (ScheduleAppointment)selectedAppointment;
                DateTime start_time = selected_appointment.StartTime;
                DateTime end_time = selected_appointment.EndTime;
                subject_text.Text = selected_appointment.Subject;
                location_text.Text = selected_appointment.Location;
                index_of_appointment = getIndexOfAppointment(selected_appointment, (Schedule.DataSource as ScheduleAppointmentCollection)); ;
                start_date_picker.Date = new DateTime(start_time.Year, start_time.Month, start_time.Day);
                start_time_picker.Time = new TimeSpan(start_time.Hour, start_time.Minute, start_time.Second);
                end_date_picker.Date = new DateTime(end_time.Year, end_time.Month, end_time.Day);
                end_time_picker.Time = new TimeSpan(end_time.Hour, end_time.Minute, end_time.Second);

            }
            else
            {
                subject_text.Text = "";
                location_text.Text = "";
                DateTime s_time = dateTime; //args.datetime;//
                start_date_picker.Date = new DateTime(s_time.Year, s_time.Month, s_time.Day);
                start_time_picker.Time = new TimeSpan(s_time.Hour, s_time.Minute, s_time.Second);
                // start_time_picker.Format = "12 hours";
                end_date_picker.Date = new DateTime(s_time.Year, s_time.Month, s_time.Day);
                end_time_picker.Time = new TimeSpan(s_time.Hour + 1, s_time.Minute, s_time.Second);
            }

            start_date_picker.DateSelected += StartDatePickerDateSelected;
            end_date_picker.DateSelected += EndDatePickerDateSelected;
            start_time_picker.PropertyChanged += StartTimePickerPropertyChanged;
            end_time_picker.PropertyChanged += EndTimePickerPropertyChanged;

            //Date Label
            start_date_label.Text = start_date_picker.Date.ToString("MMMM dd, yyyy");
            end_date_label.Text = end_date_picker.Date.ToString("MMMM dd, yyyy");

            //Time Label
            if (end_time_picker.Time.Hours / 12 <= 0)
                end_time_label.Text = end_time_picker.Time.ToString("") + " AM";
            else
            {
                var timePicker = (end_time_picker).Time;
                timePicker = new TimeSpan((end_time_picker).Time.Hours - 12, (end_time_picker).Time.Minutes, (end_time_picker).Time.Seconds);
                end_time_label.Text = timePicker.ToString("") + " PM";
            }

            if (start_time_picker.Time.Hours / 12 <= 0)
                start_time_label.Text = start_time_picker.Time.ToString("") + " AM";
            else
            {
                var time_picker = (start_time_picker).Time;
                time_picker = new TimeSpan((start_time_picker).Time.Hours - 12, (start_time_picker).Time.Minutes, (start_time_picker).Time.Seconds);
                start_time_label.Text = time_picker.ToString("") + " PM";
            }

        }

    
        #endregion

        #region Save
        public void saveAppointment()
        {
            (App.AppointmentCollection)[index_of_appointment].Subject = subject_text.Text;
            (App.AppointmentCollection)[index_of_appointment].Location = location_text.Text;

            DateTime startDate = new DateTime(start_date_picker.Date.Year, start_date_picker.Date.Month, start_date_picker.Date.Day, start_time_picker.Time.Hours, start_time_picker.Time.Minutes, start_time_picker.Time.Seconds);
            (App.AppointmentCollection)[index_of_appointment].StartTime = startDate;

            DateTime endDate = new DateTime(end_date_picker.Date.Year, end_date_picker.Date.Month, end_date_picker.Date.Day, end_time_picker.Time.Hours, end_time_picker.Time.Minutes, end_time_picker.Time.Seconds);
            (App.AppointmentCollection)[index_of_appointment].EndTime = endDate;

        }

        public void saveNewAppintment(ScheduleAppointment selected_appointment)
        {
            selected_appointment.Subject = subject_text.Text;
            selected_appointment.Location = location_text.Text;

            DateTime startDate = new DateTime(start_date_picker.Date.Year, start_date_picker.Date.Month, start_date_picker.Date.Day, start_time_picker.Time.Hours, start_time_picker.Time.Minutes, start_time_picker.Time.Seconds);
            selected_appointment.StartTime = startDate;

            DateTime endDate = new DateTime(end_date_picker.Date.Year, end_date_picker.Date.Month, end_date_picker.Date.Day, end_time_picker.Time.Hours, end_time_picker.Time.Minutes, end_time_picker.Time.Seconds);
            selected_appointment.EndTime = endDate;
        }

        #endregion Save

        #endregion Methods

        public double widthAlloc { get; set; }
        public double heightAlloc { get; set; }

    }
}
