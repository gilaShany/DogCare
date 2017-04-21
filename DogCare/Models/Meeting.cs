
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogCare
{
    public class Meeting 
    {
        public int Id { get; set; }

        public string Subject { get; set; }
        public string Location { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Color color { get; set; }

    }
}
