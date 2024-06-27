using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item Data")]
public class ItemData : ScriptableObject
{
    public int id;
    public string displayName;
    public string description;
    public int value;
    public bool stackable;
    public Sprite icon;

    public enum ItemType
    {
        Card,
        Gem,
        Key
    }

    public ItemType itemType;
}
