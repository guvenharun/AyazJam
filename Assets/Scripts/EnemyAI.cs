using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;         
    public float detectionRange = 10f;   
    public int rayCount = 5;             
    public Transform player;             

    private bool isPlayerInZone = false; 

    void Update()
    {
        if (isPlayerInZone)
        {

            for (int i = 0; i < rayCount; i++)
            {
                float angle = -45 + (90 / (rayCount - 1)) * i; 
                Vector3 rayDirection = Quaternion.Euler(0, angle, 0) * transform.forward;

                if (Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, detectionRange))
                {
                    if (hit.transform == player)
                    {
                        MoveTowardsPlayer();
                        return;
                    }
                }
            }
        }
    }

    private void MoveTowardsPlayer()
    {

        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
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
        if (isPlayerInZone)
        {
            for (int i = 0; i < rayCount; i++)
            {
                float angle = -45 + (90 / (rayCount - 1)) * i;
                Vector3 rayDirection = Quaternion.Euler(0, angle, 0) * transform.forward;
                Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position, rayDirection * detectionRange);
            }
        }
    }
}
