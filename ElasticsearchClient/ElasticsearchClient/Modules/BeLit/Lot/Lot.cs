using ElasticsearchClient.Modules.BeLit.Lot.ValueObjects;

namespace ElasticsearchClient.Modules.BeLit.Lot;

public class Lot
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public int SubdivisionId { get; set; }
    public int CustomerId { get; set; }
    public int TrussManufacturerId { get; set; }
    public LotAddress? Address { get; private set; }
    public string? WindExposure { get; set; }
    public string? WindSpeed { get; set; }
    public string? ExternalLotJobNumber { get; set; }
}