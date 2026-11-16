using UnityEngine;

// Attach to: EVERY door leaf (the door model itself).
// Drag an empty object placed on the hinge edge into "Hinge Point".
public class AutoDoor : MonoBehaviour
{
    [Header("Opening")]
    public float openAngle = 90f;               // flip the sign if it opens the wrong way
    public float openSpeed = 3f;
    public float openDistance = 6f;             // how close the player must be to open it

    [Header("Closing")]
    public bool closeAfterPassing = true;       // close once the player has walked through
    public float passDistance = 1.5f;           // how far past the door before it closes
    public bool lockAfterPassing = false;       // tick this if the door must stay shut afterwards

    [Header("Hinge")]
    public Transform hingePoint;                // empty object on the hinge edge

    [Header("Lock")]
    public bool isLocked = false;

    private Transform player;
    private Vector3 closedPos;
    private Quaternion closedRot;
    private Vector3 hingeWorld;
    private Vector3 outsideDir;                 // direction from the door to where the player came from
    private bool shouldOpen;
    private float openAmount;                   // 0 = closed, 1 = open

    void Start()
    {
        PlayerController pc = FindFirstObjectByType<PlayerController>();
        if (pc != null) player = pc.transform;

        closedPos = transform.position;
        closedRot = transform.rotation;
        hingeWorld = hingePoint != null ? hingePoint.position : transform.position;
    }

    void Update()
    {
        if (player != null && !isLocked)
        {
            Vector3 rel = player.position - hingeWorld;
            rel.y = 0f;

            if (!shouldOpen)
            {
                // Open when the player comes close
                if (rel.magnitude <= openDistance)
                {
                    shouldOpen = true;
                    outsideDir = rel.normalized;   // remember which side the player came from
                }
            }
            else if (closeAfterPassing)
            {
                // Close when the player is on the other side of the door
                float side = Vector3.Dot(rel, outsideDir);
                if (side < -passDistance)
                {
                    shouldOpen = false;
                    if (lockAfterPassing) isLocked = true;
                }
            }
        }

        openAmount = Mathf.MoveTowards(openAmount, shouldOpen ? 1f : 0f, openSpeed * 0.3f * Time.deltaTime);

        // Turn around the hinge, always from the closed pose, so it never drifts
        Quaternion turn = Quaternion.Euler(0f, openAngle * openAmount, 0f);
        transform.position = hingeWorld + turn * (closedPos - hingeWorld);
        transform.rotation = turn * closedRot;
    }

    void OnDrawGizmosSelected()
    {
        Vector3 p = hingePoint != null ? hingePoint.position : transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(p, 0.08f);   // red dot = where the hinge is
    }

    public void Lock() { isLocked = true; shouldOpen = false; }
    public void Unlock() { isLocked = false; }
}