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

    }

    public interface IECEF 
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }
}
