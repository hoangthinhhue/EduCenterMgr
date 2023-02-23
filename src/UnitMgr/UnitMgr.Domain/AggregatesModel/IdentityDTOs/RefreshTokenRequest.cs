// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace UnitMgr.Domain.AggregatesModel.IdentityDTOs;

public class RefreshTokenRequest
{
    public RefreshTokenRequest(string token,string refreshToken)
    {
        Token = token;
        RefreshToken = refreshToken;
    }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}
