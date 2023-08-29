using Datably.Units;

namespace ElasticsearchClient.Modules.SalesOrderLine;

public class VendorPriceRequestSelections
{
    public int VendorPriceRequestId { get; private set; }
    public Money Cost { get; private set; }
    public int Quantity { get; private set; }
    public string PartId { get; private set; }

    public VendorPriceRequestSelections(int vendorPriceRequestId, Money cost, int quantity, string partId)
    {
        VendorPriceRequestId = vendorPriceRequestId;
        Cost = cost;
        Quantity = quantity;
        PartId = partId;
    }
}