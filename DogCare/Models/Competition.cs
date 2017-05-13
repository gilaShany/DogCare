using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DogCare.Models
{
    class Competition
    {
        Image index;
        Dog dog;
        Image dogImage;

        public Image Index
        {
            get { return index; }
            set { index = value; }
        }

        public Dog Dog
        {
            get { return dog; }
            set { dog = value; }
        }

        public Image DogImage
        {
            get { return dogImage; }
            set { dogImage = value; }
        }
    }
}
