
using UnityEngine;

public class KeyController : MonoBehaviour, IInteractable
{
    public string InteractionMessage => "Pick up the key";

    public void Interact()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GiveKey();
        }

        Debug.Log("Key collected!");

        Destroy(gameObject);
    }
}
