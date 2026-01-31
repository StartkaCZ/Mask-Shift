using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SegmentPopulator : MonoBehaviour
{
    [Header("Grid Settings (must match your project)")]
    [SerializeField] int                _lanes = 3;
    [SerializeField] int                _cells = 10;
    [SerializeField] float              _cellSpacing = 2.1f;    // your value

    [Header("Debug")]
    [SerializeField] MaskPatternGroup   _debugGroup = MaskPatternGroup.Neutral;

    PatternLibrary                      _patternLibrary;

    void Awake()
    {
        _patternLibrary = GetComponent<PatternLibrary>();
    }


    // Convenience overload for now (until masks are wired)
    public void Populate(Transform segmentRoot, float segmentOriginZ)
    {
        Populate(segmentRoot, segmentOriginZ, _debugGroup);
    }

    public void Populate(Transform segmentRoot, float segmentOriginZ, MaskPatternGroup groupOverride)
    {
        if (_patternLibrary == null)
        {
            Debug.LogError("SegmentPopulator: patternLibrary is not assigned.");
            return;
        }

        var pattern = _patternLibrary.GetRandom(groupOverride);
        if (pattern == null)
        {
            Debug.LogWarning($"No patterns available for group {groupOverride}. Segment left empty.");
            return;
        }

        float half = (_lanes - 1) / 2f;

        // Put spawned objects under a child for cleanliness
        var floorLookup = segmentRoot.GetComponent<SegmentFloorLookup>();
        var contentRoot = new GameObject("Content").transform;
        contentRoot.SetParent(segmentRoot, false);

        for (int lane = 0; lane < _lanes; lane++)
        {
            for (int cell = 0; cell < _cells; cell++)
            {
                char token = pattern.GetCell(lane, cell);
                if (token == '.') continue;

                float x = (lane - half) * _cellSpacing;           // lanes: -3, 0, +3
                float z = segmentOriginZ + cell * _cellSpacing;

                if (token == 'O')
                {
                    Spawn(pattern.obstaclePrefab, contentRoot, x, z);
                }
                else if (token == 'C')
                {
                    Spawn(pattern.coinPrefab, contentRoot, x, z);
                }
                else if (token == '>' || token == '<')
                {
                    var trap = Spawn(pattern.trapPrefab, contentRoot, x, z);

                    // Configure direction if your trap script supports it
                    var trapScript = trap.GetComponent<ShootingTrapHorizontal>();
                    if (trapScript != null)
                    {
                        // Right = +X, Left = -X
                        if (token == '>')
                            trapScript.SetDirection(Vector3.right);
                        else
                            trapScript.SetDirection(Vector3.left);
                    }
                }
                else if (token == 'G')
                {
                    if (floorLookup != null)
                        floorLookup.TryRemoveTile(lane, cell);
                }
            }
        }
    }

    private GameObject Spawn(GameObject prefab, Transform parent, float x, float z)
    {
        if (prefab == null) return null;

        var go = Instantiate(prefab, parent);
        go.transform.position = new Vector3(x, go.transform.position.y, z);
        go.transform.rotation = Quaternion.identity;
        return go;
    }
}
