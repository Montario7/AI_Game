using UnityEngine;

public class GuardCatch : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float catchDistance = 1.5f;

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= catchDistance)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.PlayerCaught();
            }
            else
            {
                Debug.LogError("GameManager.Instance is NULL!");
            }
        }
    }
}