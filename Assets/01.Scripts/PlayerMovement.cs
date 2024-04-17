using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    public Transform cam;

    [SerializeField]
    private float _speed = 6f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    float gravity;

    bool isDie = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        isDie = false;
    }

    void Update()
    {
        if (!GameManager.instance.isGameOver)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            Vector3 dir = new Vector3(h, 0f, v).normalized;

            gravity -= 9.81f * Time.deltaTime;

            if (controller.isGrounded) gravity = 0;

            if (dir.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                moveDir = new Vector3(moveDir.x, gravity, moveDir.z);

                controller.Move(moveDir * _speed * Time.deltaTime);
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag == "Guard")
        {
            GameManager.instance.GameOver();
        }
    }
}
