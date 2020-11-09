
using Microsoft.IdentityModel.Tokens;
using PasswordManagerMobile.Models;
using PasswordManagerMobile;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManagerApp.Handlers
{
    public  class JwtHelper
    {



        private readonly AppSettings _config;
        
        public JwtHelper()
        {
            _config = App.AppSettings;
        }
        public bool ValidateToken(AccessToken accessToken,out ClaimsPrincipal claimsPrincipal)
        {
            
            RSA rsa = RSA.Create();
            rsa.FromXmlString(_config.PublicKey);
            var keyBytes = Encoding.UTF8.GetBytes(_config.SecretEncryptionKey);
            var symmetricSecurityKey = new SymmetricSecurityKey(keyBytes);

            var tokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new RsaSecurityKey(rsa),
                TokenDecryptionKey = symmetricSecurityKey,
                ValidateIssuer = true,
                ValidIssuer = _config.Issuer,
                ValidateAudience = true,
                ValidAudience = _config.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero

            };


            var handler = new JwtSecurityTokenHandler();
            claimsPrincipal = null;
            

            try
            {
                claimsPrincipal = handler.ValidateToken(
                accessToken.JwtToken,
                tokenValidationParameters,
                out SecurityToken securityToken);
            }
            catch (Exception)
            {
                return false;
            }

            
            return true;
        }
    }
}
