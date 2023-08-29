using Bogus;
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

public class LotFaker : Faker<Lot>
{
    public LotFaker()
    {
        RuleFor(x => x.Id, f => f.Random.Int(1, 1000));
        RuleFor(x => x.Name, f => f.Commerce.ProductName());
        RuleFor(x => x.SubdivisionId, f => f.Random.Int(1, 10));
        RuleFor(x => x.CustomerId, f => f.Random.Int(1, 100));
        RuleFor(x => x.TrussManufacturerId, f => f.Random.Int(1, 50));
        RuleFor(x => x.Address, f => new LotAddressFaker().Generate());
        RuleFor(x => x.WindExposure, f => f.PickRandom(new string[] { "B", "C", "D" }));
        RuleFor(x => x.WindSpeed, f => f.Random.Float(90, 150).ToString("F2"));
        RuleFor(x => x.ExternalLotJobNumber, f => f.Random.AlphaNumeric(10));
    }
}