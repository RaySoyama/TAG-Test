using UnityEngine;

public class TelemetryManager : MonoBehaviour
{
    private static TelemetryManager instance = null;
    public static TelemetryManager Instance
    {
        get
        {
            return instance;
        }
    }

    [ReadOnlyField]
    public float timeAlive = 0.0f;

    [ReadOnlyField]
    public int enemiesKilled = 0;

    public Entity player = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void FixedUpdate()
    {
        //I know this isn't ideal, but since we dont have a "Game Manager"
        if (player.CurrentHealth > 0)
        {
            timeAlive += Time.fixedDeltaTime;
        }
    }
}
