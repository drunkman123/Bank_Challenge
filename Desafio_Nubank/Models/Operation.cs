using System.Text.Json.Serialization;

public class Operation
{
    [JsonPropertyName("operation")]
    public OperationType Type { get; set; }

    [JsonPropertyName("unit-cost")]
    public decimal UnitCost { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
    public Operation(OperationType type, decimal unitCost, int quantity)
    {
        Type = type;
        UnitCost = unitCost;
        Quantity = quantity;
    }
}