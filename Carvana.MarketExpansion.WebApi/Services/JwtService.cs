using Carvana.MarketExpansion.WebApi.Models;

namespace Carvana.MarketExpansion.WebApi.Services
{
    public class JwtService : IJwtService
    {
        private readonly IEncodingService _encodingService;

        public JwtService(IEncodingService encodingService)
        {
            _encodingService = encodingService;
        }

        private string CreateEncodedHeader()
        {
            var header = new JwtHeader {typ = "JWT", alg = "HS256"};
            var encoded = _encodingService.EncodeObject(header);
            return encoded;
        }

        private JwtHeader DecodeHeader(string encodedHeader)
        {
            var jwtHeader = _encodingService.DecodeObject<JwtHeader>(encodedHeader);
            return jwtHeader;
        }
    }
}