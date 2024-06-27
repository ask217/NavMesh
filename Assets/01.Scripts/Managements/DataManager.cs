using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using Unity.VisualScripting;
using Microsoft.Unity.VisualStudio.Editor;

public class DataManager : MonoBehaviour
{
    #region singleton
    public static DataManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public SaveData saveData = new SaveData();

    public GameObject ItemUI;
    public RectTransform contant;

    TextMeshProUGUI[] resultText = new TextMeshProUGUI[2];

    string path;

    public void Save()
    {
        string data = JsonUtility.ToJson(saveData);
        File.WriteAllText(path, data);
    }

    public void Load()
    {
        if (File.Exists(path))
        {
            string data = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(data);
        }
        else
        {
            Save();
        }
    }

    public void AddItem(ItemData data)
    {
        saveData.items.Add(data);
        AddItemInInventory(data);
        Save();
    }

    public void AddItemInInventory(ItemData data)
    {
        GameObject item = Instantiate(ItemUI);
        item.GetComponentInChildren<UnityEngine.UI.Image>().sprite = data.icon;
        item.GetComponentInChildren<TextMeshProUGUI>().text = data.name;
        item.transform.SetParent(contant,false);
    }

    public bool CheckItem(ItemData data)
    {
        for(int i = 0; i < saveData.items.Count; i++)
        {
            if(saveData.items[i] == data)
            {
                return true;
            }
        }

        return false;
    }

    void Start()
    {
        path = Path.Combine(Application.persistentDataPath, "save.json");
        for(int i = 0; i < saveData.items.Count; i++)
        {
            AddItemInInventory(saveData.items[i]);
        }
    }
}

[System.Serializable]
public class SaveData
{
    public Transform playerPos;
    public List<ItemData> items;
}
