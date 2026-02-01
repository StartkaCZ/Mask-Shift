using UnityEngine;
using UnityEngine.InputSystem;

public enum MaskType
{
    Stone,
    Mirror,
}


public class MaskController : MonoBehaviour
{
    [SerializeField] MaskType       _currentMask = MaskType.Stone;

    public MaskType                 CurrentMask => _currentMask;

    // Optional: simple event so other systems can react later (UI, curse, etc.)
    public System.Action<MaskType>  OnMaskChanged;

    public void SetMask(MaskType mask)
    {
        if (_currentMask == mask)
        {
            if (mask == MaskType.Mirror)
                _currentMask = MaskType.Stone;
            else
                _currentMask = MaskType.Mirror;
        }
        else
            _currentMask = mask;

        OnMaskChanged?.Invoke(_currentMask);
    }

    // Input System (Send Messages) hooks
    public void OnMaskStone(InputValue value) { if (value.isPressed) SetMask(MaskType.Stone); }
    public void OnMaskMirror(InputValue value) { if (value.isPressed) SetMask(MaskType.Mirror); }
}
