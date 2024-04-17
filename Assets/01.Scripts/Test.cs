using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    int NavigationStep = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Navigation());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Navigation()
    {
        StartCoroutine(Rotate(0.5f, -60));
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => NavigationStep > 0);
        StartCoroutine(Rotate(1f, 120));
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => NavigationStep > 1);
        StartCoroutine(Rotate(0.5f, -60));
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => NavigationStep > 2);
        print("end");
    }

    IEnumerator Rotate( float duration, float yAngle )
    {
        Vector3 startForward = transform.forward;
        Vector3 targetForward = Quaternion.Euler(0f, yAngle, 0f ) * startForward;
        float ratio = 0f;
        float vel = 1f / duration;
        while( ratio < 1f )
        {
            transform.forward = Vector3.Slerp( startForward, targetForward, ratio ); //필요시 AnimationCurve나 EasingFunction검토.
            yield return null;
            ratio += Time.deltaTime * vel;
        }
        transform.forward = targetForward;

        if(NavigationStep >= 2)
        {
            NavigationStep = -1;
        }

        NavigationStep++;
    }
}
