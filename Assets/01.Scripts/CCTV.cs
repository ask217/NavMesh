using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CCTV : MonoBehaviour
{
    enum CCTVState
    {
        Idle,
        Alert,
        discover,
    }

    public Animator cameraAnim;
    private MeshRenderer camRenderer;

    CCTVState state;

    [SerializeField]
    private Transform target;    // 플레이어

    public float radius = 3f; // 탐색 범위 반지름

    void Start()
    {
        camRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if(target == null)
        {
            target = GameObject.FindWithTag("Player").transform;
        }

        switch(state)
        {
            case CCTVState.Idle:
                //TODO: 회전
                break;
            case CCTVState.Alert:
                //TODO: 잠시 멈추고 확인
                break;
            case CCTVState.discover:
                //TODO: 경보 울리기
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == target.tag)
        {
            //TODO: state 설정
            cameraAnim.enabled = false;
            Color red = new Color(0.6f, 0.1f, 0.1f, 0.3f);
            camRenderer.materials[0].color = red;
        }
    }
}
