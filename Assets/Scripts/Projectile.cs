using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour
{
    [Header("Lifetime")]
    [SerializeField] float  _lifetimeSeconds = 1.0f;

    Transform               _ownerRoot;
    Collider                _myCollider;

    void Awake()
    {
        _myCollider = GetComponent<Collider>();
        Destroy(gameObject, _lifetimeSeconds);
    }


    /// <summary>
    /// Call immediately after Instantiate to set the owner and ignore owner collisions.
    /// </summary>
    public void Initialise(Transform owner)
    {
        _ownerRoot = owner;

        if (_ownerRoot == null || _myCollider == null) return;

        // Ignore collisions with any collider on the owner (and its children)
        var ownerColliders = _ownerRoot.GetComponentsInChildren<Collider>();
        foreach (var collider in ownerColliders)
        {
            if (collider == null) continue;
            Physics.IgnoreCollision(_myCollider, collider, true);
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision == null || collision.collider == null) return;

        if (IsOwnerCollision(collision.collider.transform)) return;

        gameObject.SetActive(false);
        //Destroy(gameObject);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        if (IsOwnerCollision(other.transform)) return;
        
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }


    private bool IsOwnerCollision(Transform other)
    {
        if (_ownerRoot == null || other == null) return false;

        // Treat anything under the ownerRoot as "owner"
        return other == _ownerRoot || other.IsChildOf(_ownerRoot);
    }
}
