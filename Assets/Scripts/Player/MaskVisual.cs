using UnityEngine;

[RequireComponent(typeof(MaskController))]
public class MaskVisual : MonoBehaviour
{
    [Header("Renderer")]
    [SerializeField] Renderer   _maskRenderer;

    [Header("Materials")]
    [SerializeField] Material   _stoneMat;
    [SerializeField] Material   _featherMat;
    [SerializeField] Material   _mirrorMat;
    [SerializeField] Material   _oracleMat;

    MaskController              _maskController;

    private void Awake()
    {
        _maskController = GetComponent<MaskController>();

        if (_maskRenderer == null)
        {
            var child = transform.GetChild(0);
            if (child != null) _maskRenderer = child.GetComponent<Renderer>();
        }
    }

    private void OnEnable()
    {
        if (_maskController != null)
            _maskController.OnMaskChanged += Apply;
    }

    private void OnDisable()
    {
        if (_maskController != null)
            _maskController.OnMaskChanged -= Apply;
    }

    private void Start()
    {
        Apply(_maskController != null ? _maskController.CurrentMask : MaskType.Stone);
    }

    private void Apply(MaskType mask)
    {
        if (_maskRenderer == null) return;

        Material m = mask switch
        {
            MaskType.Stone => _stoneMat,
            MaskType.Feather => _featherMat,
            MaskType.Mirror => _mirrorMat,
            _ => _stoneMat
        };

        if (m != null)
            _maskRenderer.sharedMaterial = m;
    }
}
