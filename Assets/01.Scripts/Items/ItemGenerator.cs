using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    Camera camera;

    public string prefix;
    public string pathFolder;

    public List<GameObject> sceneObjects;
    public List<ItemData> dataObjects;

    void Awake()
    {
        camera = GetComponent<Camera>();
    }

    [ContextMenu("Screenshot")]
    private void ProcessScreenshots()
    {
        StartCoroutine(Screenshot());
    }

    private IEnumerator Screenshot()
    {
        for(int i = 0; i < sceneObjects.Count; i++)
        {
            GameObject obj = sceneObjects[i];
            ItemData data = dataObjects[i];

            obj.SetActive(true);

            yield return null;

            TakeScreenShot(Path.Combine($"{Application.dataPath}",$"{pathFolder}",$"{data.id}_Icon.png"));

            yield return null;

            obj.SetActive(false);

            Sprite s = AssetDatabase.LoadAssetAtPath<Sprite>($"Arts/{pathFolder}/{data.id}_Icon.png");
            if(s != null)
            {
                data.icon = s;
                EditorUtility.SetDirty(data);
            }

            yield return null;
        }
    }

    void TakeScreenShot(String fullPath)
    {
        if(camera == null)
        {
            camera = GetComponent<Camera>();
        }

        RenderTexture rt = new RenderTexture(256, 256, 24);
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(256, 256, TextureFormat.RGBA32, false);
        camera.Render();
        camera.targetTexture = rt;

        StartCoroutine(Take(screenShot,rt, fullPath));
    }

    WaitForEndOfFrame end = new WaitForEndOfFrame();

    public IEnumerator Take(Texture2D screenShot, RenderTexture rt, string fullPath){
        yield return end;

        screenShot.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
        camera.targetTexture = null;
        camera.targetTexture = null;

        if(Application.isEditor)
        {
            DestroyImmediate(rt);
        }
        else
        {
            Destroy(rt);
        }

        byte[] bytes = screenShot.EncodeToPNG();
        System.IO.File.WriteAllBytes(fullPath, bytes);
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }
}
