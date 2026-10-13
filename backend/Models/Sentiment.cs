public class Sentiment
{
    public int Id { get; set; }
    public string Type { get; set; }
    public double Score { get; set; }
    public News News { get; set; }
    public int NewsId { get; set; }
}