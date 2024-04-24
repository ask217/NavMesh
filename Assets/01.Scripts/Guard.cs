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
    Animator guardAnim;

    GuardState state;

    //부채꼴 범위 탐색 관련 변수
    [SerializeField]
    private Transform target;    // 플레이어
    [SerializeField]
    private float angleRange = 60f; // 부채꼴형 탐색 범위 각도
    public float radius = 3f; // 탐색 범위 거리

    bool isCollision = false;

    // 타겟팅 관련 변수
    RaycastHit hitInfo;

    //이동 관련 변수
    private int curNode = 0;
    public List<Transform> wayPoint = new List<Transform>();

    bool playerDetection;

    int NavigationStep = 0;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        guardAnim = GetComponent<Animator>();

        target = GameObject.FindWithTag("Player").transform;

        playerDetection = false;
        NavigationStep = 0;
    }

    void Update()
    {
        #region 상태머신
        if (target == null)
        {
            target = GameObject.FindWithTag("Player").transform;
        }

        switch (state)
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
        #endregion

        #region  탐색
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
        #endregion

        if ((!agent.pathPending && agent.remainingDistance < 3f) && playerDetection)
        {
            StartCoroutine(Navigation());
        }

        #region 디버깅

        // print("isCollision: " + isCollision);
        // print("Guard State: " + state);

        if (hitInfo.transform != null)
        {
            // print("HitInfo: " + hitInfo.transform.name);
        }

        #endregion
    }

    void Move()
    {
        if (!playerDetection)
        {
            angleRange = 75f;
            radius = 5f;

            if (isCollision)
            {
                RaycastHit hitInfo;

                if (Physics.Raycast(transform.position, target.position - transform.position, out hitInfo) && !GameManager.instance.isGameOver)
                {
                    print("HitInfo: " + hitInfo.transform.name);
                    if (hitInfo.transform.tag == target.tag)
                    {
                        state = GuardState.combat;
                    }
                }
            }

            if (!agent.pathPending && agent.remainingDistance < 2f)
            {
                MoveToNext();
            }

            if (curNode == wayPoint.Count)
            {
                curNode = 0;
            }
        }
    }

    void Alert()
    {
        angleRange = 120f;
        radius = 7f;

        if (!playerDetection)
        {
            StopCoroutine(ModeChanger(0f));
            StartCoroutine(ModeChanger(5f));

            Move();
        }
    }

    void Targeting()
    {
        if (!isCollision)
        {
            StartCoroutine(ModeChanger(3f));
        }
        else
        {
            if (agent.isStopped == true)
            {
                agent.isStopped = false;
            }

            StopAllCoroutines();

            if (Physics.Raycast(transform.position, target.position - transform.position, out hitInfo) && !GameManager.instance.isGameOver)
            {
                Debug.DrawRay(transform.position, target.position - transform.position, Color.blue);

                print("HitInfo: " + hitInfo.transform.name);

                if (hitInfo.transform.tag == target.tag)
                {
                    agent.destination = hitInfo.transform.position;
                }
            }
        }
    }

    public void CCTVDetection(Vector3 playerPos)
    {
        playerDetection = true;

        state = GuardState.Alert;

        agent.destination = playerPos;
    }

    private void MoveToNext()
    {
        if (curNode >= wayPoint.Count)
        {
            curNode = 0;
        }

        agent.destination = wayPoint[curNode].position;

        curNode++;
    }

    IEnumerator Navigation()
    {
        print("Navigation Start");
        agent.isStopped = true;
        StartCoroutine(Rotate(0.5f, -60));
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => NavigationStep > 0);
        StartCoroutine(Rotate(1f, 120));
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => NavigationStep > 1);
        agent.isStopped = false;
        NavigationStep = 0;
        MoveToNext();
        StartCoroutine(ModeChanger(15f));
    }

    IEnumerator Rotate(float duration, float yAngle)
    {
        Vector3 startForward = transform.forward;
        Vector3 targetForward = Quaternion.Euler(0f, yAngle, 0f) * startForward;
        float ratio = 0f;
        float vel = 1f / duration;
        while (ratio < 1f)
        {
            transform.forward = Vector3.Slerp(startForward, targetForward, ratio);
            yield return null;
            ratio += Time.deltaTime * vel;
        }
        transform.forward = targetForward;

        NavigationStep++;
    }

    IEnumerator ModeChanger(float WaitSeconds)
    {
        print(WaitSeconds);
        switch (state)
        {
            case GuardState.Alert:
                yield return new WaitForSeconds(WaitSeconds);
                state = GuardState.Idle;
                if (playerDetection)
                {
                    playerDetection = false;
                }
                break;

            case GuardState.combat:
                yield return new WaitForSeconds(WaitSeconds);
                state = GuardState.Alert;
                break;
        }
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
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
#endif
    }
}
