using UnityEngine;

[ExecuteInEditMode]
public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject player = null;

    [SerializeField]
    private Vector3 offset = new Vector3();

    [SerializeField]
    private bool forceLookAt = false;

    void Update()
    {
        if (player == null)
        {
            Debug.LogError("MISSING REF TO PLAYER. CAMERA CAN'T FOLLOW");
            return;
        }

        transform.position = player.transform.position + offset;

        if (forceLookAt == true)
        {
            transform.LookAt(player.transform.position);
        }
    }
}
