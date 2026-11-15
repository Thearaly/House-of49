using UnityEngine;

// Attach to: Main Camera
public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;                         // drag Player here
    public Vector3 targetOffset = new Vector3(0, 1.5f, 0); // look at the head, not the feet

    [Header("Distance")]
    public float distance = 5f;
    public float minDistance = 2f;
    public float maxDistance = 8f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 3f;
    public float minPitch = -20f;
    public float maxPitch = 60f;

    [Header("Smoothing / Collision")]
    public float followSmoothing = 12f;
    public LayerMask obstacleMask = ~0;              // walls the camera should not go through
    public float collisionRadius = 0.3f;

    private float yaw;
    private float pitch = 20f;

    void Start()
    {
        yaw = transform.eulerAngles.y;
        LockCursor(true);
    }

    void LateUpdate()
    {
        if (target == null) return;

        bool playing = GameManager.Instance == null ||
                       GameManager.Instance.CurrentState == GameState.Playing;
        LockCursor(playing);

        if (playing)
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            distance -= Input.GetAxis("Mouse ScrollWheel") * 3f;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 focus = target.position + targetOffset;

        // Pull the camera in if a wall is between it and the player
        float finalDistance = distance;
        Vector3 backDir = rotation * Vector3.back;
        if (Physics.SphereCast(focus, collisionRadius, backDir, out RaycastHit hit, distance, obstacleMask,
                               QueryTriggerInteraction.Ignore) && !hit.transform.IsChildOf(target))
        {
            finalDistance = Mathf.Max(hit.distance, 0.5f);
        }

        Vector3 desiredPos = focus + backDir * finalDistance;
        transform.position = Vector3.Lerp(transform.position, desiredPos, followSmoothing * Time.deltaTime);
        transform.rotation = rotation;
    }

    void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}