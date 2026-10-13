using System.Text.Json.Serialization;

public class News
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Url { get; set; }
    public DateTime PublishDate { get; set; }
    public NewsSource? Source { get; set; }
    public int SourceId { get; set; }
    [JsonIgnore]

    public IList<Coin>? Coins { get; set; }
    [JsonIgnore]
    public IList<Prediction>? Predictions { get; set; }
}