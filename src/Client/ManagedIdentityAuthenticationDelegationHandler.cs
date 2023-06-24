using Azure.Core;
using Azure.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fleet
{
    /// <summary>
    /// A delegation handler for the <see cref="HttpClient"/> to allow authentication with the Azure Managed Identity
    /// </summary>
    public class ManagedIdentityAuthenticationDelegationHandler : DelegatingHandler
    {
        private const string TenantId = "36946883-9d0b-4229-82f9-316f0ae71b20";

        private readonly TokenCredential _tokenCredential;
        private AccessToken _cachedToken;
        private readonly string _scope;
        private readonly ILogger<ManagedIdentityAuthenticationDelegationHandler> _logger;

        public ManagedIdentityAuthenticationDelegationHandler(string scope, string tenantId, ILogger<ManagedIdentityAuthenticationDelegationHandler> logger) 
            : this (scope, new DefaultAzureCredential(new DefaultAzureCredentialOptions { VisualStudioCodeTenantId = tenantId }), logger)
        {
        }

        public ManagedIdentityAuthenticationDelegationHandler(string scope, ILogger<ManagedIdentityAuthenticationDelegationHandler> logger)
          : this(scope, new DefaultAzureCredential(new DefaultAzureCredentialOptions { VisualStudioCodeTenantId = TenantId }), logger)
        {
        }

        public ManagedIdentityAuthenticationDelegationHandler(string scope, TokenCredential credentialSource, ILogger<ManagedIdentityAuthenticationDelegationHandler> logger)
        {
            _tokenCredential = credentialSource ?? throw new ArgumentNullException(nameof(credentialSource));
            _scope = scope ?? throw new ArgumentNullException(nameof(scope));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                await Authenticate(request, false, cancellationToken);
                return await base.SendAsync(request, cancellationToken);
            }
            catch (CredentialUnavailableException)
            {
                throw;
            }
            catch (AuthenticationFailedException)
            {
                await Authenticate(request, true, cancellationToken);
                return await base.SendAsync(request, cancellationToken);
            }
        }

        private async Task Authenticate(HttpRequestMessage request, bool force = false, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(_cachedToken.Token) || force)
            {
                _cachedToken = await GetTokenAsync(cancellationToken);
                _logger.LogInformation("Generated MSI token: ", _cachedToken.Token);
            }

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _cachedToken.Token);
        }

        public ValueTask<AccessToken> GetTokenAsync(CancellationToken cancellationToken = default) => _tokenCredential.GetTokenAsync(new TokenRequestContext(new[] { _scope }, tenantId: TenantId), cancellationToken);
    }
}
