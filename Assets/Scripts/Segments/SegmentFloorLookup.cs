using System.Collections.Generic;
using UnityEngine;

public class SegmentFloorLookup : MonoBehaviour
{
    [SerializeField] string _generatedRootName = "Generated";

    readonly Dictionary<(int lane, int cell), 
        GameObject>         _tiles = new();


    void Awake()
    {
        CacheTiles();
    }

    public void CacheTiles()
    {
        _tiles.Clear();

        var generated = transform.Find(_generatedRootName);
        if (generated == null) return;

        for (int i = 0; i < generated.childCount; i++)
        {
            var t = generated.GetChild(i);
            // Expected name: Floor_L{lane}_C{cell}
            // Example: Floor_L1_C7
            if (!t.name.StartsWith("Floor_L")) continue;

            ParseName(t.name, out int lane, out int cell);
            _tiles[(lane, cell)] = t.gameObject;
        }
    }

    public bool TryRemoveTile(int lane, int cell)
    {
        if (_tiles.TryGetValue((lane, cell), out var tile) && tile != null)
        {
            Destroy(tile); // runtime-safe
            _tiles.Remove((lane, cell));
            return true;
        }
        return false;
    }


    private static void ParseName(string name, out int lane, out int cell)
    {
        lane = 0; cell = 0;

        // Floor_L1_C7 -> ["Floor", "L1", "C7"]
        var parts = name.Split('_');
        if (parts.Length < 3) return;

        if (parts[1].StartsWith("L")) int.TryParse(parts[1].Substring(1), out lane);
        if (parts[2].StartsWith("C")) int.TryParse(parts[2].Substring(1), out cell);
    }
}
