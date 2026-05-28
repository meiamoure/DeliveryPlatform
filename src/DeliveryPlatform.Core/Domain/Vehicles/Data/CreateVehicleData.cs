using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryPlatform.Core.Domain.Vehicles.Data;

public sealed record CreateVehicleData(
    string Plate,
    decimal MaxWeightKg,
    decimal MaxVolumeM3,
    int SpeedKmh,
    Guid DepotNodeId
);

