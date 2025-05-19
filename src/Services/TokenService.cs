using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using ECommerce.src.Interfaces;
using ECommerce.src.Models;

using Microsoft.IdentityModel.Tokens;

namespace ECommerce.src.Services
{
    /// <summary>
    /// Service responsible for generating JWT tokens for authenticated users.
    /// Implements the ITokenService interface.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;

        /// <summary>
        /// Initializes a new instance of the TokenService class.
        /// Retrieves the signing key from the configuration.
        /// </summary>
        /// <param name="config">Application configuration containing JWT settings.</param>
        public TokenService(IConfiguration config)
        {
            _config = config;
            var signingKey = _config["JWT:SignInKey"] ?? throw new ArgumentNullException("Key not found");
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        }

        /// <summary>
        /// Generates a JWT token for the specified user and role.
        /// </summary>
        /// <param name="user">The user for whom the token is generated.</param>
        /// <param name="role">The user's role.</param>
        /// <returns>A JWT token as a string.</returns>
        public string GenerateToken(User user, string role)
        {
            var claims = new List<Claim>{
                new(JwtRegisteredClaimNames.Email, user.Email!),
                new(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new(ClaimTypes.Role, role),
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"],
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}