using Bogus;

namespace ElasticsearchClient.Modules.Subdivision;

public class Subdivision
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Subdivision(int id, string name)
    {
        Id = id;
        Name = name;
    }
}

public class SubdivisionFaker : Faker<Subdivision>
{
    public SubdivisionFaker()
    {
        RuleFor(o => o.Id, f => f.UniqueIndex);
        RuleFor(o => o.Name, f => f.Company.CompanyName());
    }
}