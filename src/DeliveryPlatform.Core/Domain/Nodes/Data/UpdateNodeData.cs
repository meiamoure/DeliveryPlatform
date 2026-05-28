using DeliveryPlatform.Core.Common;

namespace DeliveryPlatform.Core.Domain.Nodes.Data;

public record UpdateNodeData(string Name, double Lat, double Lng, NodeType Type);

