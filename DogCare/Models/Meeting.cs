using SQLite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogCare
{
    public class Meeting
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Subject { get; set; }
        public string Location { get; set; }
        public string From { get; set; }
        public string To { get; set; }
 
}
}
