using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using netcoreauth.model;

namespace netcoreauth.api
{
    public static class Utilities
    {
        public static Claim[] GetTokenClaims(string sub, DateTime dateTime)
		{
			// Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
			// You can add other claims here, if you want:
			return new Claim[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, sub),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Iat, PublicFunctions.ToUnixEpochDate(dateTime).ToString(), ClaimValueTypes.Integer64)
			};
		}

        public static string GetSubFromToken(string tokentype, string token)
        {
            string email = string.Empty;
			var tokenRepository = new TokenRepository(Startup.connString);
            var getToken = tokenRepository.GetByTypeAndToken(tokentype, token);
            if (getToken != null)
            {
                email = (new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken).Subject;
            }
            return email;
		}

        public static dynamic GenerateTokens(string email)
        {
            DateTime now = DateTime.Now;
            var accessJwt = new JwtSecurityToken(issuer: Startup.userTokenOptions.Issuer, audience: Startup.userTokenOptions.Audience, claims: GetTokenClaims(email, now), notBefore: now, expires: now.Add(Startup.userTokenOptions.AccessExpiration), signingCredentials: Startup.userTokenOptions.SigningCredentials);
            var encodedAccessJwt = new JwtSecurityTokenHandler().WriteToken(accessJwt);

            var refreshJwt = new JwtSecurityToken(issuer: Startup.userTokenOptions.Issuer, audience: Startup.userTokenOptions.Audience, claims: GetTokenClaims(email, now), notBefore: now, expires: now.Add(Startup.userTokenOptions.RefreshExpiration), signingCredentials: Startup.userTokenOptions.SigningCredentials);
            var encodedRefreshJwt = new JwtSecurityTokenHandler().WriteToken(refreshJwt);

            //var token = new TokenRepository(Startup.connString);
            //token.AddForUser(email, "access", encodedAccessJwt);
            //token.AddForUser(email, "refresh", encodedRefreshJwt);

			var result = new
            {
                access_token = encodedAccessJwt,
                access_token_expires_in = (int)Startup.userTokenOptions.AccessExpiration.TotalSeconds,
                refresh_token = encodedRefreshJwt,
                refresh_token_expires_in = (int)Startup.userTokenOptions.RefreshExpiration.TotalSeconds,
            };

            return result;
		}

        public static string GenerateActiveToken(string email)
        {
			DateTime now = DateTime.Now;
            var activeJwt = new JwtSecurityToken(issuer: Startup.userTokenOptions.Issuer, audience: Startup.userTokenOptions.Audience, claims: GetTokenClaims(email, now), notBefore: now, expires: now.Add(Startup.userTokenOptions.ActiveExpiration), signingCredentials: Startup.userTokenOptions.SigningCredentials);
			var encodedActiveJwt = new JwtSecurityTokenHandler().WriteToken(activeJwt);
			var token = new TokenRepository(Startup.connString);
			token.AddForUser(email, "active", encodedActiveJwt);

			return encodedActiveJwt;
		}

		public static string GenerateResetToken(string email)
		{
			DateTime now = DateTime.Now;
            var resetJwt = new JwtSecurityToken(issuer: Startup.userTokenOptions.Issuer, audience: Startup.userTokenOptions.Audience, claims: GetTokenClaims(email, now), notBefore: now, expires: now.Add(Startup.userTokenOptions.ResetExpiration), signingCredentials: Startup.userTokenOptions.SigningCredentials);
			var encodedResetJwt = new JwtSecurityTokenHandler().WriteToken(resetJwt);
			var token = new TokenRepository(Startup.connString);
			token.AddForUser(email, "reset", encodedResetJwt);

			return encodedResetJwt;
		}

        public static string ActiveMailAsync(string email)
        {
            string sent = string.Empty;
            string activeToken = GenerateActiveToken(email);
            sent = new Mail().SendEmailAsync(email, "xxxxxx@imoogoo.com", "Account activation", "http://www.xxxxxx.com/api/users/active/" + activeToken).Result;
            return sent;
        }

		public static string ResetMailAsync(string email)
		{
			string sent = string.Empty;
			string resetToken = GenerateResetToken(email);
            sent = new Mail().SendEmailAsync(email, "xxxxxx@imoogoo.com", "Account password reset", "http://www.xxxxxx.com/api/users/reset/" + resetToken).Result;
			return sent;
		}

	}
}
