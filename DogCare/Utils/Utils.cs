
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace DogCare.Utils
{
    public class Utils
    {
        public static double GetDistanceInMeters(Position pos1, Position pos2)
        {
            // returns the distance between two positions in meters
            var R = 6371; // Radius of the earth in km
            var dLat = deg2rad(pos2.Latitude - pos1.Latitude);  // deg2rad below
            var dLon = deg2rad(pos2.Longitude - pos1.Longitude);
            var a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(deg2rad(pos1.Latitude)) * Math.Cos(deg2rad(pos2.Latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
                ;
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
            return d * 1000;
        }
    
        public static double deg2rad(double deg) {
            return deg * (Math.PI / 180);
        }

        public static double KPHtoMPS(double kph)
        {
            // converts speed from Kilometer/Hour to Meter/Second
            return kph / 3.6;
        }

        public static string ConvertPositionsListToString (List<Position> list)
        {
            string listAsString;
            if (list != null)
            {
                // Build a string representing the poitions in the list
                StringBuilder sb = new StringBuilder();
                foreach (Position p in list)
                {
                    sb.Append(p.Latitude);
                    sb.Append(';');
                    sb.Append(p.Longitude);
                    sb.Append(';');
                }
                // Trim the last semi-colon, so that we have "1;2;3" instead of "1;2;3;"
                listAsString = sb.ToString().TrimEnd(';');
            }
            else
            {
                // Assign null to the string to preserve the fact that the list was null
                listAsString = null;
            }

            return listAsString;
        }

        public static List<Position> convertPositionsStringToList (string positionString)
        {
            List<Position> listOfPositions = new List<Position>();
            if (!String.IsNullOrEmpty(positionString))
            {
                // Split on ';' to get the individual doubles
                string[] pStrings = positionString.Split(';');
                int len = pStrings.Length;
                // Create a List
                List<Position> myPsotitions = new List<Position>();
                for (int i = 0; i < len-1; i=i+2)
                { 
                    double latitude = Convert.ToDouble(pStrings[i]);
                    double Longitude = Convert.ToDouble(pStrings[i+1]);
                    Position p = new Position(latitude, Longitude);
                    // Store each in the same order they were retured.
                    listOfPositions.Add(p);
                }
            }
            // Empty string would split to one string if we didn't handle it separately, resulting in the wrong array.
            else
            {
                listOfPositions = null;
            }

            return listOfPositions;
        }


        public static string ConvertFileToString(MediaFile file)
        {
            var stream = file.GetStream();
            file.Dispose();
            Byte[] bytes = new Byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            //StreamReader reader = new StreamReader(stream);
            //return (reader.ReadToEnd());
        }

        public static Stream ConvertStringToStream (string s)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(s);
            MemoryStream stream = new MemoryStream(byteArray);
            return stream;
        }

    }
}
