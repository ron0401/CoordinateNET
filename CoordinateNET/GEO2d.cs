using System;
using System.Collections.Generic;

namespace CoordinateNET
{
    public class GEO2d :IPossibleConvertToECEF
    {
        public GEO2d(double latitude, double longitude, TypeOfEllipsoid ellipsoid)
        {
            this.Longitude = new Longitude(longitude);
            this.Latitude = new Latitude(latitude);
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
        
        public Longitude Longitude { get; set; } = new Longitude();
        public Latitude Latitude { get; set; } = new Latitude();



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
    public class Latitude : IAngle
    {
        public Latitude(double value)
        {
            this.Value = value;
        }
        public Latitude()
        {

        }
        public enum TypeOfLatitude 
        {
            North, South
        }
        public TypeOfLatitude LatitudeType { get; set; }
        public double Value { get; set; }
    }

    public class Longitude : IAngle
    {
        public Longitude(double value) 
        {
            this.Value = value;
        }
        public Longitude()
        {
            
        }
        public enum TypeOfLongitude 
        {
            East, West
        }
        public TypeOfLongitude LongitudeType { get; set; } = TypeOfLongitude.East;
        public double Value { get; set; } = 0;

    }
}
