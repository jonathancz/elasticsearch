using Datably.Units;

namespace ElasticsearchClient.Modules.SalesOrderLine;

public class PriceRequest
{
    /// <summary>
    /// Customer Id is selected on the quote line / in the query.
    /// </summary>
    public string CustomerId { get; set; }

    /// <summary>
    /// Site is used for looking up stock.  Which site are we pulling stock from. (IILs)
    /// </summary>
    public string Site { get; set; }

    /// <summary>
    /// Part ID / Device that is selected
    /// </summary>
    public string PartId { get; set; }

    /// <summary>
    /// Customer Part Id - really only used for selecting a VPA.
    /// </summary>
    public string CustomerPartId { get; set; }

    /// <summary>
    /// Manufacturer is used in conjunction with part id in order to determine which vendor / manufacturer
    /// is used for a particular part.
    /// </summary>
    public string ManufacturerId { get; set; }

    public string ManufacturerName { get; set; }

    /// <summary>
    /// Set on the calculator directly, direct lookup.
    /// </summary>
    public string Market { get; set; }

    /// <summary>
    /// Set on the calculator / quote line.
    /// </summary>
    public string ProductLine { get; set; }

    /// <summary>
    /// Lookup from Part Master - User_19.  If it is sole source, then it is exclusive.
    /// </summary>
    public string Position { get; set; }

    /// <summary>
    /// Category is based on user 23.  This can be China, Long Tail, Major, Interco, etc.
    /// </summary>
    public string CustomerCategory { get; set; }

    /// <summary>
    /// Part Category is set from ---not sure yet -- but it will help identify cap/res/bulk, etc.
    /// </summary>
    public string PartCategory { get; set; }

    /// <summary>
    /// customer site comes from SOFCM.User4
    /// </summary>
    public string CustomerSite { get; set; }

    /// <summary>
    /// Yet to be determined.  Default to No.
    /// </summary>
    public string QuickTurn { get; set; }

    /// <summary>
    /// Cost is the Material Cost -- comes from a combination of item costs (vprs & iils)
    /// </summary>
    public Money? Cost { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string MfgRepId { get; set; }


    public bool HasCost => Cost != null;
    public bool HasException { get; set; }
    public Currency CustomerCurrency { get; set; }
    public CurrencyConversion CustomerCurrencyConversion { get; set; }

    public decimal CommissionPercentage { get; set; }

    public PriceRequest()
    {
    }
}