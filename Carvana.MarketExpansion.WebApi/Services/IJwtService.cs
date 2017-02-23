﻿using System;
using Carvana.MarketExpansion.WebApi.Models;

namespace Carvana.MarketExpansion.WebApi.Services
{
    public interface IJwtService
    {
        DateTime EpochTime { get; }
        string CreateToken(JwtPayload jwtPayload);
        JwtPayload CreateJwtPayload(User user, DateTime issuedAt, int tokenExpiration, string tokenType);
        bool IsSignatureValid(string jwToken);
        bool IsTokenExpired(string jwToken);
        JwtPayload GetJwtPayload(string jwToken);
    }
}