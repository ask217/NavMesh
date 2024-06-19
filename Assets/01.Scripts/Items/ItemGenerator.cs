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

    [ContextMenu("Screenshot-One")]
    private void ProcessScreenshot()
    {
        StartCoroutine(ScreenshotOne());
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

    private IEnumerator ScreenshotOne()
    {
        GameObject obj = sceneObjects[0];
            ItemData data = dataObjects[0];

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

    void TakeScreenShot(string fullPath)
    {
        if(camera == null)
        {
            camera = GetComponent<Camera>();
        }

        RenderTexture old = camera.targetTexture;

        RenderTexture rt = new RenderTexture(256, 256, 32, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        camera.targetTexture = rt;

        camera.Render();

        camera.targetTexture = old;

        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D(256, 256,TextureFormat.ARGB32, false, true);
        screenShot.ReadPixels(new Rect(0,0,rt.width,rt.height),0,0);
        screenShot.Apply();
        RenderTexture.active = null;

        byte[] bytes = screenShot.EncodeToPNG();

        Destroy(screenShot);
        File.WriteAllBytes(fullPath, bytes);
        // AssetDatabase.ImportAsset(fullPath);
    }

    WaitForEndOfFrame end = new WaitForEndOfFrame();

//     public IEnumerator Take(Texture2D screenShot, RenderTexture rt, string fullPath)
//     {
//         yield return end;

//         RenderTexture.active = rt;

//         screenShot.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);

//         screenShot.Apply();

//         // if(Application.isEditor)
//         // {
//         //     DestroyImmediate(rt);
//         // }
//         // else
//         // {
//         //     Destroy(rt);
//         // }

//         byte[] bytes = ImageConversion.EncodeToPNG(screenShot);
//         File.WriteAllBytes(fullPath, bytes);

// #if UNITY_EDITOR
//         AssetDatabase.Refresh();
// #endif
//     }
}
