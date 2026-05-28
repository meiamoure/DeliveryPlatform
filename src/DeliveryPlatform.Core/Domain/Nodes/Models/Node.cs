using DeliveryPlatform.Core.Common;
using DeliveryPlatform.Core.Domain.Nodes.Data;

namespace DeliveryPlatform.Core.Domain.Nodes.Models;

public class Node
{
    private Node() { } 

    internal Node(Guid id, string name, double lat, double lng, NodeType type)
    {
        Id = id;
        Name = name;
        Lat = lat;
        Lng = lng;
        Type = type;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = default!;
    public double Lat { get; private set; }
    public double Lng { get; private set; }
    public NodeType Type { get; private set; }

    public static Node Create(CreateNodeData d) =>
        new(Guid.NewGuid(), d.Name, d.Lat, d.Lng, d.Type);

    public void Update(UpdateNodeData d)
    {
        Name = d.Name;
        Lat = d.Lat;
        Lng = d.Lng;
        Type = d.Type;
    }
}
