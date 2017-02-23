using System;
using System.Security.Cryptography;
using Carvana.MarketExpansion.WebApi.Data;
using Carvana.MarketExpansion.WebApi.Models;

namespace Carvana.MarketExpansion.WebApi.Services
{
    public class JwtService : IJwtService
    {
        private readonly string _secret = "OurLittleSecret";
        private readonly int _accessTokenLifespanMinutes = 20;
        private readonly int _refreshTokenLifespanDays = 14;
        private readonly DateTime _epochTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        private readonly IJwtEncodingService _encodingService;
        private readonly IAccountRepository _accountRepository;

        public JwtService(
            IJwtEncodingService encodingService, 
            IAccountRepository accountRepository)
        {
            _encodingService = encodingService;
            _accountRepository = accountRepository;
        }

        public string CreateAccessToken(string email)
        {
            var header = CreateEncodedHeader();
            var user = _accountRepository.GetUserByUserName(email);
            var payload = CreateEncodedPayload(user);
            var headerAndPayload = $"{header}.{payload}";
            var signature = GenerateSignature(headerAndPayload);
            var token = $"{headerAndPayload}.{signature}";
            return token;
        }

        //public string CreateRefreshToken(string email)
        //{
        //    var header = CreateEncodedHeader();
        //    var user = _accountRepository.GetUserByUserName(email);
        //    var payload = CreateEncodedPayload(user);
        //    var headerAndPayload = $"{header}.{payload}";
        //    var signature = GenerateSignature(headerAndPayload);
        //    var token = $"{headerAndPayload}.{signature}";
        //    return token;
        //}

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

        private string CreateEncodedPayload(User user)
        {
            var currentUtcTime = DateTime.UtcNow;
            var iat = GetTotalSecondsSinceEpochTime(currentUtcTime);
            var exp = GetAccessTokenExpirationInSeconds(currentUtcTime);

            var payload = new JwtPayload
            {
                aud = "market-expansion",
                exp = exp.ToString(),
                iat = iat.ToString(),
                iss = "market-expansion-authorization",
                nbf = iat.ToString(),
                jti = Guid.NewGuid().ToString().ToLower(),
                sub = user.Email,
                userEmail = user.Email,
                userId = user.Id,
                userSecurityClaims = user.SecurityClaims
            };

            var encoded = _encodingService.EncodeObject(payload);
            return encoded;
        }

        private int GetTotalSecondsSinceEpochTime(DateTime currentUtcTime)
        {
            var totalSecondsSinceEpochTime = (int)currentUtcTime.Subtract(_epochTime).TotalSeconds;
            return totalSecondsSinceEpochTime;
        }

        private int GetAccessTokenExpirationInSeconds(DateTime currentUtcTime)
        {
            var expirationInSeconds =
                (int) currentUtcTime.AddMinutes(_accessTokenLifespanMinutes).Subtract(_epochTime).TotalSeconds;

            return expirationInSeconds;
        }

        private int GetRefreshTokenExpirationInSeconds(DateTime currentUtcTime)
        {
            var expirationInSeconds =
                (int)currentUtcTime.AddDays(_refreshTokenLifespanDays).Subtract(_epochTime).TotalSeconds;

            return expirationInSeconds;
        }

        private DateTime EpochTime => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        private JwtPayload DecodePayload(string encodedPayload)
        {
            var jwtPayload = _encodingService.DecodeObject<JwtPayload>(encodedPayload);
            return jwtPayload;
        }

        private string GenerateSignature(string headerAndPayload)
        {
            var secretBytes = System.Text.Encoding.UTF8.GetBytes(_secret);
            var sha = new HMACSHA256(secretBytes);
            var bytesToHash = System.Text.Encoding.UTF8.GetBytes(headerAndPayload);
            var hash = sha.ComputeHash(bytesToHash);
            var signature = _encodingService.EncodeBytes(hash);
            return signature;
        }
    }
}