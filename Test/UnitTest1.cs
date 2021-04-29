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
            var geo_1   = new GEO(35.686536, 139.756921, 0);
            var ecef    = geo_1.ConvertToECEF();
            var geo_2   = ecef.ConvertToGEO();

            Assert.IsTrue(System.Math.Abs(geo_1.Latitude - geo_2.Latitude) < 0.0000001);
            Assert.IsTrue(System.Math.Abs(geo_1.Longitude - geo_2.Longitude) < 0.0000001);
            Assert.IsTrue(System.Math.Abs(geo_1.Altitude - geo_2.Altitude) < 0.0000001);
        }

        [TestMethod]
        public void ConvertGEO2ENU_and_reconvert()
        {
            var geo_1   = new GEO(35.686536, 139.756921, 0);
            var geo_2   = new GEO(35.680793, 139.758468, 0);
            var enu     = geo_2.ConvertToENU(geo_1);
            var geo_3   = enu.ConvertToGEO();

            Assert.IsTrue(System.Math.Abs(geo_3.Latitude - geo_2.Latitude) < 0.001);
            Assert.IsTrue(System.Math.Abs(geo_3.Longitude - geo_2.Longitude) < 0.001);
            Assert.IsTrue(System.Math.Abs(geo_3.Altitude - geo_2.Altitude) < 1);
        }
    }
}
