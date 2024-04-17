using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV : MonoBehaviour
{
    enum CCTVState
    {
        Idle,
        Alert,
        discover,
    }

    CCTVState state;

    [SerializeField]
    private Transform target;    // 플레이어

    public float radius = 3f; // 탐색 범위 반지름

    void Start()
    {
        
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
                //TODO: 이동/회전
                break;
            case CCTVState.Alert:
                //TODO: 잠시 멈추고 확인
                break;
            case CCTVState.discover:
                //TODO: 경보 울리기
                break;
        }
    }
}
