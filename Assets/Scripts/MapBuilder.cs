using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.IO;

public class MapBuilder : MonoBehaviour 
{
    private Material material;
    private Renderer renderer;
    private Texture2D hotspotPic;
    private string hotspotName = "Textures/arrow";
    private string shaderStyle = "Insideout";
    private Sprite hotspot;
    private SpriteRenderer sr;
    private List<GameObject> sp = new List<GameObject>();
    SphereChanger sphereChanger;
    private int fromPointID;
    private int toPointID;
    private float sphereScale = 3;
    private Dictionary<(int,int),float> azimuts = new Dictionary<(int, int), float>();
    private GameObject tripod;


    private void Start()
    {
        tripod = GameObject.Find("Tripod");
    }
    public void BuildMap(Parser.Points points)
    {
        sphereChanger = GameObject.Find("SphereChanger").GetComponent<SphereChanger>();
        hotspotPic = Resources.Load<Texture2D>(hotspotName);
        for (int i = 0; i < points.points.Length; i++)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = new Vector3((sphereScale + 0.5f) * i, 0, 0);
            sphere.name += points.points[i].id;
            sphere.transform.localScale = new Vector3(sphereScale, sphereScale, sphereScale);
            sphere.gameObject.GetComponent<SphereCollider>().enabled = false;
            renderer = sphere.GetComponent<Renderer>();

            material = new Material(Shader.Find(shaderStyle));
            sphere.GetComponent<Renderer>().material = material;
            var picture = Resources.Load<Texture2D>(points.points[i].Picture);
            //var picture = (Texture2D) LoadPNG(points.points[i].Picture);
            renderer.material.mainTexture = picture;
            sp.Add(sphere);
        }
        for (int i = 0; i < points.points.Length; i++)
        {
            for (int j = 0; j < points.points[i].Neighbors.Length; j++)
            {
                fromPointID = points.points[i].id;
                toPointID = points.points[i].Neighbors[j].PointID;
                GameObject go = new GameObject("hotspot" + toPointID);
                go.transform.parent = sp[i].transform;
                sr = go.AddComponent<SpriteRenderer>() as SpriteRenderer;
                float wantedAzimuthDeg = points.points[i].Neighbors[j].Azimut;
                azimuts.Add((points.points[i].id, points.points[i].Neighbors[j].PointID), points.points[i].Neighbors[j].Azimut);
                float wantedAzimuthRad = wantedAzimuthDeg * Mathf.PI / 180;
                float x = 0.4f * Mathf.Cos(-wantedAzimuthRad);
                float y = 0;
                float z = 0.4f * Mathf.Sin(-wantedAzimuthRad);
                go.transform.localRotation = Quaternion.Euler(0, wantedAzimuthDeg + 90, 0);
                sr.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
                var sprite = Sprite.Create(hotspotPic, new Rect(0, 0, hotspotPic.width, hotspotPic.height), new Vector3(0.1f, 0.1f));
                go.transform.localPosition = new Vector3(x, y, z);
                sr.sprite = sprite;
                SphereCollider sc = go.AddComponent(typeof(SphereCollider)) as SphereCollider;
                EventTrigger trigger = go.AddComponent<EventTrigger>();
                EventTrigger.Entry click = new EventTrigger.Entry();
                EventTrigger.Entry enter = new EventTrigger.Entry();
                EventTrigger.Entry exit = new EventTrigger.Entry();
                click.eventID = EventTriggerType.PointerDown;
                enter.eventID = EventTriggerType.PointerEnter;
                exit.eventID = EventTriggerType.PointerExit;
                exit.callback.AddListener((data) => { OnPointerExitDelegate((PointerEventData)data); });
                enter.callback.AddListener((data) => { OnPointerEnterDelegate((PointerEventData)data); });
                click.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
                trigger.triggers.Add(click);
                trigger.triggers.Add(enter);
                trigger.triggers.Add(exit);
            }
        }
        //Debug.LogError(sp[0].name);
        GameObject wantedSphere = sp[0];
        //foreach (KeyValuePair<(int, int), float> pair in azimuts)
        //    Debug.Log((pair.Key, pair.Value));

        sphereChanger.ChangeSphere(wantedSphere.transform, 180);
    }

    private void OnPointerDownDelegate(PointerEventData data)
    {
        Debug.Log(data);
        float azimuth = 0;
        String requestedID = data.pointerPressRaycast.gameObject.name.Substring(7);
        GameObject wantedSphere = GameObject.Find("Sphere" + requestedID);
        try
        {
            string fromPoint = data.lastPress.name.Substring(7);
            //Debug.LogError("Last press is : " + data.lastPress + "length is : " + data.lastPress.name.Length);
            //Debug.LogError(fromPoint);
            azimuth = azimuts[(Int32.Parse(data.pointerPressRaycast.gameObject.name.Substring(7)), Int32.Parse(data.lastPress.name.Substring(7)))];
            //Debug.Log("the angle from : " + data.pointerPressRaycast.gameObject.name.Substring(7) + " to : " + data.lastPress.name.Substring(7) + " is : " + azimuth);
        }
        catch (Exception e) { Debug.LogError(e.StackTrace); }
        sphereChanger.ChangeSphere(wantedSphere.transform,azimuth);
    }
    private void OnPointerEnterDelegate(PointerEventData data)
    {
        var script = tripod.GetComponent<VrGaze>();
        float azimuth = 0;
        String requestedID = data.pointerCurrentRaycast.gameObject.name.Substring(7);
        //Debug.Log(data.pointerCurrentRaycast.gameObject.name.Substring(7));
        string fromPoint = script.last;
        if (fromPoint.Equals("")) { fromPoint = "1"; azimuth = 0; }
        else
        {
            azimuth = azimuts[(Int32.Parse(requestedID), Int32.Parse(fromPoint))];
            //Debug.Log("the angle from : " + requestedID + " to : " + fromPoint + " is : " + azimuth);
        }
        //GameObject wantedHotspot = GameObject.Find("Sphere" + requestedID).transform.Find("hotspot" + fromPoint).gameObject;
        //Debug.LogError(data);
        script.GVRon(requestedID, azimuth);
    }
    private void OnPointerExitDelegate(PointerEventData data)
    {
        var script = tripod.GetComponent<VrGaze>();
        script.GVROff();
    }
    private Texture2D LoadPNG(string filePath)
    {
        WWW www = new WWW("file:///" + filePath);
        IEnumerator Start()
        {
            while (!www.isDone)
            {
                Debug.LogError("Illegal Path!!");
                yield return null;
            }
        }
        return FlipTextureVertically(www.texture);
    }
    private Texture2D FlipTextureVertically(Texture2D original)
    {
        var originalPixels = original.GetPixels();

        Color[] newPixels = new Color[originalPixels.Length];

        int width = original.width;
        int rows = original.height;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                newPixels[x + y * width] = originalPixels[x + (rows - y - 1) * width];
            }
        }

        original.SetPixels(newPixels);
        original.Apply();
        return original;
    }
}