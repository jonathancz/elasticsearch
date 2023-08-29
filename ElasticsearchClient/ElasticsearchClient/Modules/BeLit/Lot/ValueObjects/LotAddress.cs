using Bogus;

namespace ElasticsearchClient.Modules.BeLit.Lot.ValueObjects;

public class LotAddress
{
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public string County { get; set; }
}

public class LotAddressFaker : Faker<LotAddress>
{
    public LotAddressFaker()
    {
        RuleFor(x => x.Street, f => f.Address.StreetAddress());
        RuleFor(x => x.City, f => f.Address.City());
        RuleFor(x => x.State, f => f.Address.State());
        RuleFor(x => x.Zip, f => f.Address.ZipCode());
        RuleFor(x => x.County, f => f.Address.County());
    }
}