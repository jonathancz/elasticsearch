using Datably.Units;

namespace ElasticsearchClient.Modules.SalesOrderLine;

public class SalesOrderLine
{
    public int SalesOrderIdSurrogate { get; private set; }
    public int SalesOrderLineNumberId { get; private set; }
    public int LineNumber { get; private set; }
    private SalesOrderLineStatus _status { get; set; }
    public ItemCost ItemCost { get; private set; }
    public PriceRequest PriceRequest { get; private set; }
    public CalculatedPrice CalculatedPrice { get; private set; }
    public string UserId { get; private set; }

    // chosen price should always be in customer currency
    public Money ChosenPrice { get; private set; }
}