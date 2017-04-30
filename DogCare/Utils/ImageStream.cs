using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Plugin.Media.Abstractions;
using System.IO;
using Android.Graphics;

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
                byte[] bytesOfResizeImage = ResizeImage(bytes, 200, 200);

                memStream.Position = 0;
                return Convert.ToBase64String(bytesOfResizeImage);
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

        public static byte[] ResizeImage(byte[] imageData, float width, float height)
        {

			return ResizeImageAndroid ( imageData, width, height );
        }

		
		public static byte[] ResizeImageAndroid (byte[] imageData, float width, float height)
		{
			// Load the bitmap
			Bitmap originalImage = BitmapFactory.DecodeByteArray (imageData, 0, imageData.Length);
			Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, false);

			using (MemoryStream ms = new MemoryStream())
			{
				resizedImage.Compress (Bitmap.CompressFormat.Jpeg, 100, ms);
				return ms.ToArray ();
			}
		}

    }
}
