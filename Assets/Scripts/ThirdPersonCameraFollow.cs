
using UnityEngine;

public class ThirdPersonCameraFollow : MonoBehaviour
{
    public Transform target;

    public float distance = 4.5f;
    public float height = 2.0f;
    public float lookHeight = 1.3f;
    public float mouseSensitivity = 2f;

    private float yaw;
    private float pitch = 12f;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("CameraFollow: Assign the Player target.");
            enabled = false;
            return;
        }

        yaw = target.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void LateUpdate()
    {
        if (target == null)
            return;

        // Keep the cursor free while a UI panel is open.
        LevelManager levelManager =
            FindFirstObjectByType<LevelManager>();

        if (levelManager != null && levelManager.IsLevelCompleted)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetMouseButtonDown(0) &&
            Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, -10f, 45f);
        }

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 offset =
            rotation * Vector3.back * distance +
            Vector3.up * height;

        transform.position = target.position + offset;

        transform.LookAt(
            target.position + Vector3.up * lookHeight
        );
    }

}
