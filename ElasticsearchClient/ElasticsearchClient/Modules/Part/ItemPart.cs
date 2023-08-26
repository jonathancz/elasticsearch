using Bogus;

namespace ElasticsearchClient.Modules.Part;

public class ItemPart
{
    public Guid Id { get; set; }
    public string Partcode { get; set; }
    public string Description { get; set; }
    public decimal Cost { get; set; }

    public ItemPart(Guid id, string partcode, string description, decimal cost)
    {
        Id = id;
        Partcode = partcode;
        Description = description;
        Cost = cost;
    }
}

public class ItemPartFaker : Faker<ItemPart>
{
    public ItemPartFaker()
    {
        CustomInstantiator(f => new ItemPart(Guid.NewGuid(),
            f.Commerce.Product(),
            f.Lorem.Sentence(),
            f.Finance.Amount(1m, 1000m)));
    }
}