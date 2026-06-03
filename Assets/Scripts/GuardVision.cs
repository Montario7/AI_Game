using UnityEngine;

public class GuardVision : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float viewDistance = 8f;
    [SerializeField] private float viewAngle = 60f;
    [SerializeField] private LayerMask obstacleMask;

    public bool CanSeePlayer { get; private set; }

  private void Update()
{
    CheckVision();

    if (CanSeePlayer)
    {
        Debug.Log("Guard sees the player!");
    }
}

    private void CheckVision()
    {
        CanSeePlayer = false;

        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f;

        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > viewDistance)
            return;

        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer > viewAngle * 0.5f)
            return;

        Vector3 rayOrigin = transform.position + Vector3.up * 1.0f;
        Vector3 rayDirection = (player.position + Vector3.up * 0.5f) - rayOrigin;

        if (Physics.Raycast(rayOrigin, rayDirection.normalized, out RaycastHit hit, viewDistance, ~0))
        {
            if (hit.transform == player)
            {
                CanSeePlayer = true;
            }
        }
    }
}