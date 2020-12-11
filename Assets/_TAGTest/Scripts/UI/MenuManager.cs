using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI telemetryText = null;


    void Awake()
    {
        gameObject.SetActive(false);
    }


    public void OpenMenu()
    {
        gameObject.SetActive(true);

        //Update Telemetry text
        if (telemetryText == null)
        {
            Debug.LogError("Missing ref to telemetry text");
            return;
        }
        telemetryText.text = $"Time Alive: {Mathf.RoundToInt(TelemetryManager.Instance.timeAlive)}\nEnemies Killed: {TelemetryManager.Instance.enemiesKilled}";
    }

    public void OnRestart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

}
