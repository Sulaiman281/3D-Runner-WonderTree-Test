using UnityEngine;

public class PickUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        // increase score system
        GameController.instance.MoneyPicked += 100;
        Destroy(gameObject);
    }
}
