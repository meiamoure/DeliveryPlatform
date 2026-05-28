using System;

namespace DeliveryPlatform.Application.Domain.Deliveries.Commands.ImportDeliveriesFromCsv;

internal static class CsvParsing
{
    public static DateTime? ParseTime(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        return DateTime.Parse(value);
    }
}
