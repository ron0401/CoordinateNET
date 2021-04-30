using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateNET
{
    public abstract class LocalCoordinate2d :Vector2d, ICoordinate2d, ILocalCoordinate
    {
        public GEO2d Datum { get; set; } = new GEO2d();
        public ECEF DatumECEF
        {
            get
            {
                return Datum.ConvertToECEF();
            }
            set
            {
                var el = this.Datum.Ellipsoid;
                this.Datum = CoordinateConverter.ConvertECEF2GEO(value);
                this.Datum.Ellipsoid = el;
            }
        }

        public double GetDistance2d(ICoordinate2d coordinate)
        {
            if (coordinate.GetType() != this.GetType())
            {
                throw new TypeAccessException();
            }

            if (!(coordinate is ILocalCoordinate))
            {
                throw new Exception();
            }

            if (!(this.Datum.Equals(((ILocalCoordinate)coordinate).Datum)))
            {
                throw new Exception();
            }
            return this.GetDistance((Vector)coordinate);
        }
    }
    public class LocalRotationCoordinate2d : LocalCoordinate2d, IPossibleConvertToGEO, IPossibleConvertToECEF
    {
        public ENU2d ENU
        {
            get
            {
                double y = this.Y;
                double x = this.X;
                double angle = -1 * Angle2Radian(_RotateAngleDegree);
                double e = Math.Cos(angle) * x + Math.Sin(angle) * y;
                double n = -1 * Math.Sin(angle) * x + Math.Cos(angle) * y;
                return new ENU2d(e,n,this.Datum);       
            }
        }
        private double _RotateAngleDegree = 0;
        public double RotateAngleDegree 
        { 
            get 
            { 
                return _RotateAngleDegree; 
            } 
            set 
            { 
                _RotateAngleDegree = NormalizationAngle(value); 
            } 
        }
        public double RotateAngleRadian 
        { 
            get 
            { 
                return Angle2Radian(_RotateAngleDegree) ; 
            } 
            set 
            { 
                _RotateAngleDegree = Radian2Angle(NormalizationAngle(value)); 
            } 
        }

        private double NormalizationAngle(double angle) 
        {
            const double th_H = 360; 
            const double th_L = 0;
            while (angle > th_H)
            {
                angle =- th_H;
            }
            while (angle < th_L)
            {
                angle =+ th_H;
            }

            return angle;
        }
        private static double Angle2Radian(double angle)
        {
            return angle * Math.PI / 180;
        }
        private static double Radian2Angle(double radian)
        {
            return radian / Math.PI * 180;
        }
        private void SetDatum(string[] datum)
        {
            if (datum.Length != 3)
            {
                throw new Exception();
            }
            SetDatum(double.Parse(datum[0]), double.Parse(datum[1]), double.Parse(datum[2]));
        }
        private void SetDatum(double[] datum)
        {
            if (datum.Length != 3)
            {
                throw new Exception();
            }
            SetDatum(datum[0], datum[1], datum[2]);
        }
        private void SetDatum(GEO2d datum, double rotation)
        {
            this.Datum = datum;
            this.RotateAngleRadian = rotation;
        }
        private void SetDatum(double latitude, double longitude, double rotation)
        {
            this.Datum.Latitude = latitude;
            this.Datum.Longitude = longitude;
            this.RotateAngleRadian = rotation;
        }
        private bool IsZero(double value)
        {
            if (value < 0.000001 && value > -0.000001)
            {
                return true;
            }
            return false;
        }

        public GEO2d ConvertToGEO()
        {
            return this.ENU.ConvertToGEO();
        }

        public ECEF ConvertToECEF()
        {
            return this.ConvertToGEO().ConvertToECEF();
        }
    }
}
