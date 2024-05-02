using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class Bullet : MonoBehaviour
{
    public bool isCanMove = true;

    Transform firePos;
    Vector3 dir;
    Ray ray;
    RaycastHit hitInfo;

    void Start()
    {
        firePos = GameObject.Find("FirePos").transform;
        dir = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
        ray = new Ray(firePos.position, dir);
    }

    void Update()
    {
        if(Physics.Raycast(ray, out hitInfo))
        {
            if(hitInfo.transform.tag == "Guard")
            {
                hitInfo.transform.GetComponent<Guard>().Damaged();
            }
        }
        if (isCanMove)
        {
            transform.position += ray.direction * 50 * Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        isCanMove = false;
        Destroy(gameObject, 3f);
    }
}
