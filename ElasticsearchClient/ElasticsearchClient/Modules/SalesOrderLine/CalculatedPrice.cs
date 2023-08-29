using Datably.Units;
using Newtonsoft.Json;

namespace ElasticsearchClient.Modules.SalesOrderLine;

public class CalculatedPrice
{
    // **All money should be system currency, except Customer Price**
    public Money BasePrice { get; private set; }

    public decimal? LaborOHPercentage { get; private set; }
    public decimal? SGAPercentage { get; private set; }
    public decimal? EbitdaTargetPercentage { get; private set; }

    /// <summary>
    /// This is Price without Commission
    /// </summary>
    public Money Price { get; private set; }

    public Money PriceWithCommission =>
        CommissionPercentage == null ? Price : (Price * (1m + CommissionPercentage));

    public Money CustomerPriceWithCommission =>
        CommissionPercentage == null ? CustomerPrice : (CustomerPrice * (1m + CommissionPercentage));

    public decimal CommissionPercentage { get; set; }

    public Money CustomerCommissionAmount =>
        new Money(CommissionPercentage * CustomerPrice);

    public Money CommissionAmount =>
        Conversion == null ? CustomerCommissionAmount : (CustomerCommissionAmount * Conversion).Round(3);

    // The from should be the customer currency, to should be system currency.
    public CurrencyConversion Conversion { get; private set; }

    // This should be in customer currency
    [JsonProperty]
    public Money CustomerPrice =>
        Conversion == null ? Price : (Price * Conversion).Round(3);

    public Money Cost { get; private set; }

    public string Reason { get; private set; }

    [JsonProperty]
    public decimal? Markup =>
        Cost == null || Cost.Amount == 0m ? (decimal?)null : (Price / Cost) - 1m;

    [JsonProperty]
    public decimal? MaterialMargin =>
        Cost == null || Cost.Amount == 0m ? (decimal?)null : 1 - Cost / Price;

    [JsonProperty]
    public decimal? GrossMargin =>
        Cost == null || Cost.Amount == 0m ? (decimal?)null : 1 - ((BasePrice * LaborOHPercentage + Cost) / Price);

    /// <summary>
    /// This is calculated working backwards from a calculated price in order to figure out what the EBITDA margin is
    /// by backing out the LaborOH and SGA from the Price. Calculation is available in JIRA-SLM-328
    /// 1 - MC/BP - LOH - SGA
    /// </summary>
    public decimal? CalculatedEbitdaMargin
    {
        get
        {
            // we cannot calculate if it is null
            if (Cost == null)
                return null;

            // if there are no margin pieces, we can only calculate material margin
            if (LaborOHPercentage == null || SGAPercentage == null)
                return MaterialMargin;

            // else, here's the real formula
            return Math.Round((1 - (Cost / Price) - LaborOHPercentage - SGAPercentage) ?? 0m, 3);
        }
    }

    public string CalculatorUsed { get; private set; }
    public DateTime CreatedDateTime { get; private set; }
}