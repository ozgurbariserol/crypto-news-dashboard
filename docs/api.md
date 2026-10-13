# API and SignalR notes

Base URL (dev): `http://localhost:5000`. Controllers use `[Route("[controller]")]`. Most write endpoints return a bare `true`/`false` instead of HTTP status codes.

## NewsSource

`NewsSource { id, title, url, rssUrl, isSpa, isActive, xpath }`

- `GET /NewsSource` returns all sources.
- `POST /NewsSource` creates one (body: NewsSource).
- `PATCH /NewsSource` updates all fields of the source with the given `id`.
- `DELETE /NewsSource/{id}` deletes it.

## News

`News { id, title, content, url, publishDate, sourceId, source }`. Articles inserted from RSS carry `content = "-"` until content is fetched.

- `GET /News`, `GET /News/Sentiments`
- `GET /News/AggregatorSource`: copies all news sources into the RSS queue.
- `GET /News/AggregatorNews`: copies articles with `content == "-"` into the content queue.
- `GET /News/PredictionQueue`: replaces the analysis queue with articles that have content and no Prediction and no Sentiment.
- `GET /News/ClearQueue`
- `POST /News`, `POST /News/AddRange`, `PATCH /News/UpdateContent` (body `{ id, content }`)
- `POST /News/SavePredictionsAndSentiment`

### SavePredictionsAndSentiment body

```json
{
  "news_id": 1,
  "sentiment": { "label": "example-label", "score": 0.5 },
  "ner_btc": [[ { "type": "EXAMPLE", "entity": ["example"], "position": [0, 1], "probability": [0.9] } ]],
  "ner_fin": []
}
```

All values are illustrative. Only `ner_btc[0]` is persisted (as `Predictions` rows); `Sentiments` gets one row. The saved sentiment (with its article) is then pushed to all SignalR clients. An article with `id = news_id` must exist.

To try the live page without the analysis service: create a news source (`POST /NewsSource`), insert an article referencing it (`POST /News`), open `/sentiment`, then POST the body above.

## Calls the backend makes (external services, not included)

| Timer | Interval | Request |
|---|---|---|
| RSS fetch | 30 s | `POST {Services:Collector}/predict` body `{ url: <rssUrl>, sourceId }` per queued source |
| Content fetch | 10 s | `POST {Services:Collector}/getcontent` body `{ url, id, xpath }` per queued article |
| Analysis | 10 s | `POST {Services:Prediction}/predict` body `{ id, content }` per queued article |

## SignalR

- Endpoint: `/hub` (`SentimentHub : Hub<IClient>`).
- Server-to-client event: `Sentiment(Sentiment)`, payload `{ id, type, score, newsId, news }`.
- The hub declares no client-callable methods.
