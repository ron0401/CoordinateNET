using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateNET
{
    public interface ICoordinate2d 
    {
        public double GetDistance2d(ICoordinate2d coordinate);
    }

    public interface IPossibleConvertToGEO
    {
        public GEO2d ConvertToGEO();
    }

    public interface IPossibleConvertToECEF
    {
        public ECEF ConvertToECEF();
    }

    public interface ILocalCoordinate
    {
        public GEO2d Datum { get; set; }
    }

    public interface IGEO
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Altitude { get; set; }

        public ECEF ConvertToECEF();
        public ENU2d ConvertToENU(IGEO datum);
    }

    public interface IAngle
    {
        public double Value { get; set; }
    }
}
