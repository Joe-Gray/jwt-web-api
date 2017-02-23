using System;
using System.Security.Cryptography;
using Carvana.MarketExpansion.WebApi.Models;

namespace Carvana.MarketExpansion.WebApi.Services
{
    public class JwtService : IJwtService
    {
        private readonly string _secret = "OurLittleSecret";
        private readonly IJwtEncodingService _jwtEncodingService;

        public JwtService(IJwtEncodingService jwtEncodingService)
        {
            _jwtEncodingService = jwtEncodingService;
        }

        public DateTime EpochTime => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public string CreateToken(JwtPayload jwtPayload)
        {
            var header = CreateEncodedHeader();
            var encodedPayload = _jwtEncodingService.EncodeObject(jwtPayload);
            var headerAndPayload = $"{header}.{encodedPayload}";
            var signature = GenerateSignature(headerAndPayload);
            var token = $"{headerAndPayload}.{signature}";
            return token;
        }
        
        public JwtPayload CreateJwtPayload(User user, DateTime issuedAt, int tokenExpiration, string tokenType)
        {
            var iat = GetTotalSecondsSinceEpochTime(issuedAt);

            var payload = new JwtPayload
            {
                aud = "market-expansion",
                exp = tokenExpiration.ToString(),
                iat = iat.ToString(),
                iss = "market-expansion-authorization",
                nbf = iat.ToString(),
                jti = Guid.NewGuid().ToString().ToLower(),
                sub = user.Email,
                userEmail = user.Email,
                userId = user.Id,
                tokenType = tokenType
            };

            return payload;
        }

        private string CreateEncodedHeader()
        {
            var header = new JwtHeader { typ = "JWT", alg = "HS256" };
            var encoded = _jwtEncodingService.EncodeObject(header);
            return encoded;
        }

        private string GenerateSignature(string headerAndPayload)
        {
            var secretBytes = System.Text.Encoding.UTF8.GetBytes(_secret);
            var sha = new HMACSHA256(secretBytes);
            var bytesToHash = System.Text.Encoding.UTF8.GetBytes(headerAndPayload);
            var hash = sha.ComputeHash(bytesToHash);
            var signature = _jwtEncodingService.EncodeBytes(hash);
            return signature;
        }
        
        public bool DoesSignatureMatch(string jwToken)
        {
            var segments = jwToken.Split('.');
            var headerAndPayload = $"{segments[0]}.{segments[1]}";
            var signature = GenerateSignature(headerAndPayload);

            var signatureMatches = signature == segments[2];
            return signatureMatches;
        }

        public JwtPayload GetJwtPayload(string jwToken)
        {
            var segments = jwToken.Split('.');
            var jwtPayload = DecodePayload(segments[1]);
            return jwtPayload;
        }
        
        private JwtHeader DecodeHeader(string encodedHeader)
        {
            var jwtHeader = _jwtEncodingService.DecodeObject<JwtHeader>(encodedHeader);
            return jwtHeader;
        }
        
        private int GetTotalSecondsSinceEpochTime(DateTime currentUtcTime)
        {
            var totalSecondsSinceEpochTime = (int)currentUtcTime.Subtract(EpochTime).TotalSeconds;
            return totalSecondsSinceEpochTime;
        }

        private JwtPayload DecodePayload(string encodedPayload)
        {
            var jwtPayload = _jwtEncodingService.DecodeObject<JwtPayload>(encodedPayload);
            return jwtPayload;
        }
    }
}