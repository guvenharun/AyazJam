using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//dfdf
public class GBasicMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float slowSpeed;
    [SerializeField] private float acceleration;
    
    private float currentSpeed;

    public float moveX;
    public float moveZ;

    public Vector3 movement;


    Rigidbody rb;

    void Start()
    {

        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        movement = new Vector3(moveX, 0f, moveZ);

        if (Input.GetKey(KeyCode.LeftControl))
        {
            currentSpeed = slowSpeed;

        }
        else
        {
            currentSpeed = movementSpeed;

   
        }
    }

    private void FixedUpdate()
    {

        rb.MovePosition(rb.position + movement * currentSpeed* Time.fixedDeltaTime);
    }


}
