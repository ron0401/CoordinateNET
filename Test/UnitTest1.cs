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
    }
}
