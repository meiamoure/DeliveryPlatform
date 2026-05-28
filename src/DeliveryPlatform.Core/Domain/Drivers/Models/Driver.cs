using DeliveryPlatform.Core.Common;
using DeliveryPlatform.Core.Domain.Drivers.Data;

namespace DeliveryPlatform.Core.Domain.Drivers.Models;

public class Driver : IAggregateRoot
{
    private Driver() { }

    internal Driver(Guid id, string name, string phone)
    {
        Id = id;
        Name = name;
        Phone = phone;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = default!;
    public string Phone { get; private set; } = default!;

    public static Driver Create(CreateDriverData d) =>
        new(Guid.NewGuid(), d.Name, d.Phone);

    public void Update(UpdateDriverData d)
    {
        Name = d.Name;
        Phone = d.Phone;
    }
}
