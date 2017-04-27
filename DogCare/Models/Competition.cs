using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogCare.Models
{
    class Competition
    {
        int index;
        Dog dog;

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        public Dog Dog
        {
            get { return dog; }
            set { dog = value; }
        }
    }
}
