using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoordinateNET;

namespace Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ConvertGEO2ECEF_and_reconvert()
        {
            var geo_1   = new GEO2d(35.686536, 139.756921);
            var ecef    = geo_1.ConvertToECEF();
            var geo_2   = ecef.ConvertToGEO();

            Assert.IsTrue(System.Math.Abs(geo_1.Latitude.Value - geo_2.Latitude.Value) < 0.0000001);
            Assert.IsTrue(System.Math.Abs(geo_1.Longitude.Value - geo_2.Longitude.Value) < 0.0000001);
        }

        [TestMethod]
        public void ConvertGEO2ENU_and_reconvert()
        {
            var geo_1   = new GEO2d(35.686536, 139.756921);
            var geo_2   = new GEO2d(35.680793, 139.758468);
            var enu     = geo_2.ConvertToENU(geo_1);
            var geo_3   = enu.ConvertToGEO();

            Assert.IsTrue(System.Math.Abs(geo_3.Latitude.Value - geo_2.Latitude.Value) < 0.001);
            Assert.IsTrue(System.Math.Abs(geo_3.Longitude.Value - geo_2.Longitude.Value) < 0.001);

        }
        [TestMethod]
        public void CycleConvert()
        {
            var geo_1 = new GEO2d(35.686536, 139.756921);
            var geo_2 = new GEO2d(35.680793, 139.758468);
            var enu_0 = geo_2.ConvertToENU(geo_1);
            var enu_target = enu_0;
            for (int i = 0; i < 1000; i++)
            {
                var g = enu_target.ConvertToGEO();
                enu_target = g.ConvertToENU(geo_1);
            }
            Assert.IsTrue(System.Math.Abs(enu_0.E - enu_target.E) < 0.01);
            Assert.IsTrue(System.Math.Abs(enu_0.N - enu_target.N) < 0.01);

        }
    }
}
