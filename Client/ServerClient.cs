namespace Client
{
    public class ServerClient
    {
        private readonly HttpClient _httpClient;

        public ServerClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public Task<string> GetStatus()
        {
            return _httpClient.GetStringAsync("/account/status");
        }
    }
}
