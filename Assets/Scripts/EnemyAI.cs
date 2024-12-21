using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        Idle,
        Chasing,
        Returning,
        BackToWhomb
    }

    public State currentState = State.Idle;

    public float moveSpeed = 3f;
    public float rotationSpeed = 10f;
    public float detectionRange = 15f;
    public float detectionAngle = 90f;
    public float raycastInterval = 0.2f;
    private float nextRaycastTime = 0f;
    private float timePassed = 0f;

    public int rayCount = 10;

    public Transform player;

    private Vector3 lastKnownPosition;
    private Quaternion whombRotation;
    private Vector3 whombaPosition;
    private bool hasLastKnownPosition = false;

    private Rigidbody rb;

    private float idleRotationAngle = 0f;

    void Start()
    {
        whombaPosition = transform.position;
        whombRotation = transform.rotation; 
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component missing!");
        }
    }

    void Update()
    {
        if (Time.time >= nextRaycastTime)
        {
            nextRaycastTime = Time.time + raycastInterval;
            PerformRaycast();
        }

        switch (currentState)
        {
            case State.Idle:
                PerformIdle();
                break;
            case State.Chasing:
                PerformChasing();
                break;
            case State.Returning:
                PerformReturning();
                break;
            case State.BackToWhomb:
                PerformBack();
                break;
        }
    }
    private void PerformBack()
    {
        MoveTowards(whombaPosition);
        if (Vector3.Distance(transform.position , whombaPosition) < 2.5f)
        {
            transform.rotation = whombRotation;
            StopMovement();
            currentState = State.Idle;
        }

    }

    private void PerformIdle()
    {

        idleRotationAngle = Mathf.PingPong(Time.time * rotationSpeed, 90f); 


        transform.rotation = Quaternion.Euler(0, idleRotationAngle, 0);


        if (!hasLastKnownPosition)
        {
            return; 
        }


        currentState = State.Chasing;
    }

    private void PerformChasing()
    {
        if (player != null)
        {
            MoveTowards(player.position);
        }

        if (!hasLastKnownPosition)
        {
            currentState = State.Idle;

        }
    }

    private void PerformReturning()
    {
        if (hasLastKnownPosition)
        {
            MoveTowards(lastKnownPosition);


            if (Vector3.Distance(transform.position, lastKnownPosition) < 2.5f)
            {
                
                StopMovement();
                timePassed += Time.deltaTime;

                if(timePassed >= 3.25f)
                {
                    timePassed = 0f;
                    hasLastKnownPosition = false;
                    currentState = State.BackToWhomb;
                }
            }
        }

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
                    currentState = State.Chasing;
                    break;
                }
            }
        }

        if (!playerDetected && currentState == State.Chasing)
        {
            currentState = State.Returning;
        }
    }

    public void MoveTowards(Vector3 targetPosition)
    {
        if (rb == null) return;

        Vector3 direction = (targetPosition - transform.position).normalized;
        Vector3 velocity = direction * moveSpeed;

        velocity.y = rb.velocity.y; 

        rb.velocity = velocity;

        direction.y = 0; 
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
            player = other.transform;
            currentState = State.Chasing;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentState = State.Returning;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = currentState == State.Chasing ? Color.red : Color.blue;
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
