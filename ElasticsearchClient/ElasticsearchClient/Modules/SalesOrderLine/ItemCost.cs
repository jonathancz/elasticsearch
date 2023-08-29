using Datably.Units;

namespace ElasticsearchClient.Modules.SalesOrderLine;

public class ItemCost
{
    public List<VendorPriceRequestSelections> VendorPriceRequests { get; }
    public List<LotSelections> Lots { get; }

    // private fields
    public Money Cost { get; }
    public int QuantityRequested { get; private set; }
    public int TotalQuantityRepresented { get; private set; }

    public Money ExtendedCost => VendorPriceRequests.Sum(x => x.Cost * x.Quantity) + Lots.Sum(x => x.Cost * x.Quantity);

    public ItemCost(int quantityRequested, List<VendorPriceRequestSelections> vendorPriceRequests = null,
        List<LotSelections> lots = null)
    {
        // set all details
        QuantityRequested = quantityRequested;
        VendorPriceRequests = vendorPriceRequests;
        Lots = lots;

        VendorPriceRequests ??= new List<VendorPriceRequestSelections>();
        Lots ??= new List<LotSelections>();

        // calculate details:
        TotalQuantityRepresented = VendorPriceRequests.Sum(x => x.Quantity) + Lots.Sum(x => x.Quantity);
        if (TotalQuantityRepresented == 0)
            throw new DivideByZeroException("Lots and VPR quantities should be greater than zero.");

        Cost = ((VendorPriceRequests.Sum(x => (x.Cost * x.Quantity)) +
                 Lots.Sum(x => (x.Cost * x.Quantity))) / TotalQuantityRepresented).Round(3);
    }

    // EF Constructor
    private ItemCost()
    {
    }
}