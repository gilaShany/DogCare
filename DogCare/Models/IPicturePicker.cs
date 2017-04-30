using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogCare.Models
{
    public interface IPicturePicker
    {
        Task<Stream> GetImageStreamAsync();
    }
}
