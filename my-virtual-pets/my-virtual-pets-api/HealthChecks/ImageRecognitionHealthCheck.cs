using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace my_virtual_pets_api.HealthChecks
{
    public class ImageRecognitionHealthCheck : IHealthCheck
    {
        private HttpClient _httpClient;

        public ImageRecognitionHealthCheck(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("https://api.dragoneye.ai/status", cancellationToken);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
                {
                    return HealthCheckResult.Healthy("Image recognition API is operational");
                }
                return response.IsSuccessStatusCode
                ? HealthCheckResult.Healthy("Image recognition API is operational.")
                : HealthCheckResult.Unhealthy($"Unexpected status code for image recognition API: {response.StatusCode}");

            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy($"Failed to reach image recognition API: {ex.Message}");
            }
        }
    }
}
