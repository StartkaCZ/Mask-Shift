using UnityEngine;

public class ShootingTrapHorizontal : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] GameObject     _projectilePrefab;
    [SerializeField] Transform      _firePoint;

    [Header("Timing")]
    [SerializeField] float          _fireInterval = 1.2f;

    [Header("Projectile Motion")]
    [SerializeField] float          _projectileSpeed = 10f;
    [SerializeField] Vector3        _direction = Vector3.right; // set left/right in inspector

    float                           _timer;


    void Reset()
    {
        _direction = Vector3.right;
        _fireInterval = 1.2f;
        _projectileSpeed = 10f;
    }


    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _fireInterval)
        {
            _timer = 0f;
            Fire();
        }
    }


    private void Fire()
    {
        if (_projectilePrefab == null || _firePoint == null) return;

        var go = Instantiate(_projectilePrefab, _firePoint.position, Quaternion.identity);

        var rb = go.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = _direction.normalized * _projectileSpeed; // Unity 6 uses linearVelocity
        }
        else
        {
            // fallback if someone removed rigidbody
            go.transform.position += _direction.normalized * _projectileSpeed * Time.deltaTime;
        }
    }


    public void SetDirection(Vector3 dir)
    {
        _direction = dir;

        if (_direction == Vector3.right)
            transform.localEulerAngles = Vector3.zero;
        else
        {
            transform.GetChild(0).localEulerAngles = new Vector3(0, 180, 0);
        }
    }
}
