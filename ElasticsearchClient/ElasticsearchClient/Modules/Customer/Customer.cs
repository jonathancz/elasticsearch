using Bogus;

namespace ElasticsearchClient.Modules.Customer;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Customer()
    {
        
    }
    public Customer(int id, string name)
    {
        Id = id;
        Name = name;
    }
}

public class CustomerFaker : Faker<Customer>
{
    public CustomerFaker()
    {
        RuleFor(o => o.Id, f => f.UniqueIndex);
        RuleFor(o => o.Name, f => f.Person.FullName);
    }
}