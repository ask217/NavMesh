using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public ItemData data;

    void Start()
    {
        gameObject.SetActive(true);
    }
    
    public void CheckKey()
    {
        if(DataManager.instance.CheckItem(data))
        {
            gameObject.SetActive(false);
        }
    }
}
