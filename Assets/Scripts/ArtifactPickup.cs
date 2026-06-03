using UnityEngine;

public class ArtifactPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.CollectArtifact();
            Destroy(gameObject);
        }
    }
}