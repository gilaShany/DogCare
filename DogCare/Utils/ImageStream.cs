using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Plugin.Media.Abstractions;
using System.IO;

namespace DogCare.Utils
{
    class ImageStream
    {
        public static string ConvertStreamToString(MemoryStream memStream)
        {
            if (memStream != null)
            {
                memStream.Position = 0;
                byte[] bytes = memStream.ToArray();
                memStream.Position = 0;
                return Convert.ToBase64String(bytes);
            }
            else
                return "";
        }

        public static MemoryStream ConvertStringToStream(string base64String)
        {
            byte[] bytes = System.Convert.FromBase64String(base64String);
            MemoryStream memStream = new MemoryStream(bytes);
            memStream.Position = 0;
            return memStream;
        }

        public static MemoryStream ConvertStreamToMemoryStream(Stream stream)
        {
            try
            {
                byte[] buffer = new byte[16 * 1024];
                MemoryStream ms = new MemoryStream();
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                ms.Position = 0;
                return ms;
            }
            catch (Exception le)
            {
                // TODO: Log this error.
                Debug.WriteLine("My error massage");
                Debug.WriteLine(le.Message);
                Debug.WriteLine(le.StackTrace);
                return new MemoryStream();
            }
        }
    }
}
