using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item Data")]
public class ItemData : ScriptableObject
{
    public string id;
    public string displayName;
    public string description;
    public Sprite icon;
    public GameObject prefab;
}
