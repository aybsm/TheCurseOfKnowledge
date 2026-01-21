using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TheCurseOfKnowledge.Core.Interfaces.Authentications
{
    public interface IAuthenticationProvider
    {
        AuthenticationHeaderValue GetToken();
        Task<AuthenticationHeaderValue> GetTokenAsync();
    }
}
