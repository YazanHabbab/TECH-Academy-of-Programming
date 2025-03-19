using System.IdentityModel.Tokens.Jwt;
using TECH_Academy_of_Programming.Models;

namespace TECH_Academy_of_Programming.Interfaces
{
    public interface IAuthService
    {
        Task<JwtSecurityToken> CreateJwtToken(User user);
    }
}