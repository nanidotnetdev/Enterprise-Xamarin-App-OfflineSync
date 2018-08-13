using System;
namespace EnterpriseAddLogs.Services
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using EnterpriseAddLogs.Models;
    using Newtonsoft.Json;

    class LogService : Service, ILogService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<LogEntity>> GetAllLogsAsync()
        {
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, ServiceConstants.Urls.GetAllLogEntities))
                {
                    AddRequestHeaders(request, ServiceConstants.SubscriptionKeys.PrimaryKey);

                    HttpResponseMessage response = null;

                    using (var cancellationToken = new CancellationTokenSource())
                    {
                        cancellationToken.CancelAfter(TimeSpan.FromSeconds(ServiceConstants.Defaults.ServiceTimeoutInSecs));

                        response = await client.SendAsync(request, cancellationToken.Token).ConfigureAwait(false);
                    }

                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<ICollection<LogEntity>>(json);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<LogEntity> GetLogAsync(Guid id)
        {
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, string.Format(ServiceConstants.Urls.GetLogEntity, id)))
                {

                    AddRequestHeaders(request, ServiceConstants.SubscriptionKeys.PrimaryKey);

                    HttpResponseMessage response = null;

                    using (var cancellationSource = new CancellationTokenSource())
                    {
                        cancellationSource.CancelAfter(TimeSpan.FromSeconds(ServiceConstants.Defaults.ServiceTimeoutInSecs));

                        response = await client.SendAsync(request, cancellationSource.Token).ConfigureAwait(false);
                    }

                    response.EnsureSuccessStatusCode();

                    var jsonString = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<LogEntity>(jsonString);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public async Task<LogEntity> SaveLogAsync(LogEntity log)
        {
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, ServiceConstants.Urls.SaveLogEntity))
                {
                    AddRequestHeaders(request, ServiceConstants.SubscriptionKeys.PrimaryKey);

                    HttpResponseMessage response = null;

                    //Instead of POST Async add content in the request.
                    request.Content = new StringContent(JsonConvert.SerializeObject(log), Encoding.UTF8, "application/json"); ;

                    using (var cancellationToken = new CancellationTokenSource())
                    {
                        cancellationToken.CancelAfter(TimeSpan.FromSeconds(ServiceConstants.Defaults.ServiceTimeoutInSecs));

                        response = await client.SendAsync(request, cancellationToken.Token)
                            .ConfigureAwait(false);

                    }

                    response.EnsureSuccessStatusCode();

                    var jsonString = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<LogEntity>(jsonString);
                }
            }
        }
    }
}
