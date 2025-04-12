using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private Vector3 startPosition;
    private Vector3 lastSeenPosition;
    private bool playerVisible = false;
    private bool searching = false;

    public float visionAngle = 40f;
    public float visionRange = 10f;
    public float rotationSpeed = 120f;
    public Transform[] points;

    private float rotateAmount = 0f;
    private int currentPointIndex = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;

        if (points.Length > 0)
        {
            agent.SetDestination(points[currentPointIndex].position);
        }
        else
        {
            agent.isStopped = true;
        }
    }

    void Update()
    {
        if (points.Length == 0 || player == null) return;

        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        float angle = Vector3.Angle(transform.forward, directionToPlayer.normalized);

        if (distanceToPlayer <= visionRange && angle < visionAngle / 2f && CanSeePlayer())
        {
            playerVisible = true;
            lastSeenPosition = player.position;
            searching = false;
            agent.SetDestination(player.position);
        }
        else if (playerVisible)
        {
            playerVisible = false;
            searching = true;
            agent.SetDestination(lastSeenPosition);
        }
        else if (searching)
        {
            float distance = Vector3.Distance(transform.position, lastSeenPosition);
            if (distance < 0.5f)
            {
                RotateInPlace();
            }
        }
        else
        {
            float distanceToCurrentPoint = Vector3.Distance(transform.position, points[currentPointIndex].position);
            if (distanceToCurrentPoint < 1f)
            {
                currentPointIndex = (currentPointIndex + 1) % points.Length;
                agent.SetDestination(points[currentPointIndex].position);
            }
        }

        // Animatsiya: yurayotganida Walk true, to‘xtaganida yoki aylanayotganida false
        bool isMoving = agent.velocity.magnitude > 0.1f && !searching;
        animator.SetBool("Walk", isMoving);
    }

    bool CanSeePlayer()
    {
        Vector3 origin = transform.position + Vector3.up * 1.5f;
        Vector3 dir = (player.position - origin).normalized;

        if (Physics.Raycast(origin, dir, out RaycastHit hit, visionRange))
        {
            return hit.collider.CompareTag("Player");
        }
        return false;
    }

    void RotateInPlace()
    {
        float step = rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, step);
        rotateAmount += step;

        animator.SetBool("Walk", false); // Aylanayotganida yurmasin

        if (rotateAmount >= 360f)
        {
            rotateAmount = 0f;
            searching = false;
            currentPointIndex = (currentPointIndex + 1) % points.Length;
            agent.SetDestination(points[currentPointIndex].position);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 leftBoundary = Quaternion.Euler(0, -visionAngle / 2f, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, visionAngle / 2f, 0) * transform.forward;

        Gizmos.DrawRay(transform.position, leftBoundary * visionRange);
        Gizmos.DrawRay(transform.position, rightBoundary * visionRange);
    }
}
