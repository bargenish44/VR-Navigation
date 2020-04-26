using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

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


    public void BuildMap(Parser.Points points)
    {
        hotspotPic = Resources.Load<Texture2D>(hotspotName);
        sphereChanger = (new GameObject("SphereChanger")).AddComponent<SphereChanger>();
        for (int i = 0; i < points.points.Length; i++)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = new Vector3((sphereScale+0.5f) * i, 0, 0); 
            sphere.name = sphere.name + points.points[i].id;
            sphere.transform.localScale = new Vector3(sphereScale, sphereScale, sphereScale);
            sphere.gameObject.GetComponent<SphereCollider>().enabled = false;
            renderer = sphere.GetComponent<Renderer>();

            material = new Material(Shader.Find(shaderStyle));
            sphere.GetComponent<Renderer>().material = material;
            var picture = Resources.Load<Texture2D>(points.points[i].Picture);
            renderer.material.mainTexture = picture;
            sp.Add(sphere);
        }
        for (int i = 0; i < points.points.Length; i++)
        {
            for(int j=0;j<points.points[i].Neighbors.Length;j++)
            {
                fromPointID = points.points[i].id;
                toPointID = points.points[i].Neighbors[j].PointID;
                Debug.LogWarning("sphere" + fromPointID + " have sphere" + toPointID + " as neighbor");
                GameObject go = new GameObject("hotspot"+ toPointID);
                go.transform.parent = sp[i].transform;
                sr = go.AddComponent<SpriteRenderer>() as SpriteRenderer;
                float wantedAzimuthDeg = points.points[i].Neighbors[j].Azimut;
                float wantedAzimuthRad = wantedAzimuthDeg * Mathf.PI / 180;
                float x = 0.4f * Mathf.Cos(-wantedAzimuthRad);
                float y = 0; 
                float z = 0.4f * Mathf.Sin(-wantedAzimuthRad);
                go.transform.localRotation = Quaternion.Euler(0, wantedAzimuthDeg+90, 0);
                sr.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
                var sprite = Sprite.Create(hotspotPic, new Rect(0, 0, hotspotPic.width, hotspotPic.height), new Vector3(0.1f, 0.1f));
                go.transform.localPosition= new Vector3(x, y, z);
                sr.sprite = sprite;
                SphereCollider sc = go.AddComponent(typeof(SphereCollider)) as SphereCollider;
                EventTrigger trigger = go.AddComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerDown;
                entry.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
                trigger.triggers.Add(entry);
                Debug.Log(trigger.triggers.Count+"\t"+fromPointID + "\ttoPoint : "+toPointID);
            }
        }
    }

    private void OnPointerDownDelegate(PointerEventData data)
    {
        Debug.Log(data);
        String requestedID = data.pointerPressRaycast.gameObject.name.Substring(7);
        Debug.LogError("PointerDown");
        Debug.LogError("from pointID is : " + fromPointID + "\tToPointID is : " + requestedID);
        GameObject wantedSphere = GameObject.Find("Sphere"+ requestedID);
        Debug.Log("toSphere before : "+wantedSphere.name);
        sphereChanger.ChangeSphere(wantedSphere.transform);
    }
}