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
            //var utc0 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            //var issueTime = DateTime.Now;
            //var issueTimeUtc = DateTime.UtcNow;

            //var iat = (int)issueTime.Subtract(utc0).TotalSeconds;
            //var iat2 = (int)issueTimeUtc.Subtract(utc0).TotalSeconds;


            IJwtEncodingService encodingService = new JwtEncodingService();
            var jwtHeader = new JwtHeader {typ = "JWT", alg = "HS256"};
            var encoded = encodingService.EncodeObject(jwtHeader);
            var decoded = encodingService.DecodeObject<JwtHeader>(encoded);

            Assert.AreEqual(jwtHeader.alg, decoded.alg);
            Assert.AreEqual(jwtHeader.typ, decoded.typ);
        }
    }
}
