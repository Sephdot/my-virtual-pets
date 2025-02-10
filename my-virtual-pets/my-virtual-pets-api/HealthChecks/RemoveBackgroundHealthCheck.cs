using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace my_virtual_pets_api.HealthChecks
{
    public class RemoveBackgroundHealthCheck : IHealthCheck
    {
        private HttpClient _httpClient;

        public RemoveBackgroundHealthCheck(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("https://status.picsart.io/");
                if (!response.IsSuccessStatusCode)
                {
                    return HealthCheckResult.Unhealthy($"Failed to reach status page of background remover API. HTTP {response.StatusCode}");
                }

                string pageContent = await response.Content.ReadAsStringAsync();
                if (pageContent.Contains("All Systems Operational")) return HealthCheckResult.Healthy("Remove background API is operational");
                return HealthCheckResult.Unhealthy("Background remover API has reported issues.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy($"Failed to reach Background remover API status page: {ex.Message}");
            }
        }
    }
}
