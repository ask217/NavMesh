using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FirstPersonPlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float movePower;

    public float groundDrag;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask GroundMask;
    bool grounded;

    public Transform Orientation;

    float inputH;
    float inputV;

    Vector3 direction;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
    void Update()
    {
        //Gound Check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, GroundMask);

        PlayerInput();
        SpeedControl();

        //Gound Drag
        if(grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0f;
        }
    }

    void FixedUpdate()
    {
        PlayerMove();
    }

    private void PlayerInput()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");
    }

    private void PlayerMove()
    {
        //calculate movement direction
        direction = Orientation.forward * inputV + Orientation.right * inputH;

        rb.AddForce(direction.normalized * moveSpeed * movePower, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;

            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}
