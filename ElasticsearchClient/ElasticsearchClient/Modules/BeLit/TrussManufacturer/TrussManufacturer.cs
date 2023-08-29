using Bogus;

namespace ElasticsearchClient.Modules.BeLit.TrussManufacturer;

public class TrussManufacturer
{
    public int Id { get; private set; }
    public string Name { get; private set; }
}

public class TrussManufactureFake : Faker<TrussManufacturer>
{
    public TrussManufactureFake()
    {
        RuleFor(x => x.Id, f => f.Random.Int(1, 1000));
        RuleFor(x => x.Name, f => f.Company.CompanyName());
    }
}