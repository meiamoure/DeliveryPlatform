using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryPlatform.Application.Routing.DistanceMatrixBuilder;
public sealed record OsrmTableResult(
    double[][] Durations,
    double[][] Distances
);
