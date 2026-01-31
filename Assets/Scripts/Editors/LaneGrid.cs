using UnityEngine;

public class LaneGrid : MonoBehaviour
{
    [Header("Grid Shape")]
    [SerializeField] int        _lanes = 3;                 // fixed requirement
    [SerializeField] int        _visibleCells = 20;         // how far you see

    [Header("Cell Spacing")]
    [Tooltip("Centre-to-centre spacing between lanes (X axis).")]
    [SerializeField] float      _laneSpacing = 2f;

    [Tooltip("Centre-to-centre spacing between cells (Z axis).")]
    [SerializeField] float      _cellSpacing = 2f;

    [Tooltip("Optional extra gap added to both laneSpacing and cellSpacing.")]
    [SerializeField] float      _cellGap = 0.1f;

    [Header("Prefabs")]
    [SerializeField] GameObject _floorTilePrefab;

    [Header("Generated Root")]
    [SerializeField] Transform  _generatedRoot;

    void OnValidate()
    {
        _lanes = Mathf.Max(1, _lanes);
        _visibleCells = Mathf.Max(1, _visibleCells);
        _laneSpacing = Mathf.Max(0.1f, _laneSpacing);
        _cellSpacing = Mathf.Max(0.1f, _cellSpacing);
        _cellGap = Mathf.Max(0f, _cellGap);
    }

    public void RegenerateFloor()
    {
        ClearGenerated();

        if (_floorTilePrefab == null)
        {
            Debug.LogWarning("LaneGrid: Assign a floorTilePrefab first.");
            return;
        }

        EnsureRoot();

        float half = (_lanes - 1) / 2f;
        float xStep = _laneSpacing + _cellGap;
        float zStep = _cellSpacing + _cellGap;

        for (int z = 0; z < _visibleCells; z++)
        {
            for (int lane = 0; lane < _lanes; lane++)
            {
                float x = (lane - half) * xStep;
                float posZ = z * zStep;

                var tile = Instantiate(_floorTilePrefab, _generatedRoot);
                tile.name = $"Floor_L{lane}_C{z}";
                tile.transform.localPosition = new Vector3(x, 0f, posZ);
                tile.transform.localRotation = Quaternion.identity;
            }
        }
    }

    public void ClearGenerated()
    {
        if (_generatedRoot == null) return;

        // Safe for both edit and play mode
        for (int i = _generatedRoot.childCount - 1; i >= 0; i--)
        {
            var child = _generatedRoot.GetChild(i).gameObject;

            if (Application.isPlaying)
                Destroy(child);
            else
                DestroyImmediate(child);
        }
    }

    private void EnsureRoot()
    {
        if (_generatedRoot != null) return;

        var rootObj = new GameObject("Generated");
        rootObj.transform.SetParent(transform, false);
        _generatedRoot = rootObj.transform;
    }
}
