using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField, Min(0.0f)]
    private float speed = 10;

    void FixedUpdate()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
