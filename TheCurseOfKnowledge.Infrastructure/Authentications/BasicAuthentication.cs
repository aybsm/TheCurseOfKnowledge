using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TheCurseOfKnowledge.Core.Interfaces.Authentications;

namespace TheCurseOfKnowledge.Infrastructure.Authentications
{
    public class BasicAuthentication : IAuthenticationProvider
    {
        readonly string _username;
        readonly string _password;
        public BasicAuthentication(string username, string password)
        {
            _username = username;
            _password = password;
        }
        public AuthenticationHeaderValue GetToken()
            => new AuthenticationHeaderValue("basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_username}:{_password}")));
        public async Task<AuthenticationHeaderValue> GetTokenAsync()
            => await Task.FromResult(new AuthenticationHeaderValue("basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_username}:{_password}"))));
    }
}
