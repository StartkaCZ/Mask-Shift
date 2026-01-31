using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int    _value = 1;
    [SerializeField] float  _degreesPerSecond = 180f;

    void Update()
    {
        transform.Rotate(0f, _degreesPerSecond * Time.deltaTime, 0f, Space.World);
    }


    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        GameManager.Instance?.AddScore(_value);
        Destroy(gameObject);
    }
}
