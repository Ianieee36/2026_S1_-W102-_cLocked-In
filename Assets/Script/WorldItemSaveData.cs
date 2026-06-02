using System.Collections.Generic;
using UnityEngine;

public static class WorldItemSaveData
{
    private static HashSet<string> pickedUpIDs = new HashSet<string>();

    public static void MarkPickedUp(string id)
    {
        pickedUpIDs.Add(id);
    }

    public static bool IsPickedUp(string id)
    {
        return pickedUpIDs.Contains(id);
    }

    public static List<string> GetAllPickedUp()
    {
        return new List<string>(pickedUpIDs);
    }

    public static void LoadPickedUp(List<string> ids)
    {
        pickedUpIDs = new HashSet<string>(ids ?? new List<string>());
    }

    public static void Clear()
    {
        pickedUpIDs.Clear();
    }
}