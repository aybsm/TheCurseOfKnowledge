using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TheCurseOfKnowledge.Core.Interfaces.Authentications;

namespace TheCurseOfKnowledge.Infrastructure.Authentications
{
    public class ApiKeyAuthentication : IAuthenticationProvider
    {
        readonly string _token;
        public ApiKeyAuthentication(string token)
            => _token = token;
        public AuthenticationHeaderValue GetToken()
            => new AuthenticationHeaderValue(_token);
        public async Task<AuthenticationHeaderValue> GetTokenAsync()
            => await Task.FromResult(new AuthenticationHeaderValue(_token));
    }
}
