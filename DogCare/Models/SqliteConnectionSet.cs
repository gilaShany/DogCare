using SQLite;
using Syncfusion.SfSchedule.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DogCare
{
    public class SqliteConnectionSet
    {
        public static SQLiteAsyncConnection _connection;
        public static ObservableCollection<Meeting> _appointments;
        public static StackLayout mainStack = new StackLayout();
        public static ScheduleAppointmentCollection AppointmentCollection;
        public static bool isNewAppointment;
        public static bool isNewCalendar = false;
        public static bool isTappedOnce = true;
    }
}
