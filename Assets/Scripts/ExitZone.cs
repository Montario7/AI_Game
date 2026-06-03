using UnityEngine;

public class ExitZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameManager.Instance.HasArtifact)
        {
            GameManager.Instance.PlayerWon();
        }
        else
        {
            Debug.Log("The exit is locked. Find the artifact first.");
        }
    }
}