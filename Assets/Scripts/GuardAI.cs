using UnityEngine;
using UnityEngine.AI;

public class GuardAI : MonoBehaviour
{
    private enum GuardState
    {
        Patrol,
        Chase,
        Search
    }

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private GuardVision guardVision;

    [Header("Patrol Settings")]
    [SerializeField] private float waitTimeAtPoint = 2f;

    [Header("Chase/Search Settings")]
    [SerializeField] private float loseSightDelay = 1.5f;
    [SerializeField] private float searchWaitTime = 2f;

    private NavMeshAgent agent;
    private GuardState currentState = GuardState.Patrol;

    private int currentPointIndex = 0;
    private float waitTimer;
    private bool isWaiting;

    private float loseSightTimer;
    private Vector3 lastKnownPlayerPosition;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[currentPointIndex].position);
        }
    }

    private void Update()
    {
        switch (currentState)
        {
            case GuardState.Patrol:
                UpdatePatrol();

                if (guardVision != null && guardVision.CanSeePlayer)
                {
                    lastKnownPlayerPosition = player.position;
                    loseSightTimer = loseSightDelay;
                    currentState = GuardState.Chase;
                }
                break;

            case GuardState.Chase:
                UpdateChase();
                break;

            case GuardState.Search:
                UpdateSearch();
                break;
        }
    }

    private void UpdatePatrol()
    {
        if (patrolPoints.Length == 0) return;

        if (!isWaiting && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            isWaiting = true;
            waitTimer = waitTimeAtPoint;
        }

        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0f)
            {
                isWaiting = false;
                GoToNextPoint();
            }
        }
    }

    private void UpdateChase()
    {
        if (player == null) return;

        if (guardVision != null && guardVision.CanSeePlayer)
        {
            lastKnownPlayerPosition = player.position;
            loseSightTimer = loseSightDelay;
        }
        else
        {
            loseSightTimer -= Time.deltaTime;
        }

        NavMeshHit hit;
        if (NavMesh.SamplePosition(lastKnownPlayerPosition, out hit, 2f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }

        if (loseSightTimer <= 0f)
        {
            currentState = GuardState.Search;
            waitTimer = searchWaitTime;
        }
    }

    private void UpdateSearch()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0f)
            {
                currentState = GuardState.Patrol;
                isWaiting = false;

                if (patrolPoints.Length > 0)
                {
                    agent.SetDestination(patrolPoints[currentPointIndex].position);
                }
            }
        }

        if (guardVision != null && guardVision.CanSeePlayer)
        {
            lastKnownPlayerPosition = player.position;
            loseSightTimer = loseSightDelay;
            currentState = GuardState.Chase;
        }
    }

    private void GoToNextPoint()
    {
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        agent.SetDestination(patrolPoints[currentPointIndex].position);
    }
}