﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateNET
{
    internal static class CoordinateConverter
    {
        class EllipsoidConst
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

        private static Dictionary<GEO.TypeOfEllipsoid, EllipsoidConst> GetEllipsoidDictionary()
        {
            return new Dictionary<GEO.TypeOfEllipsoid, EllipsoidConst>()
            {
                {GEO.TypeOfEllipsoid.WGS84, new EllipsoidConst() { a = 6378137, f_1 = 298.257223563 }},
                {GEO.TypeOfEllipsoid.GRS84, new EllipsoidConst() { a = 6378137, f_1 = 298.257222101 }}
            };
        }
        internal static ECEF ConvertGEO2ECEF(GEO geo)
        {
            EllipsoidConst el;
            GetEllipsoidDictionary().TryGetValue(geo.Ellipsoid, out el);

            double b = Math.PI * geo.Latitude / 180.0;
            double l = Math.PI * geo.Longitude / 180.0;
            double N = el.a / Math.Sqrt(1.0 - el.e2 * Math.Pow(Math.Sin(b), 2.0));

            return new ECEF()
            {
                X = (N + geo.Altitude) * Math.Cos(b) * Math.Cos(l),
                Y = (N + geo.Altitude) * Math.Cos(b) * Math.Sin(l),
                Z = (N * (1.0 - el.e2) + geo.Altitude) * Math.Sin(b)

            };
        }
        internal static GEO ConvertECEF2GEO(ECEF ecef, GEO.TypeOfEllipsoid ellipsoid)
        {
            EllipsoidConst el;
            GetEllipsoidDictionary().TryGetValue(ellipsoid, out el);
            double p = Math.Sqrt(ecef.X * ecef.X + ecef.Y * ecef.Y);
            double r = Math.Sqrt(p * p + ecef.Z * ecef.Z);
            double mu = Math.Atan(ecef.Z / p * ((1.0 - el.f) + el.e2 * el.a / r));

            double B = Math.Atan((ecef.Z * (1.0 - el.f) + el.e2 * el.a * Math.Pow(Math.Sin(mu), 3)) / ((1.0 - el.f) * (p - el.e2 * el.a * Math.Pow(Math.Cos(mu), 3))));
            return new GEO()
            {
                Latitude = 180.0 * B / Math.PI,
                Longitude = 180.0 * Math.Atan2(ecef.Y, ecef.X) / Math.PI,
                Altitude = p * Math.Cos(B) + ecef.Z * Math.Sin(B) - el.a * Math.Sqrt(1.0 - el.e2 * Math.Pow(Math.Sin(B), 2))
            };
        }
        internal static GEO ConvertECEF2GEO(ECEF ecef)
        {
            return ConvertECEF2GEO(ecef, GEO.TypeOfEllipsoid.WGS84);
        }

        internal static ENU ConvertGEO2ENU(GEO geo, GEO datum)
        {
            var e = geo.ConvertToECEF();
            var zero = datum.ConvertToECEF();
            return new ENU
            {
                E = -1 * Math.Sin(geo.Longitude * Math.PI / 180) * (e.X - zero.X) + Math.Cos(geo.Longitude * Math.PI / 180) * (e.Y - zero.Y),
                N = -1 * Math.Sin(geo.Latitude * Math.PI / 180) * Math.Cos(geo.Longitude * Math.PI / 180) * (e.X - zero.X) - Math.Sin(geo.Latitude * Math.PI / 180) * Math.Sin(geo.Longitude * Math.PI / 180) * (e.Y - zero.Y) + Math.Cos(geo.Latitude * Math.PI / 180) * (e.Z - zero.Z),
                U = Math.Cos(geo.Latitude * Math.PI / 180) * Math.Cos(geo.Longitude * Math.PI / 180) * (e.X - zero.X) + Math.Cos(geo.Latitude) * Math.Sin(geo.Longitude * Math.PI / 180) * (e.Y - zero.Y) + Math.Sin(geo.Latitude * Math.PI / 180) * (e.Z - zero.Z),
                Datum = datum
            };
        }
        internal static GEO ConvertENU2GEO(GEO geo, GEO datum)
        {
            throw new NotImplementedException();
        }
    }
}
