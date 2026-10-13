// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class PredictionDtoNER
{
    public string type { get; set; }
    public List<string> entity { get; set; }
    public List<int> position { get; set; }
    public List<double> probability { get; set; }
}

public class PredictionDto
{
    public List<List<PredictionDtoNER>> ner_btc { get; set; }
    public List<List<PredictionDtoNER>> ner_fin { get; set; }
    public SentimentDto sentiment { get; set; }
    public int news_id { get; set; }
}

public class SentimentDto
{
    public string label { get; set; }
    public double score { get; set; }
}