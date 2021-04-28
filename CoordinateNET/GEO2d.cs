using System;
using System.Collections.Generic;

namespace CoordinateNET
{
    public class GEO2d :IPossibleConvertToECEF
    {
        public GEO2d(double latitude, double longitude, TypeOfEllipsoid ellipsoid)
        {
            this.Longitude = longitude;
            this.Latitude = latitude;
            this.Ellipsoid = ellipsoid;
        }


        public GEO2d(double latitude, double longitude) : this(latitude, longitude, TypeOfEllipsoid.WGS84)
        {
            // Nothing
        }

        public GEO2d() : this(0, 0, TypeOfEllipsoid.WGS84)
        {
            // Nothing
        }

        public ECEF ConvertToECEF()
        {
            return CoordinateConverter.ConvertGEO2ECEF(this);
        }

        public ENU2d ConvertToENU(GEO2d datum)
        {
            return new ENU2d(this, datum);
        }

        public double Longitude { get; set; } = 0;
        public double Latitude { get; set; } = 0;

        public TypeOfEllipsoid Ellipsoid { get; set; } = TypeOfEllipsoid.WGS84;

        public enum TypeOfEllipsoid
        {
            WGS84,
            GRS80
        }

        public double GetDistance(GEO2d geo)
        {
            throw new NotImplementedException();
        }
    }
}
