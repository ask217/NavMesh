using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
    enum GuardState
    {
        Idle,
        Alert,
        combat,
    }

    NavMeshAgent agent;

    GuardState state;

    //부채꼴 범위 탐색 관련 변수
    [SerializeField]
    private Transform target;    // 플레이어
    [SerializeField]
    private float angleRange = 60f; // 부채꼴형 탐색 범위 각도
    public float radius = 3f; // 탐색 범위 거리

    bool isCollision = false;

    // 타겟팅 관련 변수
    bool isTargeting = false;

    //이동 관련 변수
    private int curNode = 0;
    public List<Transform> wayPoint = new List<Transform>();


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if(target == null)
        {
            target = GameObject.FindWithTag("Player").transform;
        }

        switch(state)
        {
            case GuardState.Idle:
                Move();
                break;
            case GuardState.Alert:
                Alert();
                break;
            case GuardState.combat:
                Targeting();
                break;
        }

        Vector3 distance = target.position - transform.position;

        if (distance.magnitude <= radius)
        {
            //사이 각 구하기
            float dot = Vector3.Dot(distance.normalized, transform.forward);
            float theta = Mathf.Acos(dot);
            // angleRange와 비교하기 위해 degree로 변환
            float degree = Mathf.Rad2Deg * theta;

            // 시야각 판별
            if (degree <= angleRange / 2f)
            {
                isCollision = true;
            }
            else
            {
                isCollision = false;
            }

        }
        else
        {
            isCollision = false;
        }

        #region 디버깅
        
        print("isCollision: " + isCollision);
        print("Guard State: " + state);

        Debug.DrawRay(transform.position, target.position - transform.position, Color.blue);

        #endregion
    }

    void Move()
    {
        if(isCollision)
        {
            RaycastHit hitInfo;

            if(Physics.Raycast(transform.position, target.position - transform.position, out hitInfo))
            {                
                print("HitInfo: " + hitInfo.transform.name);
                state = GuardState.combat;
            }
        }

        if(!agent.pathPending && agent.remainingDistance < 2f)
        {
            MoveToNext();
        }
        
        if(curNode == wayPoint.Count)
        {
            curNode = 0;
        }
    }

    void Alert()
    {
        StopAllCoroutines();
        StartCoroutine(ModeChanger());
        angleRange = 90f;
        radius = 5f;
    }

    void Targeting()
    {
        if(!isCollision)
        {
            StartCoroutine(ModeChanger());
        }
        else
        {
            StopAllCoroutines();
        }

        agent.destination = target.position;
    }

    private void MoveToNext()
    {
        agent.destination = wayPoint[curNode].position;

        curNode++;
    }

    IEnumerator ModeChanger()
    {
        switch(state)
        {
            case GuardState.Alert:
                yield return new WaitForSeconds(5f);
                break;

            case GuardState.combat:
                yield return new WaitForSeconds(2f);
                break;
        }

        state = GuardState.Idle;
    }

    private void OnDrawGizmos()
    {
        // 탐색 범위 기즈모
        Handles.color = new Color(1f, 0f, 0f, 0.2f);
        // DrawSolidArc(시작점, 노멀벡터(법선벡터), 그려줄 방향 벡터, 각도, 반지름)
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, angleRange / 2, radius);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -angleRange / 2, radius);

        //웨이포인트 기즈모
        for (int i = 0; i < wayPoint.Count; i++)
        {
            Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 0.3f);
            Gizmos.DrawSphere(wayPoint[i].transform.position, 2);
            Gizmos.DrawWireSphere(wayPoint[i].transform.position, 20f);

            if (i < wayPoint.Count - 1)
            {
                if (wayPoint[i] && wayPoint[i + 1])
                {
                    Gizmos.color = Color.red;
                    if (i < wayPoint.Count - 1)
                        Gizmos.DrawLine(wayPoint[i].position, wayPoint[i + 1].position);
                    if (i < wayPoint.Count - 2)
                    {
                        Gizmos.DrawLine(wayPoint[wayPoint.Count - 1].position, wayPoint[0].position);
                    }
                }
            }
        }
    }
}
