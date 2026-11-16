using UnityEngine;

// Attach to: an empty "DoorTrigger" object placed in front of the door
// (needs a Box Collider with "Is Trigger" ticked)
public class DoorController : MonoBehaviour
{
    [Header("Door leaves (the hinge objects that rotate)")]
    public Transform leftDoor;
    public Transform rightDoor;          // leave empty for a single door

    [Header("Opening")]
    public float leftOpenAngle = -90f;   // flip the sign if it opens the wrong way
    public float rightOpenAngle = 90f;
    public float openSpeed = 3f;
    public bool closeWhenPlayerLeaves = false;

    [Header("Lock")]
    public bool isLocked = false;        // ExitDoor can set this later

    private Quaternion leftClosed, rightClosed, leftOpen, rightOpen;
    private bool shouldOpen;

    void Start()
    {
        if (leftDoor != null)
        {
            leftClosed = leftDoor.localRotation;
            leftOpen = leftClosed * Quaternion.Euler(0f, leftOpenAngle, 0f);
        }
        if (rightDoor != null)
        {
            rightClosed = rightDoor.localRotation;
            rightOpen = rightClosed * Quaternion.Euler(0f, rightOpenAngle, 0f);
        }
    }

    void Update()
    {
        float t = openSpeed * Time.deltaTime;

        if (leftDoor != null)
            leftDoor.localRotation = Quaternion.Slerp(leftDoor.localRotation, shouldOpen ? leftOpen : leftClosed, t);
        if (rightDoor != null)
            rightDoor.localRotation = Quaternion.Slerp(rightDoor.localRotation, shouldOpen ? rightOpen : rightClosed, t);
    }

    void OnTriggerEnter(Collider other)
    {
        if (isLocked) return;
        if (other.GetComponent<PlayerController>() != null)
            shouldOpen = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (closeWhenPlayerLeaves && other.GetComponent<PlayerController>() != null)
            shouldOpen = false;
    }

    // For teammates' scripts (e.g. ExitDoor)
    public void Lock() { isLocked = true; shouldOpen = false; }
    public void Unlock() { isLocked = false; }
    public void Open() { shouldOpen = true; }
}