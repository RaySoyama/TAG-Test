using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab = null;

    [SerializeField]
    private GameObject playerPrefab = null;

    [SerializeField, Min(10.0f)]
    private float playerSpawnRadius = 50.0f;

    [SerializeField]
    private float currentSpawnRate = 10.0f;

    [SerializeField]
    private float logValueIncreaseOverTime = 0.1f;

    [SerializeField]
    private float maxSpawnRate = 3.0f;

    [SerializeField, ReadOnlyField]
    private float lastSpawnedTimeStamp = -100f;


    private float startSpawnRate = 0.0f;

    private void Start()
    {
        startSpawnRate = currentSpawnRate;
    }

    void Update()
    {
        if (currentSpawnRate < maxSpawnRate)
        {
            currentSpawnRate = maxSpawnRate;
        }
        else
        {
            currentSpawnRate = startSpawnRate - Mathf.Log(Time.time * logValueIncreaseOverTime); //this will still run when paused. heads up
        }


        if (Time.time - lastSpawnedTimeStamp > currentSpawnRate)
        {
            lastSpawnedTimeStamp = Time.time;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Vector2 randPos = Random.insideUnitCircle.normalized * playerSpawnRadius;
        Instantiate(enemyPrefab, new Vector3(randPos.x, 0, randPos.y), Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(playerPrefab.transform.position, playerSpawnRadius);
    }
}
