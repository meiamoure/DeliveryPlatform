using DeliveryPlatform.Core.Common;

namespace DeliveryPlatform.Core.Domain.Nodes.Data;

public record CreateNodeData(string Name, double Lat, double Lng, NodeType Type);

