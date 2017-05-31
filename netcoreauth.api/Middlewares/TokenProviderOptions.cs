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

namespace netcoreauth.api
{
	public class TokenProviderOptions
	{
        public string Issuer { get; set; }
		public string Audience { get; set; }
		public TimeSpan ActiveExpiration { get; set; } = TimeSpan.FromDays(1);
		public TimeSpan ResetExpiration { get; set; } = TimeSpan.FromDays(1);
		public TimeSpan AccessExpiration { get; set; } = TimeSpan.FromDays(30);
		public TimeSpan RefreshExpiration { get; set; } = TimeSpan.FromDays(1000);
		public SigningCredentials SigningCredentials { get; set; }
	}
}