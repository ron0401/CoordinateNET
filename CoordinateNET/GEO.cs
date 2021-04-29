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
            return CoordinateConverter.ConvertGEO2ECEF(this);
        }

        public ENU ConvertToENU(IGEO datum)
        {
            return CoordinateConverter.ConvertGEO2ENU(this,(GEO)datum);
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
    }

    public interface IGEO
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Altitude { get; set; }

        public ECEF ConvertToECEF();
        public ENU ConvertToENU(IGEO datum);
    }
}
