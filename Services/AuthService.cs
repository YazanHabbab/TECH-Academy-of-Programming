using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TECH_Academy_of_Programming.Helper;
using TECH_Academy_of_Programming.Interfaces;
using TECH_Academy_of_Programming.Models;


namespace TECH_Academy_of_Programming.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly JWT _jwt;
        public AuthService(IOptions<JWT> jwt, UserManager<User> userManager)
        {
            _jwt = jwt.Value;
            _userManager = userManager;
        }

        public async Task<JwtSecurityToken> CreateJwtToken(User user)
        {
            // Get user roles
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            // Get user roles from the claims of the user
            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            // Create claims to save to jwt
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(roleClaims);

            // Get the jwt key from jwt configuration
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));

            // Hash user credentials using the key
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var JwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.ToLocalTime().AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials
                );

            return JwtSecurityToken;
        }
    }
}