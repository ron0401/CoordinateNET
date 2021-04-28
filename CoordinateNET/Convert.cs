using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateNET
{
    internal static class CoordinateConverter
    {
        private class EllipsoidConst
        {
            /// <summary>
            /// Equatorial radius
            /// </summary>
            public double a { get; set; }

            /// <summary>
            /// Reciprocal of flattening
            /// </summary>
            public double f_1 { get; set; }
            public double f { get { return 1.0 / f_1; } }

            public double e2 { get { return (1 / f_1) * (2.0 - (1 / f_1)); } }
        }

        private static Dictionary<GEO2d.TypeOfEllipsoid, EllipsoidConst> GetEllipsoidDictionary()
        {
            return new Dictionary<GEO2d.TypeOfEllipsoid, EllipsoidConst>()
            {
                {GEO2d.TypeOfEllipsoid.WGS84, new EllipsoidConst() { a = 6378137, f_1 = 298.257223563 }},
                {GEO2d.TypeOfEllipsoid.GRS80, new EllipsoidConst() { a = 6378137, f_1 = 298.257222101 }}
            };
        }
        internal static ECEF ConvertGEO2ECEF(GEO2d geo)
        {
            EllipsoidConst el;
            GetEllipsoidDictionary().TryGetValue(geo.Ellipsoid, out el);

            double b = Math.PI * geo.Latitude / 180.0;
            double l = Math.PI * geo.Longitude / 180.0;
            double N = el.a / Math.Sqrt(1.0 - el.e2 * Math.Pow(Math.Sin(b), 2.0));

            return new ECEF()
            {
                X = N * Math.Cos(b) * Math.Cos(l),
                Y = N * Math.Cos(b) * Math.Sin(l),
                Z = (N * (1.0 - el.e2)) * Math.Sin(b)
            };
        }
        internal static GEO2d ConvertECEF2GEO(ECEF ecef, GEO2d.TypeOfEllipsoid ellipsoid)
        {
            EllipsoidConst el;
            GetEllipsoidDictionary().TryGetValue(ellipsoid, out el);
            double p = Math.Sqrt(ecef.X * ecef.X + ecef.Y * ecef.Y);
            double r = Math.Sqrt(p * p + ecef.Z * ecef.Z);
            double mu = Math.Atan(ecef.Z / p * ((1.0 - el.f) + el.e2 * el.a / r));

            double B = Math.Atan((ecef.Z * (1.0 - el.f) + el.e2 * el.a * Math.Pow(Math.Sin(mu), 3)) / ((1.0 - el.f) * (p - el.e2 * el.a * Math.Pow(Math.Cos(mu), 3))));
            return new GEO2d()
            {
                Latitude = 180.0 * B / Math.PI,
                Longitude = 180.0 * Math.Atan2(ecef.Y, ecef.X) / Math.PI
            };
        }
        internal static GEO2d ConvertECEF2GEO(ECEF ecef)
        {
            return ConvertECEF2GEO(ecef, GEO2d.TypeOfEllipsoid.WGS84);
        }

        internal static EastAndNorth ConvertGEO2ENU(GEO2d geo, GEO2d datum)
        {
            var e           = geo.ConvertToECEF();
            var zero        = datum.ConvertToECEF();
            double delta_X  = e.X - zero.X;
            double delta_Y  = e.Y - zero.Y;
            double delta_Z  = e.Z - zero.Z;
            double lambda   = Angle2Radian(geo.Longitude);
            double phi      = Angle2Radian(geo.Latitude);
            var enu = new EastAndNorth
            {
                E = (-1 * sin(lambda))              * delta_X   + (cos(lambda))                   * delta_Y     + (0)           * delta_Z,
                N = (-1 * sin(phi) * cos(lambda))   * delta_X   + (-1 * sin(phi) * sin(lambda))   * delta_Y     + cos(phi)      * delta_Z
            };
            return enu;
        }

        private static double sin(double val) { return Math.Sin(val); }
        private static double cos(double val) { return Math.Cos(val); }

        internal static GEO2d ConvertENU2GEO(ENU2d enu)
        {
            var origin      = enu.Datum.ConvertToECEF();
            double lambda   = Angle2Radian(enu.Datum.Longitude);
            double phi      = Angle2Radian(enu.Datum.Latitude);

            var ecef = new ECEF()
            {
                X = (-1 * sin(lambda))  * enu.E     + (-1 * sin(phi) * cos(lambda))     * enu.N     + (cos(phi) * cos(lambda))  * enu.U  + origin.X,
                Y = (cos(lambda))       * enu.E     + (-1 * sin(phi) * sin(lambda))     * enu.N     + (cos(phi) * sin(lambda))  * enu.U  + origin.Y,
                Z = (0)                 * enu.E     + (cos(phi))                        * enu.N     + (sin(phi))                * enu.U  + origin.Z
            };

            return ConvertECEF2GEO(ecef, enu.Datum.Ellipsoid);
        }
        private static double Angle2Radian(double angle)
        {
            return angle * Math.PI / 180;
        }
    }
}
