using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    Camera cam;

    Ray ray;
    RaycastHit hitInfo;
    public float rayDistance;
    public float rayRadius;
    public LayerMask itemLayer;
    public LayerMask DoorLayer;

    public GameObject UIText;

    Vector3 ScreenCenter;

    bool isHit;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cam = Camera.main;
        ScreenCenter = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2);

        UIText.SetActive(false);
    }

    void Update()
    {
        ray = cam.ScreenPointToRay(ScreenCenter);
        Debug.DrawRay(transform.position, transform.forward, Color.red);
        if (Physics.SphereCast(ray, rayRadius, out hitInfo, rayDistance) && hitInfo.transform.tag != "Environment")
        {
            isHit = true;
        }
        else
        {
            isHit = false;
        }

        if(isHit)
        {
            UIText.SetActive(true);
        }
        else
        {
            UIText.SetActive(false);
        }

        if (isHit && Input.GetKeyDown(KeyCode.F))
        {
            if (hitInfo.transform.tag == "Item")
            {
                DataManager.instance.AddItem(hitInfo.collider.GetComponent<ItemController>().itemData);
                Destroy(hitInfo.transform.gameObject);
            }
            
            if(hitInfo.transform.tag == "Door")
            {
                hitInfo.transform.GetComponent<Door>().CheckKey();
            }
        }
    }

    void OnDrawGizmos()
    {
        if (isHit)
        {
            Gizmos.DrawRay(transform.position, transform.forward * hitInfo.distance);
            Gizmos.DrawWireSphere(transform.position + transform.forward * hitInfo.distance, rayRadius);
        }
        else
        {
            Gizmos.DrawRay(transform.position, transform.forward * rayDistance);
        }
    }
}
