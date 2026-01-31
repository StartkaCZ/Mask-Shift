using UnityEngine;

public class ShootingTrap : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] GameObject     _projectilePrefab;
    [SerializeField] Transform      _firePoint;

    [Header("Timing")]
    [SerializeField] float          _fireInterval = 1.2f;

    [Header("Projectile Motion")]
    [SerializeField] float          _projectileSpeed = 10f;
    
    Vector3                         _direction = Vector3.back;
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

        var gameObject = Instantiate(_projectilePrefab, _firePoint.position, Quaternion.identity);
        var projectileScript = gameObject.GetComponent<Projectile>();

        if (projectileScript != null)
            projectileScript.Initialise(transform);

        var rigibBody = gameObject.GetComponent<Rigidbody>();
        if (rigibBody != null)
            rigibBody.linearVelocity = _direction.normalized * 
                                            _projectileSpeed; // Unity 6 uses linearVelocity
        else
            gameObject.transform.position += _direction.normalized * 
                                                _projectileSpeed * 
                                                Time.deltaTime;
    }
}
