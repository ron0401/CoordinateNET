using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateNET
{
    public class ECEF : Vector3d, IPossibleConvertToGEO
    {

        public GEO2d ConvertToGEO(GEO2d.TypeOfEllipsoid ellipsoid) 
        {
            return CoordinateConverter.ConvertECEF2GEO(this, ellipsoid);
        }
        public GEO2d ConvertToGEO()
        {
            return CoordinateConverter.ConvertECEF2GEO(this);
        }

        public double GetDistance(ICoordinate2d coordinate)
        {
            if (coordinate.GetType() == typeof(ECEF))
            {
                return GetDistance((ECEF)coordinate, this);
            }
            if (coordinate is IPossibleConvertToECEF)
            {
                return GetDistance(((IPossibleConvertToECEF)coordinate).ConvertToECEF(), this);
            }
            throw new Exception();
        }
    }
}
