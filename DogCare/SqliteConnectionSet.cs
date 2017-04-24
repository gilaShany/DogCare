using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogCare
{
    public class SqliteConnectionSet
    {
        public static SQLiteAsyncConnection _connection;
        public static ObservableCollection<Meeting> _appointments;
        public static int appointmentId = 1;
    }
}
