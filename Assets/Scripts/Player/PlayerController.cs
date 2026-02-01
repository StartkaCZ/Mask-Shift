using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class RunnerPlayerController : MonoBehaviour
{
    [Header("Lane Settings")]
    [SerializeField] int    _lanes = 3;
    [SerializeField] float  _laneStep = 2.1f;            // MUST match LaneGrid x-step
    [SerializeField] float  _laneSwitchSpeed = 12f;      // units/sec

    [Header("Fall Settings")]
    [SerializeField] float  _gravity = -25f;

    [Header("Forward (optional for now)")]
    [SerializeField] float  _forwardSpeed = 15f;          // set >0 later for runner feel

    CharacterController     _controller;
    MaskController          _mask;

    int                     _currentLane = 1;           // for 3 lanes: 0,1,2 => start in centre
    float                   _verticalVelocity;

    // input handling (prevents repeated lane shifts when holding a key)
    float                   _moveXRaw;
    bool                    _moveHeld;


    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _mask = GetComponent<MaskController>();
        _currentLane = Mathf.Clamp(_currentLane, 0, _lanes - 1);
    }


    void Update()
    {
        // Determine target X based on lane
        float half = (_lanes - 1) / 2f;
        float targetX = (_currentLane - half) * _laneStep;

        // Smoothly move towards target lane X
        float currentX = transform.position.x;
        float newX = Mathf.MoveTowards(currentX, 
                                       targetX, 
                                       _laneSwitchSpeed * Time.deltaTime);
        float deltaX = newX - currentX;

        // Gravity
        if (_controller.isGrounded && _verticalVelocity < 0f)
            _verticalVelocity = -2f; // keeps grounded reliably

        _verticalVelocity += _gravity * Time.deltaTime;

        Vector3 move = new Vector3(deltaX, 
                                   _verticalVelocity * Time.deltaTime,
                                   _forwardSpeed * Time.deltaTime);
        _controller.Move(move);

        // Reset "held" state when stick/keys return to neutral
        if (Mathf.Abs(_moveXRaw) < 0.2f)
            _moveHeld = false;
    }


    // Called automatically by PlayerInput (Send Messages) for the "Move" action
    public void OnMove(InputValue value)
    {
        Vector2 moveValue = value.Get<Vector2>();
        _moveXRaw = moveValue.x;

        if (_moveHeld) return;

        if (_moveXRaw > 0.5f)
        {
            _currentLane = Mathf.Min(_currentLane + 1, _lanes - 1);
            _moveHeld = true;
        }
        else if (_moveXRaw < -0.5f)
        {
            _currentLane = Mathf.Max(_currentLane - 1, 0);
            _moveHeld = true;
        }
    }
}
