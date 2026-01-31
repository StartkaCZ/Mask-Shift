using UnityEngine;

public enum HazardType
{
    Obstacle,
    Projectile,
    KillPlane
}

public class Hazard : MonoBehaviour
{
    public HazardType type = HazardType.Obstacle;

    [Min(0)]
    public int pointsOnNeutralise = 0;
}
