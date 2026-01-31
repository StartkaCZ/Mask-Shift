using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(MaskController))]
public class PlayerCollisionHandler : MonoBehaviour
{
    MaskController mask;


    void Awake()
    {
        mask = GetComponent<MaskController>();
    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider == null) return;
        if (!hit.collider.CompareTag("Hazard")) return;

        HandleHazard(hit.collider);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other == null) return;
        if (!other.CompareTag("Hazard")) return;

        HandleHazard(other);
    }


    void HandleHazard(Collider collider)
    {
        var hazard = collider.GetComponent<Hazard>();

        // If no Hazard component exists, treat as lethal hazard.
        if (hazard == null)
        {
            GameManager.Instance?.GameOver();
            return;
        }

        // Kill plane is always lethal
        if (hazard.type == HazardType.KillPlane)
        {
            GameManager.Instance?.GameOver();
            return;
        }

        // Stone smashes obstacles
        if (mask.CurrentMask == MaskType.Stone && hazard.type == HazardType.Obstacle)
        {
            AwardAndRemove(hazard, collider.gameObject);
            return;
        }

        // Mirror eats projectiles
        if (mask.CurrentMask == MaskType.Mirror && hazard.type == HazardType.Projectile)
        {
            AwardAndRemove(hazard, collider.gameObject);
            return;
        }

        // Otherwise: lethal
        GameManager.Instance?.GameOver();
    }

    private void AwardAndRemove(Hazard hazard, GameObject gameObject)
    {
        GameManager.Instance?.AddScore(hazard.pointsOnNeutralise);

        // Prevent double-award if we collide for multiple frames
        var collider = gameObject.GetComponent<Collider>();
        if (collider != null) collider.enabled = false;

        Destroy(gameObject);
    }
}
