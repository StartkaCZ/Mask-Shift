using System.Collections.Generic;
using UnityEngine;

public class PatternLibrary : MonoBehaviour
{
    readonly Dictionary<MaskPatternGroup, List<PatternSO>> _patterns = new();


    private void Awake()
    {
        LoadGroup(MaskPatternGroup.Stone, "Patterns/Stone");
        LoadGroup(MaskPatternGroup.Feather, "Patterns/Feather");
        LoadGroup(MaskPatternGroup.Mirror, "Patterns/Mirror");
        LoadGroup(MaskPatternGroup.Neutral, "Patterns/Neutral");
    }

    private void LoadGroup(MaskPatternGroup group, string resourcesPath)
    {
        var loaded = Resources.LoadAll<PatternSO>(resourcesPath);
        var list = new List<PatternSO>();

        foreach (var p in loaded)
        {
            if (p == null) continue;
            if (!p.IsValid(out var error))
            {
                Debug.LogWarning($"Pattern invalid: {p.name} ({group}) -> {error}");
                continue;
            }
            list.Add(p);
        }

        _patterns[group] = list;
        Debug.Log($"Loaded {list.Count} patterns from Resources/{resourcesPath}");
    }


    public PatternSO GetRandom(MaskPatternGroup group)
    {
        if (!_patterns.TryGetValue(group, out var list) || list.Count == 0)
            return null;

        return list[Random.Range(0, list.Count)];
    }
}
