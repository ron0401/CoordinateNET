using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateNET
{
    public class ENU2d : LocalCoordinate2d, IPossibleConvertToGEO, IPossibleConvertToECEF
    {
        public ENU2d(double east, double north, GEO2d datum) 
        {
            this.E = east;
            this.N = north;
            this.Datum = datum;
        }
        public double E { get { return X; } set { X = value; } }
        public double N { get { return Y; } set { Y = value; } }
        public double U { get { return 0; } }

        public ECEF ConvertToECEF()
        {
            return this.ConvertToGEO().ConvertToECEF();
        }

        public GEO2d ConvertToGEO() 
        {
            return CoordinateConverter.ConvertENU2GEO(this);
        }


    }
}
