using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateNET
{
    public class ENU : IENU
    {
        public double E { get; set; }
        public double N { get; set; }
        public double U { get; set; }
        public GEO Datum { get; set; } = new GEO();
        public ECEF DatumECEF 
        {
            get
            {
                return Datum.ConvertToECEF();
            }
            set
            {
                var el = this.Datum.Ellipsoid;
                this.Datum = GEO.ConvertFromECEF(value);
                this.Datum.Ellipsoid = el;
            }
        }
    }

    public interface IENU 
    {
        public double E { get; set; }
        public double N { get; set; }
        public double U { get; set; }
       
        public GEO Datum { get; set; }
    }
}
