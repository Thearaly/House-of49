
using UnityEngine;

public class KeyCollect : MonoBehaviour
{
    private bool playerNearby = false;
    private bool keyCollected = false;

    private void Update()
    {
        if (playerNearby && !keyCollected && Input.GetKeyDown(KeyCode.E))
        {
            CollectKey();
        }
    }

    private void CollectKey()
    {
        keyCollected = true;

        Debug.Log("You found the key!");

        // Hide the key after collection
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            Debug.Log("Press E to collect the key");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}
