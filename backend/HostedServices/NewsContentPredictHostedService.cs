using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

public class NewsContentPredictHostedService : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly string _baseUrl;
    private readonly ILogger<NewsContentPredictHostedService> _logger;

    public NewsContentPredictHostedService(IConfiguration configuration, ILogger<NewsContentPredictHostedService> logger)
    {
        // Target service address; defaults to the original hard-coded local address.
        _baseUrl = (configuration["Services:Prediction"] ?? "http://127.0.0.1:12000").TrimEnd('/');
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Başlangıçta 0 saniye gecikme ile ilk isteği yapar
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));

        return Task.CompletedTask;
    }

    private async void DoWork(object state)
    {
        try
        {
            var newslist = NewsSourcesList.NotPredictedNews;
            // JSON verisini oluştur
            foreach (var news in newslist.ToList())
            {
                var requestData = new
                {
                    id = news.Id,
                    content = news.Content
                };
                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // HTTP POST isteğini gönder
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync($"{_baseUrl}/predict", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                    }
                }
                NewsSourcesList.NotPredictedNews.Remove(news);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Queue processing failed");
        }
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {

        _timer?.Change(Timeout.Infinite, 0);
        _timer?.Dispose();

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
