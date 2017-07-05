using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.WebKey;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace PlayByPlay
{
    public class KeyVaultBase
    {
        protected KeyVaultClient KeyVaultClient;
        protected ClientCredential ClientCredential;
        protected string VaultAddress;

        protected string GetKeyUri(string keyName)
        {
            var retrievedKey = KeyVaultClient.GetKeyAsync(VaultAddress, keyName).GetAwaiter().GetResult();
            return retrievedKey.Key.Kid;
        }

        protected KeyBundle GetKeyBundle()
        {
            var defaultKeyBundle = new KeyBundle
            {
                Key = new JsonWebKey()
                {
                    Kty = JsonWebKeyType.Rsa,
                },
                Attributes = new KeyAttributes()
                {
                    Enabled = true,
                    Expires = UnixEpoch.FromUnixTime(int.MaxValue),
                    NotBefore = UnixEpoch.FromUnixTime(0),
                }
            };

            return defaultKeyBundle;
        }

        protected Dictionary<string, string> GetKeyTags()
        {
            return new Dictionary<string, string> { { "purpose", "Master Key" }, { "app", "MyCompany" } };
        }

        protected Dictionary<string, string> GetSecretTags()
        {
            return new Dictionary<string, string> { { "purpose", "Encrypted Secret" }, { "app", "MyCompany" } };
        }

        protected async Task<string> GetAccessToken(string authority, string resource, string scope)
        {
            var context = new AuthenticationContext(authority, TokenCache.DefaultShared);
            var result = await context.AcquireTokenAsync(resource, ClientCredential);

            return result.AccessToken;
        }

        protected HttpClient GetHttpClient()
        {
            return (HttpClientFactory.Create(new InjectHostHeaderHttpMessageHandler()));
        }
    }
}
