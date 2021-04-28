using System;
using CoordinateNET;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var geo_1 = new GEO2d(34.5000, 135.0000);
            var ecef = geo_1.ConvertToECEF();

            var geo_2 = new GEO2d(34.6000, 135.1000);
            var enu_1 = geo_2.ConvertToENU(geo_1);
            var local_1 = new LocalRotationCoordinate2d(geo_2, geo_1, 30 / 180 * Math.PI);

            var geo_3 = new GEO2d(34.7000, 135.1500);

            var enu_2 = geo_3.ConvertToENU(geo_1);
            var local_2 = new LocalRotationCoordinate2d(geo_3, geo_1, 30 / 180 * Math.PI);

            double dis = enu_2.GetDistance2d(enu_1);

            System.Console.WriteLine("This is Debug Function");
        }
    }
}
