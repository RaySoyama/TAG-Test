using UnityEngine;

[ExecuteInEditMode]
public class Billboard : MonoBehaviour
{
    [SerializeField]
    private bool runInEditor = true;

    [SerializeField]
    private bool isParallelToGround = false;

    void Update()
    {
        if (Application.isPlaying == true || runInEditor == true)
        {
            if (isParallelToGround == true)
            {
                transform.localEulerAngles = new Vector3(90, 0, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(45, 0, 0);
            }
        }
    }
}
