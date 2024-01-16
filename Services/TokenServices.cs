using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using IquraStudyBE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using NuGet.Configuration;

namespace IquraStudyBE.Services;

[AllowAnonymous]
public class TokenServices : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public TokenServices(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public JwtSecurityToken CreateToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        _ = int.TryParse(_configuration["Jwt:TokenValidityInMinutes"], out int tokenValidityInMinutes);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: authClaims,
            expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid Token");

        return principal;
    }
    public string GetEmailFromToken()
    {
        try
        {
            // Get the JWT token from the authorization header
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"]
                .FirstOrDefault()?.Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                // Token is missing or invalid
                return null;
            }

            // Decode the JWT token to get the claims
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            // Extract relevant user information from claims
            var userEmail = jsonToken?.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            
            // Return user information
            return userEmail;
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., token decoding error) and log or return null
            // You might want to implement more robust error handling in a production environment
            return null;
        }
    }
}