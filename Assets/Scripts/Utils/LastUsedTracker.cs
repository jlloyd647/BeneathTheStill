using System.Collections.Generic;

public static class LastUsedTracker
{
    private static Dictionary<string, InventoryItem> lastUsed = new();

    public static void Save(string stationId, InventoryItem item)
    {
        lastUsed[stationId] = item;
    }

    public static InventoryItem Get(string stationId)
    {
        return lastUsed.TryGetValue(stationId, out var item) ? item : null;
    }
}
