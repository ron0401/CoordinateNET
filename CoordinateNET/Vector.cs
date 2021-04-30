using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateNET
{
    public abstract class Vector
    {
        public int Dimension { get { return Data.Length; } }

        public abstract double[] Data { get; set; }

        internal static double[] sum(double[] d1, double[] d2)
        {
            if (d1.Length != d2.Length)
            {
                throw new Exception();
            }
            double[] result = new double[d1.Length];
            for (int i = 0; i < d1.Length; i++)
            {
                result[i] = d1[i] + d2[i];
            }
            return result;
        }
        internal static double[] diff(double[] d1, double[] d2)
        {
            if (d1.Length != d2.Length)
            {
                throw new Exception();
            }
            double[] result = new double[d1.Length];
            for (int i = 0; i < d1.Length; i++)
            {
                result[i] = d1[i] - d2[i];
            }
            return result;
        }
        internal double[] getUnitVector() 
        {
            double len = this.Length;
            double[] d = new double[this.Data.Length]; 
            for (int i = 0; i < this.Data.Length; i++)
            {
                d[i] = this.Data[i] * len;
            }
            return d;
        }
        public double Length 
        { 
            get
            {
                double r = 0;
                foreach (var f in this.Data)
                {
                    r = r + Math.Pow(f , 2);
                }
                return Math.Sqrt(r);
            }
            set
            {
                double len = value;
                len = len / this.Length;
                for (int i = 0; i < this.Data.Length; i++)
                {
                    this.Data[i] = this.Data[i] * len;
                }
             }
        }
        internal static double GetV2VDistance(Vector v1, Vector v2)
        {
            return diff(v1.Data, v2.Data).Length;
        }
        internal double GetDistance(Vector v) 
        {
            return GetV2VDistance(this, v);
        }
    }

    public class Vector2d:Vector
    {
        public override double[] Data { get; set; } = new double[] { 1, 1 };
        public double X { get { return Data[0]; } set { Data[0] = value; } }
        public double Y { get { return Data[1]; } set { Data[1] = value; } }

    }
    public class Vector3d : Vector2d
    {
        public override double[] Data { get; set; } = new double[] { 1, 1 ,1 };

        public double Z { get { return Data[2]; } set { Data[2] = value; } }

        public static Vector3d Sum(Vector3d v1, Vector3d v2)
        {
            return new Vector3d()
            {
                Data = sum(v1.Data, v2.Data)
            };
        }
        public static Vector3d Diff(Vector3d v1, Vector3d v2)
        {
            return new Vector3d()
            {
                Data = diff(v1.Data, v2.Data)
            };
        }
        public Vector3d GetUnitVector()
        {
            double len = this.Length;
            return new Vector3d()
            {
                Data = this.getUnitVector()
            };
        }

        internal static double GetDistance(Vector3d v1, Vector3d v2)
        {
            return Vector3d.Diff(v1, v2).Length;
        }
    }

}
