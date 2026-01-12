using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TheCurseOfKnowledge.Shared.DTOs
{
    public class BinanceKlineMessage
    {
        [JsonPropertyName("e")] public string EventType { get; set; } = string.Empty;
        [JsonPropertyName("E")] public long EventTime { get; set; }
        [JsonPropertyName("s")] public string Symbol { get; set; } = string.Empty;
        [JsonPropertyName("k")] public KlineDetail Detail { get; set; } = new();
    }

    public class KlineDetail
    {
        [JsonPropertyName("t")] public long StartTime { get; set; }
        [JsonPropertyName("T")] public long CloseTime { get; set; }
        [JsonPropertyName("s")] public string Symbol { get; set; } = string.Empty;
        [JsonPropertyName("i")] public string Interval { get; set; } = string.Empty;
        [JsonPropertyName("f")] public long FirstTradeId { get; set; }
        [JsonPropertyName("L")] public long LastTradeId { get; set; }

        // Data Harga (OHLC)
        [JsonPropertyName("o")] public string OpenPrice { get; set; } = "0";
        [JsonPropertyName("c")] public string ClosePrice { get; set; } = "0";
        [JsonPropertyName("h")] public string HighPrice { get; set; } = "0";
        [JsonPropertyName("l")] public string LowPrice { get; set; } = "0";

        // Data Volume
        [JsonPropertyName("v")] public string BaseAssetVolume { get; set; } = "0";
        [JsonPropertyName("q")] public string QuoteAssetVolume { get; set; } = "0";
        [JsonPropertyName("V")] public string TakerBuyBaseVolume { get; set; } = "0";
        [JsonPropertyName("Q")] public string TakerBuyQuoteVolume { get; set; } = "0";

        [JsonPropertyName("n")] public int NumberOfTrades { get; set; }
        [JsonPropertyName("x")] public bool IsClosed { get; set; }
        [JsonPropertyName("B")] public string Ignore { get; set; } = string.Empty;
    }
}