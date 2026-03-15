using UnityEngine;

public class MiningNode : MonoBehaviour
{
    [Header("Mining Sttings")]
    public int resourcesLeft = 5;

    private bool playerInRange = false;

    public void Mine()
    {
        if (resourcesLeft <= 0) return;

        resourcesLeft--;
        Debug.Log($"Mined 1 resource! {resourcesLeft} left in this node");
        Debug.Log("You recieved 1 ore!");

        if (resourcesLeft <= 0)
        {
            Debug.Log("Node is empty");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (playerInRange)
        {
            Mine();
        }

    }
}
