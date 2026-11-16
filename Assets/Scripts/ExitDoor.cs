using UnityEngine;
using TMPro;

// Attach to: an empty "ExitDoorTrigger" object in front of the exit door
// (Box Collider with "Is Trigger" ticked, big enough to cover the area in front of the door)
public class ExitDoor : MonoBehaviour
{
    [Header("Doors to unlock (drag the AutoDoor leaves here)")]
    public AutoDoor[] doors;

    [Header("Message (optional)")]
    public TMP_Text messageText;
    public string lockedMessage = "The door is locked. Find the key!";
    public string unlockedMessage = "The door is unlocked!";
    public float messageTime = 2.5f;

    [Header("Testing")]
    public bool pressKToGetKey = true;   // press K in Play mode to fake getting the key

    private bool unlocked;
    private float hideTime;

    void Start()
    {
        // Make sure the doors start locked
        foreach (AutoDoor d in doors) if (d != null) d.isLocked = true;
        ShowMessage("");
    }

    void Update()
    {
        if (pressKToGetKey && Input.GetKeyDown(KeyCode.K) && GameManager.Instance != null)
            GameManager.Instance.GiveKey();

        if (messageText != null && messageText.text != "" && Time.time > hideTime)
            messageText.text = "";
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() == null) return;
        if (unlocked) return;

        bool hasKey = GameManager.Instance != null && GameManager.Instance.HasKey;

        if (hasKey)
        {
            unlocked = true;
            foreach (AutoDoor d in doors) if (d != null) d.Unlock();   // AutoDoor opens by itself now
            ShowMessage(unlockedMessage);
        }
        else
        {
            ShowMessage(lockedMessage);
        }
    }

    // Walking away and coming back shows the locked message again
    void OnTriggerExit(Collider other) { }

    void ShowMessage(string msg)
    {
        if (messageText == null) return;
        messageText.text = msg;
        hideTime = Time.time + messageTime;
    }
}