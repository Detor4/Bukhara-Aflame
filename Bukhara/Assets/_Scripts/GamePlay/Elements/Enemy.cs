using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    [SerializeField] DangerSms dangerSms; // 👈 DangerSms skriptiga murojaat

    private Vector3 lastSeenPosition;
    private bool playerVisible = false;
    private bool searching = false;

    public float visionAngle = 40f;
    public float visionRange = 10f;
    public float rotationSpeed = 120f;
    public float wanderRadius = 15f;
    public float newDestinationInterval = 5f;

    private float timer = 0f;
    private float rotateAmount = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        SetNewRandomDestination();
    }

    void Update()
    {
        if (player == null) return;

        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        float angle = Vector3.Angle(transform.forward, directionToPlayer.normalized);

        // Player ko‘rinishida
        if (distanceToPlayer <= visionRange && angle < visionAngle / 2f && CanSeePlayer())
        {
            if (!playerVisible)
            {
                agent.SetDestination(player.position);

                // 👇 Faqat bir marta ishga tushadi — ko‘rgan zahoti
                dangerSms?.StartWarning();
            }

            playerVisible = true;
            lastSeenPosition = player.position;
            searching = false;
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
            timer += Time.deltaTime;
            if (timer >= newDestinationInterval || agent.remainingDistance < 1f)
            {
                SetNewRandomDestination();
                timer = 0f;
            }
        }

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

        animator.SetBool("Walk", false);

        if (rotateAmount >= 360f)
        {
            rotateAmount = 0f;
            searching = false;
            SetNewRandomDestination();
        }
    }

    void SetNewRandomDestination()
    {
        Vector3 randomPos = RandomNavMeshLocation(transform.position, wanderRadius);
        agent.SetDestination(randomPos);
    }

    Vector3 RandomNavMeshLocation(Vector3 origin, float radius)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += origin;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return origin;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 left = Quaternion.Euler(0, -visionAngle / 2f, 0) * transform.forward;
        Vector3 right = Quaternion.Euler(0, visionAngle / 2f, 0) * transform.forward;

        Gizmos.DrawRay(transform.position, left * visionRange);
        Gizmos.DrawRay(transform.position, right * visionRange);
    }
}
