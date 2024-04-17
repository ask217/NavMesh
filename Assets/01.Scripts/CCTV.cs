using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class CCTV : MonoBehaviour
{
    enum CCTVState
    {
        Idle,
        discover,
    }

    public Animator cameraAnim;
    private MeshRenderer camRenderer;

    CCTVState state;

    [SerializeField]
    private Transform target;    // 플레이어

    public float radius = 3f; // 탐색 범위 반지름

    Transform playerPos;

    void Start()
    {
        camRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (target == null)
        {
            target = GameObject.FindWithTag("Player").transform;
        }

        switch (state)
        {
            case CCTVState.Idle:
                Idle();
                break;
            case CCTVState.discover:
                DisCover();
                break;
        }
    }

    void Idle()
    {
        if (!cameraAnim.enabled)
        {
            cameraAnim.enabled = true;
            Color green = new Color(0.1f, 0.6f, 0.1f, 0.3f);
            camRenderer.materials[0].color = green;
        }
    }

    void DisCover()
    {
        if (cameraAnim.enabled)
        {
            cameraAnim.enabled = false;
            Color red = new Color(0.6f, 0.1f, 0.1f, 0.3f);
            camRenderer.materials[0].color = red;
        }

        //TODO: Play Sound and start function

        Collider[] guards = Physics.OverlapSphere(playerPos.position, 10.0f);

        for (int i = 0; i < guards.Length; i++)
        {
            guards[i].GetComponent<Guard>().CCTVDetection(playerPos);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == target.tag)
        {
            playerPos = other.transform;

            StopCoroutine(Unbinding());

            state = CCTVState.discover;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == target.tag)
        {
            StartCoroutine(Unbinding());
        }
    }

    IEnumerator Unbinding()
    {
        yield return new WaitForSeconds(3f);

        state = CCTVState.Idle;
    }
}
