using UnityEngine;

public enum MaskPatternGroup
{
    Stone,
    Feather,
    Mirror,
    Neutral
}

[CreateAssetMenu(fileName = "PatternSO", menuName = "Scriptable Objects/PatternSO")]
public class PatternSO : ScriptableObject
{
    [Header("Meta")]
    public MaskPatternGroup group = MaskPatternGroup.Neutral;

    [Header("Grid (3 lanes x 10 cells)")]
    [Tooltip("Top lane (lane index 0). Must be exactly 10 chars.")]
    public string           lane0 = "..........";

    [Tooltip("Middle lane (lane index 1). Must be exactly 10 chars.")]
    public string           lane1 = "..........";

    [Tooltip("Bottom lane (lane index 2). Must be exactly 10 chars.")]
    public string           lane2 = "..........";

    [Header("Legend (characters)")]
    [Tooltip("O = obstacle, C = coin, > = trap shoots right, < = trap shoots left, . = empty floor, G = hole (removed floor)")]
    public string           legend = "O obstacle | C coin | > trap right | < trap left | G hole | . empty floor";

    [Header("Prefabs")]
    public GameObject       obstaclePrefab;
    public GameObject       coinPrefab;
    public GameObject       trapPrefab; // PF_ShootingTrap_Horizontal

    public int              Width => 10;
    public int              Height => 3;


    public bool IsValid(out string error)
    {
        if (lane0 == null || lane1 == null || lane2 == null)
        {
            error = "One or more lane strings are null.";
            return false;
        }

        if (lane0.Length != Width || lane1.Length != Width || lane2.Length != Width)
        {
            error = "Each lane string must be exactly 10 characters long.";
            return false;
        }

        if (obstaclePrefab == null || coinPrefab == null || trapPrefab == null)
        {
            error = "Assign obstaclePrefab, coinPrefab, and trapPrefab.";
            return false;
        }

        error = "";
        return true;
    }


    public char GetCell(int lane, int cell)
    {
        return lane switch
        {
            0 => lane0[cell],
            1 => lane1[cell],
            2 => lane2[cell],
            _ => '.'
        };
    }
}
