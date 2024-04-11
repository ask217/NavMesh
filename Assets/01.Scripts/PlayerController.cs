using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;

    private CharacterController controller;
    private Vector3 dir;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");  

        Vector3 move = transform.TransformDirection(new Vector3(h, 0, v));
        dir = move * _speed; 

        
        controller.Move(dir * Time.deltaTime);
    }
}
