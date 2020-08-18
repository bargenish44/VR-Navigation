using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class MapBuilder : MonoBehaviour
{
    private Material material;
    private Renderer renderer;
    private Texture2D hotspotPic;
    private Texture2D FinalHotspotPic;
    private string hotspotName = "Textures/arrow";
    private string finalHotspotName = "Textures/FinalArrow";
    private string shaderStyle = "Insideout";
    private Sprite hotspot;
    private SpriteRenderer sr;
    private List<GameObject> sp = new List<GameObject>();
    private SphereChanger sphereChanger;
    private int fromPointID;
    private int toPointID;
    private float sphereScale = 3;
    private float sphereScaleY = 1.3f;
    private Dictionary<(int, int), float> azimuts = new Dictionary<(int, int), float>();
    private GameObject tripod;
    private string lastSphere;
    private Sprite sprite;
    private TextManager textsEditor;
    private SpheresContainer spheres;

    private void Start()
    {
        tripod = GameObject.Find("Tripod");
    }
    public void BuildMap(Parser.Points points)
    {
        try
        {
            textsEditor = GameObject.Find("TextEditor").GetComponent<TextManager>();
            sphereChanger = GameObject.Find("SphereChanger").GetComponent<SphereChanger>();
            if (points.NavigationImage.Equals(""))
                hotspotPic = Resources.Load<Texture2D>(hotspotName);
            else hotspotPic = LoadPNG(points.NavigationImage);
            if (points.FinalNavigationImage.Equals(""))
                FinalHotspotPic = Resources.Load<Texture2D>(finalHotspotName);
            else FinalHotspotPic = LoadPNG(points.FinalNavigationImage);
            // Build map Points
            spheres = GameObject.Find("Tripod").GetComponent<SpheresContainer>();
            for (int i = 0; i < points.points.Count; i++)
            {
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.localPosition = new Vector3(0, 0, 0);
                sphere.name += points.points[i].id;
                sphere.transform.localScale = new Vector3(sphereScale, sphereScaleY, sphereScale);
                sphere.gameObject.GetComponent<SphereCollider>().enabled = false;
                renderer = sphere.GetComponent<Renderer>();

                material = new Material(Shader.Find(shaderStyle));
                sphere.GetComponent<Renderer>().material = material;
                var picture = LoadPNG(points.points[i].Picture);
                renderer.material.mainTexture = picture;
                sp.Add(sphere);
                textsEditor.texts.Add(sphere.name, points.points[i].OptionalText);
            }
            lastSphere = points.points[points.points.Count - 1].id + "";
            // Build map's conections
            for (int i = 0; i < points.points.Count; i++)
            {
                for (int j = 0; j < points.points[i].Neighbors.Count; j++)
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
                    sprite = toPointID.ToString().Equals(lastSphere) ? HotspotPic(FinalHotspotPic) : HotspotPic(hotspotPic);
                    go.transform.localPosition = new Vector3(x, y, z);
                    sr.sprite = sprite;
                    SphereCollider sc = go.AddComponent(typeof(SphereCollider)) as SphereCollider;
                    EventTrigger trigger = go.AddComponent<EventTrigger>();
                    EventTrigger.Entry enter = new EventTrigger.Entry();
                    EventTrigger.Entry exit = new EventTrigger.Entry();
                    enter.eventID = EventTriggerType.PointerEnter;
                    exit.eventID = EventTriggerType.PointerExit;
                    exit.callback.AddListener((data) => { OnPointerExitDelegate((PointerEventData)data); });
                    enter.callback.AddListener((data) => { OnPointerEnterDelegate((PointerEventData)data); });
                    trigger.triggers.Add(enter);
                    trigger.triggers.Add(exit);
                }
                spheres.GetSpheres().Add(sp[i].name, sp[i]);
                if (i != 0) sp[i].SetActive(false); // viewing only the first point
            }
            GameObject wantedSphere = sp[0];
            sphereChanger.ChangeSphere(wantedSphere.transform, 0, lastSphere);
        }
        catch (NullReferenceException e) { SceneManager.LoadScene("InsertJson"); }
        catch (FileNotFoundException e1) { SceneManager.LoadScene("InsertJson"); }
    }

    private void OnPointerEnterDelegate(PointerEventData data)
    {
        var script = tripod.GetComponent<VrGaze>();
        float azimuth = 0;
        String requestedID = data.pointerCurrentRaycast.gameObject.name.Substring(7);
        string fromPoint = script.last;
        if (fromPoint.Equals("")) { fromPoint = "1"; azimuth = 0;}
        else azimuth = azimuts[(Int32.Parse(fromPoint), Int32.Parse(requestedID))];
        script.GVRon(requestedID, azimuth, lastSphere);
    }
    private void OnPointerExitDelegate(PointerEventData data)
    {
        var script = tripod.GetComponent<VrGaze>();
        script.GVROff();
    }
    private Texture2D LoadPNG(string filePath)
    {
        Texture2D tex = new Texture2D(2, 2);
        try
        {
            byte[] bytes = File.ReadAllBytes(filePath);
            tex.LoadImage(bytes);
        }
        catch (DirectoryNotFoundException e) { SceneManager.LoadScene("InsertJson"); }
        return tex;
    }
    private Sprite HotspotPic(Texture2D pic)
    {
        return Sprite.Create(pic, new Rect(0, 0, pic.width, pic.height), new Vector3(0.1f, 0.1f));
    }
}