namespace EnterpriseAddLogs.Services
{
    using System.Net.Http;

    public abstract class Service
    {
        public Service()
        {

        }

        protected static void AddRequestHeaders(HttpRequestMessage request, string subscriptionKey)
        {
            request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
        }
    }
}
