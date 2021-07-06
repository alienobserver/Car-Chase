using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float speed = 7.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Transform playerCameraParent;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 60.0f;

    Vector3 moveDirection = Vector3.zero;
    Vector2 rotation = Vector2.zero;

    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        rotation.y = transform.eulerAngles.y;
    }

    void Update()
    {
        // Player and Camera rotation
        if (canMove)
        {
            rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
            rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
            playerCameraParent.localRotation = Quaternion.Euler(rotation.x, rotation.y, 0);
            float x = Mathf.Sin( Input.GetAxis("Mouse X") * Mathf.PI / 6 ) * 6;
            float z = Mathf.Cos(Input.GetAxis("Mouse Y") * Mathf.PI / 6) * 6;
            float y = 3;
            Vector3 movement = new Vector3(x, y, z);
            playerCameraParent.position = movement;
            Debug.Log(movement);
        }
    }
}