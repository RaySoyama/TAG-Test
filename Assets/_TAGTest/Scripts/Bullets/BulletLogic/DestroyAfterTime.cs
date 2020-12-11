using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField]
    private float lifetime = 5.0f;

    [SerializeField, ReadOnlyField]
    private float spawnTimeStamp = 0;

    private void Start()
    {
        spawnTimeStamp = Time.time;
    }

    void FixedUpdate()
    {
        if (Time.time - spawnTimeStamp > lifetime)
        {
            Destroy(gameObject);
        }
    }
}
