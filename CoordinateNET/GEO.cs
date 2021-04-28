using System;
using System.Collections.Generic;

namespace CoordinateNET
{
    public class GEO : IGEO
    {
        public GEO(double latitude, double longitude, double altitude , TypeOfEllipsoid ellipsoid)
        {
            this.Longitude  = longitude;
            this.Latitude = latitude;
            this.Altitude = altitude;
            this.Ellipsoid = ellipsoid;
        }

        public GEO(double latitude, double longitude, double altitude) : this(latitude, longitude, altitude, TypeOfEllipsoid.WGS84)
        {
            // Nothing
        }

        public GEO(double latitude, double longitude) : this(latitude, longitude, 0, TypeOfEllipsoid.WGS84)
        {
            // Nothing
        }

        public GEO() : this(0, 0, 0, TypeOfEllipsoid.WGS84)
        {
            // Nothing
        }

        public ECEF ConvertToECEF() 
        {
            EllipsoidConst el;
            GetEllipsoidDictionary().TryGetValue(this.Ellipsoid, out el);

            double b = Math.PI * this.Latitude  / 180.0;
            double l = Math.PI * this.Longitude / 180.0;
            double N = el.a / Math.Sqrt(1.0 - el.e2 * Math.Pow(Math.Sin(b), 2.0));

            return new ECEF()
            {
                X = (N + this.Altitude) * Math.Cos(b) * Math.Cos(l),
                Y = (N + this.Altitude) * Math.Cos(b) * Math.Sin(l),
                Z = (N * (1.0 - el.e2) + this.Altitude) * Math.Sin(b)

            };
        }

        public static GEO ConvertFromECEF(ECEF ecef,TypeOfEllipsoid ellipsoid)
        {
            EllipsoidConst el;
            GetEllipsoidDictionary().TryGetValue(ellipsoid, out el);
            double p = Math.Sqrt(ecef.X * ecef.X + ecef.Y * ecef.Y);
            double r = Math.Sqrt(p * p + ecef.Z * ecef.Z);
            double mu = Math.Atan(ecef.Z / p * ((1.0 - el.f) + el.e2 * el.a / r));

            double B = Math.Atan((ecef.Z * (1.0 - el.f) + el.e2 * el.a * Math.Pow(Math.Sin(mu), 3)) / ((1.0 - el.f) * (p - el.e2 * el.a * Math.Pow(Math.Cos(mu), 3))));
            return new GEO() {
                Latitude = 180.0 * B / Math.PI,
                Longitude = 180.0 * Math.Atan2(ecef.Y, ecef.X) / Math.PI,
                Altitude = p * Math.Cos(B) + ecef.Z * Math.Sin(B) - el.a * Math.Sqrt(1.0 - el.e2 * Math.Pow(Math.Sin(B), 2))
            };
        }
        public static GEO ConvertFromECEF(ECEF ecef)
        {
            return ConvertFromECEF(ecef, TypeOfEllipsoid.WGS84);
        }

        public static ENU ConvertToENU(GEO geo, GEO datum) 
        {
            var e    = geo.ConvertToECEF();
            var zero = datum.ConvertToECEF();
            return new ENU
            {
                E = -1 * Math.Sin(geo.Longitude * Math.PI / 180) * (e.X - zero.X) + Math.Cos(geo.Longitude * Math.PI / 180) * (e.Y - zero.Y),
                N = -1 * Math.Sin(geo.Latitude * Math.PI / 180) * Math.Cos(geo.Longitude * Math.PI / 180) * (e.X - zero.X) - Math.Sin(geo.Latitude * Math.PI / 180) * Math.Sin(geo.Longitude * Math.PI / 180) * (e.Y - zero.Y) + Math.Cos(geo.Latitude * Math.PI / 180) * (e.Z - zero.Z),
                U = Math.Cos(geo.Latitude * Math.PI / 180) * Math.Cos(geo.Longitude * Math.PI / 180) * (e.X - zero.X) + Math.Cos(geo.Latitude) * Math.Sin(geo.Longitude * Math.PI / 180) * (e.Y - zero.Y) + Math.Sin(geo.Latitude * Math.PI / 180) * (e.Z - zero.Z),
                Datum = datum
            };
        }

        public double Longitude { get; set; } = 0;
        public double Latitude  { get; set; } = 0;
        public double Altitude  { get; set; } = 0;

        public TypeOfEllipsoid Ellipsoid { get; set; } = TypeOfEllipsoid.WGS84;

        public enum TypeOfEllipsoid
        {
            WGS84,
            GRS84
        }

        static Dictionary<TypeOfEllipsoid, EllipsoidConst> GetEllipsoidDictionary() 
        {
            return new Dictionary<TypeOfEllipsoid, EllipsoidConst>()
            {
                {TypeOfEllipsoid.WGS84, new EllipsoidConst() { a = 6378137, f_1 = 298.257223563 }},
                {TypeOfEllipsoid.GRS84, new EllipsoidConst() { a = 6378137, f_1 = 298.257222101 }}
            };
        }

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
    }

    public interface IGEO
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Altitude { get; set; }

    }
}
