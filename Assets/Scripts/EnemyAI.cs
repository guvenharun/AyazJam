using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotationSpeed = 10f;
    public float detectionRange = 15f;
    public float detectionAngle = 90f;
    public float raycastInterval = 0.2f;
    private float nextRaycastTime = 0f;

    public int rayCount = 10;

    public bool isPlayerInZone = false;

    public Transform player;

    private Vector3 lastKnownPosition;
    private bool hasLastKnownPosition = false;

    private Rigidbody rb;

    private float currentRotationAngle = 0f;
    private bool rotatingForward = true;

    public float maxRotationAngle = -90f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    void Update()
    {
        if (Time.time >= nextRaycastTime)
        {
            nextRaycastTime = Time.time + raycastInterval;
            PerformRaycast();
        }

        if (!isPlayerInZone && hasLastKnownPosition)
        {
            MoveTowards(lastKnownPosition);

            if (Vector3.Distance(transform.position, lastKnownPosition) < 0.5f)
            {
                hasLastKnownPosition = false;
                StopMovement();
            }
        }
        else if (!isPlayerInZone)
        {
            PatrolRotation();
        }
    }

    private void PatrolRotation()
    {
      
        if (rotatingForward)
        {
            currentRotationAngle += rotationSpeed * Time.deltaTime;
            if (currentRotationAngle >= maxRotationAngle)
            {
                currentRotationAngle = maxRotationAngle;
                rotatingForward = false;
            }
        }
        else
        {
            currentRotationAngle -= rotationSpeed * Time.deltaTime;
            if (currentRotationAngle <= 0f)
            {
                currentRotationAngle = 0f;
                rotatingForward = true;
            }
        }

        Quaternion targetRotation = Quaternion.Euler(0, currentRotationAngle, 0);
        transform.rotation = targetRotation;
    }

    private void PerformRaycast()
    {
        bool playerDetected = false;

        for (int i = 0; i < rayCount; i++)
        {
            float angle = -detectionAngle / 2 + (detectionAngle / Mathf.Max(1, rayCount - 1)) * i;
            Vector3 rayDirection = Quaternion.Euler(0, angle, 0) * transform.forward;

            if (Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, detectionRange))
            {
                if (hit.transform == player)
                {
                    playerDetected = true;
                    lastKnownPosition = player.position;
                    hasLastKnownPosition = true;
                    MoveTowards(player.position);
                    break;
                }
            }
        }

        if (!playerDetected && isPlayerInZone)
        {
            isPlayerInZone = false; 
        }
    }

    public void MoveTowards(Vector3 targetPosition)
    {
        if (rb == null) return;

        Vector3 direction = (targetPosition - transform.position).normalized;
        Vector3 velocity = direction * moveSpeed;

        velocity.y = rb.velocity.y;

        rb.velocity = velocity;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    public void StopMovement()
    {
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            player = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isPlayerInZone ? Color.red : Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        for (int i = 0; i < rayCount; i++)
        {
            float angle = -detectionAngle / 2 + (detectionAngle / Mathf.Max(1, rayCount - 1)) * i;
            Vector3 rayDirection = Quaternion.Euler(0, angle, 0) * transform.forward;
            Gizmos.DrawRay(transform.position, rayDirection * detectionRange);
        }

        if (hasLastKnownPosition)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(lastKnownPosition, 0.5f);
        }
    }
}
