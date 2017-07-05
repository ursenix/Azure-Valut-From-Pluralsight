using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;

namespace PlayByPlay
{
    public class InjectHostHeaderHttpMessageHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken).ContinueWith(response => response.Result, cancellationToken);
        }
    }
}

