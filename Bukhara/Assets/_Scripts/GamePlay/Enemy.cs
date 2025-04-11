using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    private Vector3 startPosition;
    private Vector3 lastSeenPosition;
    private bool playerVisible = false;
    private bool searching = false;

    public float visionAngle = 40f;
    public float visionRange = 10f;
    public float rotationSpeed = 120f; // gradus/sek
    public Transform[] points; // Nuqtalar arraysi

    private float rotateAmount = 0f;
    private int currentPointIndex = 0; // Joriy nuqta indeksi

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        agent = GetComponent<NavMeshAgent>();
        startPosition = transform.position;

        // Agar points arrayi bo'sh bo'lmasa, harakatni boshlaymiz
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
            // Playerni ko'rdik, uning orqasidan ketamiz
            playerVisible = true;
            lastSeenPosition = player.position;
            searching = false;
            agent.SetDestination(player.position);
        }
        else if (playerVisible)
        {
            // Playerni yo'qotdik, qidiruvni boshlaymiz
            playerVisible = false;
            searching = true;
            agent.SetDestination(lastSeenPosition);
        }
        else if (searching)
        {
            float distance = Vector3.Distance(transform.position, lastSeenPosition);
            if (distance < 0.5f)
            {
                RotateInPlace(); // 360 gradus aylanish
            }
        }
        else
        {
            // Player yo'q, qidiruv ham tugagan, patrulni davom ettiramiz
            float distanceToCurrentPoint = Vector3.Distance(transform.position, points[currentPointIndex].position);
            if (distanceToCurrentPoint < 1f)
            {
                currentPointIndex = (currentPointIndex + 1) % points.Length;
                agent.SetDestination(points[currentPointIndex].position);
            }
        }
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

        if (rotateAmount >= 360f)
        {
            rotateAmount = 0f;
            searching = false;

            // Patrulni davom ettiramiz
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
