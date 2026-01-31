using System.Collections.Generic;
using UnityEngine;

public class SegmentSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform      _player;
    [SerializeField] GameObject     _segmentPrefab;

    [Tooltip("Put your pre-placed segments under this transform (recommended).")]
    [SerializeField] Transform      _segmentsRoot;

    [Header("Segment Settings")]
    [SerializeField] int            _segmentCells = 10;
    [SerializeField] float          _cellSpacing = 4f;

    [Header("Streaming")]
    [SerializeField] int            _segmentsAhead = 2;
    [SerializeField] int            _segmentsBehind = 1;

    [Header("Bootstrap")]
    [SerializeField] bool           _bootstrapFromScene = true;

    float                           _segmentLength;
    int                             _nextSegmentIndex;

    readonly Queue<GameObject>      _spawned = new Queue<GameObject>();


    void Awake()
    {
        _segmentLength = _segmentCells * _cellSpacing;
    }


    void Start()
    {
        if (_player == null || _segmentPrefab == null)
        {
            Debug.LogError("SegmentSpawner: Assign player and segmentPrefab.");
            enabled = false;
            return;
        }

        if (_segmentsRoot == null)
            _segmentsRoot = transform;

        if (_bootstrapFromScene)
            BootstrapExistingSegments();
        else
            SpawnInitialSegments();
    }


    void Update()
    {
        int playerIndex = Mathf.FloorToInt(_player.position.z / _segmentLength);

        // Ensure enough segments ahead
        while (_nextSegmentIndex <= playerIndex + _segmentsAhead)
        {
            SpawnAtIndex(_nextSegmentIndex);
            _nextSegmentIndex++;
        }

        // Despawn segments too far behind
        while (_spawned.Count > 0)
        {
            var oldest = _spawned.Peek();
            var idx = oldest.GetComponent<SegmentIndex>().Index;

            if (idx < playerIndex - _segmentsBehind)
            {
                Destroy(_spawned.Dequeue());
            }
            else
            {
                break;
            }
        }
    }



    private void BootstrapExistingSegments()
    {
        _spawned.Clear();

        var list = new List<SegmentIndex>();

        for (int i = 0; i < _segmentsRoot.childCount; i++)
        {
            var child = _segmentsRoot.GetChild(i);
            if (!child.gameObject.activeInHierarchy) continue;

            var segIdx = child.GetComponent<SegmentIndex>();
            if (segIdx == null) segIdx = child.gameObject.AddComponent<SegmentIndex>();

            // Derive index from Z position (expects segments aligned to multiples of segmentLength)
            segIdx.Index = Mathf.RoundToInt(child.position.z / _segmentLength);

            // Optional: snap perfectly onto the grid to avoid drift/gaps
            child.position = new Vector3(0f, 0f, segIdx.Index * _segmentLength);

            child.name = $"Segment_{segIdx.Index}";
            list.Add(segIdx);
        }

        // Sort by index (back → front) so our queue order is correct
        list.Sort((a, b) => a.Index.CompareTo(b.Index));

        int maxIndex = int.MinValue;

        foreach (var seg in list)
        {
            _spawned.Enqueue(seg.gameObject);
            if (seg.Index > maxIndex) maxIndex = seg.Index;
        }

        // Next spawn continues after the last pre-placed segment
        _nextSegmentIndex = (maxIndex == int.MinValue) ? 0 : maxIndex + 1;
    }


    private void SpawnInitialSegments()
    {
        // Spawn [-segmentsBehind .. +segmentsAhead] around the player start
        int startIndex = -_segmentsBehind;
        int endIndex = _segmentsAhead;

        for (int idx = startIndex; idx <= endIndex; idx++)
        {
            SpawnAtIndex(idx);
        }

        _nextSegmentIndex = endIndex + 1;
    }


    private void SpawnAtIndex(int index)
    {
        float z = index * _segmentLength;

        var seg = Instantiate(_segmentPrefab, new Vector3(0f, 0f, z), Quaternion.identity, _segmentsRoot);
        seg.name = $"Segment_{index}";

        var segIdx = seg.GetComponent<SegmentIndex>();
        if (segIdx == null) segIdx = seg.AddComponent<SegmentIndex>();
        segIdx.Index = index;

        _spawned.Enqueue(seg);
    }
}
