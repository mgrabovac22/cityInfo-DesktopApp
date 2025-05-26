using BusinessLogicLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class CommunalServiceTest
    {
        [TestMethod]
        public void GetLocationInfoByAddress_PassingTheStreetNameAndGettingObject_ObjectIsNotNullAndIsValid()
        {
            CommunalService service = new CommunalService();
            string address = "Trenkova ulica";

            var result = service.GetLocationInfoByAddress(address);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetAddressFromApiAsync_ValidCoordinates_ReturnsValidResponse()
        {
            CommunalService service = new CommunalService();
            double lat = 45.8150;
            double lng = 15.9819;

            var result = await service.GetAddressFromApiAsync(lat, lng);

            Assert.IsNotNull(result);
        }
    }
}
