using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] float _degreesPerSecond = 180f;

    void Update()
    {
        transform.Rotate(0f, _degreesPerSecond * Time.deltaTime, 0f, Space.World);
    }
}
