using System;
using System.Linq;
using System.Security.Cryptography;
using Carvana.MarketExpansion.WebApi.Data;
using Carvana.MarketExpansion.WebApi.Models;

namespace Carvana.MarketExpansion.WebApi.Services
{
    public class JwtService : IJwtService
    {
        private readonly string _secret = "OurLittleSecret";
        private readonly IJwtEncodingService _jwtEncodingService;
        private readonly IAccountRepository _accountRepository;

        public JwtService(
            IJwtEncodingService jwtEncodingService, 
            IAccountRepository accountRepository)
        {
            _jwtEncodingService = jwtEncodingService;
            _accountRepository = accountRepository;
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
        
        public JwtPayload CreateJwtPayload(User user, DateTime issuedAt, int tokenExpiration, JwTokenType tokenType)
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
                userId = user.SecurityUserGuid.ToString(),
                tokenType = tokenType.ToString()
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
        
        public bool IsSignatureValid(string jwToken)
        {
            var segments = jwToken.Split('.');

            if (segments.Length != 3)
            {
                return false;
            }

            var headerAndPayload = $"{segments[0]}.{segments[1]}";
            var signature = GenerateSignature(headerAndPayload);

            var signatureMatches = signature == segments[2];
            return signatureMatches;
        }

        public bool IsTokenExpired(string jwToken)
        {
            var jwtPayload = GetJwtPayload(jwToken);
            var currentTotalSeconds = GetTotalSecondsSinceEpochTime(DateTime.UtcNow);

            var isTokenExpired = int.Parse(jwtPayload.exp) < currentTotalSeconds;
            return isTokenExpired;
        }

        public bool IsTokenRevoked(string jwToken)
        {
            var jwtPayload = GetJwtPayload(jwToken);
            var user = _accountRepository.GetUserByEmail(jwtPayload.userEmail);

            var isTokenRevoked = user.RefreshTokenId == null;
            return isTokenRevoked;
        }

        public bool IsAnyClaimInToken(string jwToken, string[] claims)
        {
            var jwtPayload = GetJwtPayload(jwToken);

            foreach (var claim in claims)
            {
                if (jwtPayload.userSecurityClaims.Any(c => c == claim))
                {
                    return true;
                }
            }

            return false;
        }

        public JwtPayload GetJwtPayload(string jwToken)
        {
            var segments = jwToken.Split('.');
            var jwtPayload = DecodePayload(segments[1]);
            return jwtPayload;
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