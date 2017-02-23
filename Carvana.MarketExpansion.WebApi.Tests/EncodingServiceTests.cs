using Carvana.MarketExpansion.WebApi.Models;
using Carvana.MarketExpansion.WebApi.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carvana.MarketExpansion.WebApi.Tests
{
    [TestClass]
    public class EncodingServiceTests
    {
        [TestMethod]
        public void Should_Encode_And_Decode()
        {
            IEncodingService encodingService = new EncodingService();
            var jwtHeader = new JwtHeader {typ = "JWT", alg = "HS256"};
            var encoded = encodingService.EncodeObject(jwtHeader);
            var decoded = encodingService.DecodeObject<JwtHeader>(encoded);

            Assert.AreEqual(jwtHeader.alg, decoded.alg);
            Assert.AreEqual(jwtHeader.typ, decoded.typ);
        }
    }
}
