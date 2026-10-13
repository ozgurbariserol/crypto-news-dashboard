using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class NewsController : ControllerBase
{
    private NewsCollectorDbContext _context;
    private IHubContext<SentimentHub, IClient> _hub;
    public NewsController(NewsCollectorDbContext context, IHubContext<SentimentHub, IClient> hub)
    {
        _context = context;
        _hub = hub;
    }

    [HttpGet]
    public IEnumerable<News> Get()
    {
        return _context.News.ToList();
    }
    [HttpGet("Sentiments")]
    public IEnumerable<Sentiment> GetSentiments()
    {
        return _context.Sentiments.ToList();
    }
    [HttpGet("AggregatorSource")]
    public bool GetAggregate()
    {
        NewsSourcesList.List.AddRange(_context.NewsSources.ToList());
        return true;
    }
    [HttpPost("SavePredictionsAndSentiment")]
    public bool SavePredictionsAndSentiment(PredictionDto predictionDto)
    {
        var predictions = predictionDto.ner_btc[0].Select(x => new Prediction
        {
            NewsId = predictionDto.news_id,
            PredictionKey = String.Join(" ", x.entity),
            PredictionValue = x.probability.Max().ToString(),
            PredictionType = x.type
        });

        _context.Predictions.AddRange(predictions);
        var sentiment = new Sentiment { NewsId = predictionDto.news_id, Type = predictionDto.sentiment.label, Score = predictionDto.sentiment.score };
        var newsentiment = _context.Sentiments.Add(sentiment);
        _context.SaveChanges();
        sentiment = _context.Sentiments.First(x => newsentiment.Entity.Id == x.Id);
        _hub.Clients.All.Sentiment(sentiment);

        return true;
    }
    [HttpGet("AggregatorNews")]
    public bool GetAggregate2()
    {
        var news = _context.News.Where(x => x.Content == "-").ToList();
        NewsSourcesList.News.AddRange(news);
        return true;
    }


    [HttpGet("ClearQueue")]
    public bool ClearQueue()
    {
        NewsSourcesList.News = new List<News>();
        NewsSourcesList.NotPredictedNews = new List<News>();
        NewsSourcesList.List = new List<NewsSource>();
        return true;
    }

    [HttpGet("PredictionQueue")]
    public bool PredictionQueue()
    {
        NewsSourcesList.NotPredictedNews = _context.News
            .Where(x => x.Content != "-")
            .Where(x => !_context.Predictions.Any(y => y.News.Id == x.Id))
            .Where(x => !_context.Sentiments.Any(y => y.News.Id == x.Id))
            .ToList();
        return true;
    }


    [HttpPost]
    public bool Post(News news)
    {
        try
        {
            _context.Add(news);
            _context.SaveChanges();

            return true;
        }
        catch (System.Exception)
        {

            return false;
        }
    }

    [HttpPost("AddRange")]
    public bool AddRange(List<News> news)
    {
        try
        {
            _context.News.AddRange(news);
            _context.SaveChanges();

            return true;
        }
        catch (System.Exception)
        {

            return false;
        }
    }

    [HttpPatch("UpdateContent")]
    public bool Update(ContentDto news)
    {
        try
        {
            var source = _context.News.First(x => news.Id == x.Id);
            source.Content = news.Content;

            if (source != null)
            {
                _context.Update(source);
                _context.SaveChanges();
                return true;

            }
            return false;
        }
        catch (System.Exception)
        {

            return false;
        }
    }

    [HttpDelete("{id}")]
    public bool Delete(int id)
    {
        try
        {
            var source = _context.NewsSources.First(x => x.Id == id);
            if (source != null)
            {
                _context.Remove(source);
                _context.SaveChanges();
                return true;
            }

            return true;
        }
        catch (System.Exception)
        {

            return false;
        }
    }
}
