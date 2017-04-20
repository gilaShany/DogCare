
using System;
using System.Collections.Generic;
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

    }
}
