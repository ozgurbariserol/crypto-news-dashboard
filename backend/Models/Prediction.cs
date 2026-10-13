public class Prediction
{
    public int Id { get; set; }
    public string PredictionKey { get; set; }
    public string PredictionType { get; set; }
    public string PredictionValue { get; set; }
    public News News { get; set; }
    public int NewsId { get; set; }
}