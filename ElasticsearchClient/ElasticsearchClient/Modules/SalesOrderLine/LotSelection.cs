using Datably.Units;
using Newtonsoft.Json;

namespace ElasticsearchClient.Modules.SalesOrderLine;

public class LotSelections
{
    public Money Cost { get; set; }
    public string LotId { get; set; }
    public string PartId { get; set; }
    public int Quantity { get; set; }
    public Money ManufacturedCost { get; set; }

    [JsonConstructor]
    public LotSelections()
    {
    }

    /// <summary>
    /// Swagger will generate the wrong code for this.  We will use a simple Money object because json deserialization will recognize that {amount, currency} fits this scheme.
    /// </summary>
    /// <param name="lotId"></param>
    /// <param name="cost"></param>
    /// <param name="quantity"></param>
    public LotSelections(string lotId, Money cost, int quantity, string partId, Money manufacturedCost)
    {
        Cost = cost;
        LotId = lotId;
        Quantity = quantity;
        PartId = partId;
        ManufacturedCost = manufacturedCost;
    }
}