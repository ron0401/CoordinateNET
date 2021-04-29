using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateNET
{
    public class ECEF : IECEF
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public GEO ConvertToGEO(GEO.TypeOfEllipsoid ellipsoid) 
        {
            return CoordinateConverter.ConvertECEF2GEO(this, ellipsoid);
        }
        public GEO ConvertToGEO()
        {
            return CoordinateConverter.ConvertECEF2GEO(this);
        }

    }

    public interface IECEF 
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }
}
