using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateNET
{
    public class LocalCoordinate : ILocalCoordinate
    {
        public double X 
        {
            get 
            {
                if (IsZero(_RotateAngleDegree))
                {
                    return this.ENU.E;
                }
                double angle = -1 * Angle2Radian(_RotateAngleDegree);
                return Math.Cos(angle) * this.ENU.E + Math.Sin(angle) * this.ENU.N ;
            }
            set 
            {
                double angle = Angle2Radian(_RotateAngleDegree);
                double y = this.Y;
                double x = value;
                this.ENU.E = Math.Cos(angle) * x + Math.Sin(angle) * y;
                this.ENU.N = -1 * Math.Sin(angle) * x + Math.Cos(angle) * y;
            } 
        }
        public double Y 
        {
            get 
            {
                if (IsZero(_RotateAngleDegree))
                {
                    return this.ENU.N;
                }
                double angle = -1 * Angle2Radian(_RotateAngleDegree);
                return -1 * Math.Sin(angle) * this.ENU.E + Math.Cos(angle) * this.ENU.N;
            }
            set 
            {
                double angle = Angle2Radian(_RotateAngleDegree);
                double y = value;
                double x = this.X;
                this.ENU.E = Math.Cos(angle) * x + Math.Sin(angle) * y;
                this.ENU.N = -1 * Math.Sin(angle) * x + Math.Cos(angle) * y;
            } 
        }
        public double Z { get { return ENU.U; } set { ENU.U = value; } }

        public GEO Datum 
        { 
            get 
            {
                return this.ENU.Datum ;
            }
            set 
            {
                this.ENU.Datum = value ;
            } 
        }

        private bool IsZero(double value)
        {
            if (value < 0.000001 && value > -0.000001)
            {
                return true;
            }
            return false;
        }

        public ENU ENU { get; set; } = new ENU();

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

        public void SetDatum(double[] datum) 
        {
            if (datum.Length != 3)
            { 
                throw new Exception();
            }
            SetDatum(datum[0], datum[1], datum[2]);
        }
        public void SetDatum(GEO datum, double rotation)
        {
            this.Datum = datum;
            this.RotateAngleRadian = rotation;
        }
        public void SetDatum(double latitude, double longitude, double rotation) 
        {
            this.Datum.Latitude = latitude;
            this.Datum.Longitude = longitude;
            this.RotateAngleRadian = rotation;
        }
    }

    public interface ILocalCoordinate 
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public double RotateAngleDegree { get; set; }
        public double RotateAngleRadian { get; set; }
    }
}
