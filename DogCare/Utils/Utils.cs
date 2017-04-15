
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace DogCare.Utils
{
    class Utils
    {
        public static double GetAngle(Position a, Position b, Position c)
        {
            double p12 = GetDistance(a, b);
            double p13 = GetDistance(a, c);
            double p23 = GetDistance(b, c);
            //arccos((P122 + P132 - P232) / (2 * P12 * P13))
            if (p12 == 0 || p13 == 0)
            {
                return 0;
            }
            else
            {
                return Math.Acos((Math.Pow(p12, 2) + Math.Pow(p13, 2) - Math.Pow(p23, 2)) / (2 * p12 * p13)) * 180 / Math.PI;
            }
        }

        public static double GetDistance(Position pos1, Position pos2)
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

    }
}
