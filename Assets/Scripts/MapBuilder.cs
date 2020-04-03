using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MapBuilder : MonoBehaviour 
{
    GameObject sphere;
    Material material;
    Renderer renderer;
    Texture2D hotspotPic;
    private string hotspotName = "Textures/arrow";
    private string shaderStyle = "Insideout";
    private Sprite hotspot;
    private SpriteRenderer sr;
    private List<GameObject> sp = new List<GameObject>();
    SphereChanger sphereChanger;
    private float sphereScale = 3;


    public void BuildMap(Parser.Points points)
    {
        sphereChanger = (new GameObject("SphereChanger")).AddComponent<SphereChanger>();
        //for loop for every pic and pos x = 3.5 * i
        for (int i = 0; i < points.points.Length; i++)
        {
            sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = new Vector3(3.5f * i, 0, 0);
            sphere.name = sphere.name + i;
            sphere.transform.localScale = new Vector3(sphereScale, sphereScale, sphereScale);
            sphere.gameObject.GetComponent<SphereCollider>().enabled = false;
            renderer = sphere.GetComponent<Renderer>();

            material = new Material(Shader.Find(shaderStyle));
            sphere.GetComponent<Renderer>().material = material;
            var picture = Resources.Load<Texture2D>(points.points[i].Picture);
            renderer.material.mainTexture = picture;
            hotspotPic = Resources.Load<Texture2D>(hotspotName);
            sp.Add(sphere);
        }
        for (int i = 0; i < points.points.Length; i++)
        {
            foreach (Parser.Neighbor nei in points.points[i].Neighbors)
            {
                GameObject go = new GameObject("hotspot"+nei.PointID);
                go.transform.parent = sp[i].transform;
                sr = go.AddComponent<SpriteRenderer>() as SpriteRenderer;
                sr.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
                var sprite = Sprite.Create(hotspotPic, new Rect(0, 0, hotspotPic.width, hotspotPic.height), new Vector3(0.1f, 0.1f));
                go.transform.localPosition= new Vector3(0, 0-0.03f, 0-0.4f);
                sr.sprite = sprite;
                SphereCollider sc = go.AddComponent(typeof(SphereCollider)) as SphereCollider;
                EventTrigger trigger = go.AddComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerDown;
                entry.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data,nei.PointID); });
                trigger.triggers.Add(entry);
            }
        }
    }

    private void OnPointerDownDelegate(PointerEventData data, int PointID)
    {
        GameObject wantedSphere = GameObject.Find("Sphere"+PointID);
        sphereChanger.ChangeSphere(wantedSphere.transform);
    }
}
