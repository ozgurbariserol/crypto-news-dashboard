using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class NewsSourceController : ControllerBase
{
    private NewsCollectorDbContext _context;
    public NewsSourceController(NewsCollectorDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IEnumerable<NewsSource> Get()
    {
        return _context.NewsSources.ToList();
    }

    [HttpPost]
    public bool Post(NewsSource newsSource)
    {
        try
        {
            _context.Add(newsSource);
            _context.SaveChanges();

            return true;
        }
        catch (System.Exception)
        {

            return false;
        }
    }

    [HttpPatch]
    public bool Update(NewsSource newsSource)
    {
        try
        {
            var source = _context.NewsSources.First(x => newsSource.Id == x.Id);
            source.IsActive = newsSource.IsActive;
            source.IsSpa = newsSource.IsSpa;
            source.Title = newsSource.Title;
            source.Url = newsSource.Url;
            source.RssUrl = newsSource.RssUrl;
            source.Xpath = newsSource.Xpath;

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
