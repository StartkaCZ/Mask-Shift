using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform  _target;
    [SerializeField] Vector3    _offset = new Vector3(0f, 6f, -10f);
    [SerializeField] Vector3    _targetOffset = new Vector3(0f, 1f, 6f);
    [SerializeField] float      _followSpeed = 8f;


    void LateUpdate()
    {
        if (_target == null) return;

        Vector3 desired = _target.position + _offset;
        transform.position = Vector3.Lerp(transform.position, desired, _followSpeed * Time.deltaTime);
        transform.LookAt(_target.position + _targetOffset);
    }
}
