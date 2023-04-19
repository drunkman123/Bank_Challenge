using System.Text.Json.Serialization;

class Operation
{
    [JsonPropertyName("operation")]
    public OperationType Type { get; set; }

    [JsonPropertyName("unit-cost")]
    public double UnitCost { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
}