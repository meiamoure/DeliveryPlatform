using System;
using DeliveryPlatform.Application.Routing.Models;

namespace DeliveryPlatform.Application.Routing.Vrp;

public static class TwoOpt
{
    private const int HugeTime = int.MaxValue / 8;

    public static IReadOnlyList<Guid> Improve(
        IReadOnlyList<Guid> route,
        DistanceMatrix matrix,
        int maxPasses = 50)
    {
        if (route is null) throw new ArgumentNullException(nameof(route));
        if (route.Count < 4) return route;

        var best = route.ToList();

        if (best[0] != best[^1])
            throw new InvalidOperationException("Route must start and end with the same depot id (closed tour).");

        for (int pass = 0; pass < maxPasses; pass++)
        {
            var improved = false;

            for (int i = 1; i < best.Count - 2; i++)
            {
                for (int k = i + 1; k < best.Count - 1; k++)
                {
                    var a = best[i - 1];
                    var b = best[i];
                    var c = best[k];
                    var d = best[k + 1];

                    // если матрица может быть неполной — пропускай безопасно
                    if (!matrix.TryGet(a, b, out var ab) ||
                        !matrix.TryGet(c, d, out var cd) ||
                        !matrix.TryGet(a, c, out var ac) ||
                        !matrix.TryGet(b, d, out var bd))
                        continue;

                    if (ab?.TravelTimeMin >= HugeTime || cd?.TravelTimeMin >= HugeTime ||
                        ac?.TravelTimeMin >= HugeTime || bd?.TravelTimeMin >= HugeTime)
                        continue;

                    var delta = (ac!.TravelTimeMin + bd!.TravelTimeMin) - (ab!.TravelTimeMin + cd!.TravelTimeMin);

                    if (delta < 0)
                    {
                        ReverseInPlace(best, i, k);
                        improved = true;
                        break; // first improvement
                    }
                }

                if (improved) break;
            }

            if (!improved) break;
        }

        return best;
    }

    private static void ReverseInPlace(List<Guid> list, int left, int right)
    {
        while (left < right)
        {
            (list[left], list[right]) = (list[right], list[left]);
            left++;
            right--;
        }
    }
}
