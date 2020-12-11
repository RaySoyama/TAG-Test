using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private KeyCode keyMoveUp = KeyCode.W;
    [SerializeField]
    private KeyCode keyMoveDown = KeyCode.S;
    [SerializeField]
    private KeyCode keyMoveLeft = KeyCode.A;
    [SerializeField]
    private KeyCode keyMoveRight = KeyCode.D;

    [SerializeField]
    private KeyCode keyAction1 = KeyCode.E;


    [SerializeField, ReadOnlyField]
    private Vector3 movementInputCache = new Vector3();
    [SerializeField, ReadOnlyField]
    private Vector3 mousePosCache = new Vector3();

    [SerializeField]
    private Entity currentEntity = null;


    [SerializeField]
    private Camera mainCam = null;

    private void Update()
    {
        if (currentEntity == null)
        {
            Debug.LogError("INPUT MANAGER MISSING ENTITY TO CONTROL");
            return;
        }


        movementInputCache = Vector3.zero;
        mousePosCache = Vector3.zero;

        if (Input.GetKey(keyMoveUp) == true)
        {
            movementInputCache += Vector3.forward;
        }

        if (Input.GetKey(keyMoveDown) == true)
        {
            movementInputCache += Vector3.back;
        }

        if (Input.GetKey(keyMoveLeft) == true)
        {
            movementInputCache += Vector3.left;
        }

        if (Input.GetKey(keyMoveRight) == true)
        {
            movementInputCache += Vector3.right;
        }

        //normalize
        movementInputCache.Normalize();

        //Call Movement Function in Entity
        if (movementInputCache != Vector3.zero)
        {
            currentEntity.Move(movementInputCache);
        }

        mousePosCache = Input.mousePosition;

        if (mainCam == null)
        {
            Debug.LogError("Missing Reference to Main Cam in Input Manager");
        }
        else
        {
            currentEntity.LookAt(mainCam.ScreenToWorldPoint(mousePosCache) - mainCam.transform.position);
        }


        if (Input.GetKeyDown(keyAction1) == true || Input.GetMouseButtonDown(0)) //hard coded left click
        {
            currentEntity.Attack();
        }

    }
}
