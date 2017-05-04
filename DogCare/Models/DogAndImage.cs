using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DogCare.Models
{
    class DogAndImage
    {
        Dog dog;
        Image dogImage;

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
