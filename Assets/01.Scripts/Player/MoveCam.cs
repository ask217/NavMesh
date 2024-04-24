using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    public Transform camPosition;

    void Update()
    {
        transform.position = camPosition.position;
    }
}
