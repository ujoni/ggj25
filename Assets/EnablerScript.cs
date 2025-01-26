using System;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.SceneManagement;
using V2 = UnityEngine.Vector2;
using V3 = UnityEngine.Vector3;
using TMPro;

public class EnablerScript : MonoBehaviour
{
    public List<GameObject> objects;
    public List<GameObject> actives;
    GameObject player;
    int acts;
    int inacts;
    int c ;
    bool initialized = false;
    GameObject loggero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start(){
        loggero = GameObject.Find("Loggero");
        Initialize();
    }
    public void Initialize()
    {

        actives = new List<GameObject>();
        initialized = true;
        player = GameObject.Find("Sukeltaja");
        c = 0;
         
         foreach(GameObject obj in objects){
            InActonate(obj);
         }
         //print(rootobjects.Count);
         ActivateClose();
    }

    void ActivateClose() {
        acts = 0;
        Collider2D[] colls = Physics2D.OverlapBoxAll(player.transform.position, new V2(80, 80), 0);
        foreach (Collider2D coll in colls) {
            if (objects.Contains(coll.gameObject)){
                if (coll.gameObject.GetComponent<Renderer>().enabled == false) acts += 1;
                Actonate(coll.gameObject);
                
                if (!actives.Contains(coll.gameObject)) actives.Add(coll.gameObject);
            }
        }
        //print(actives.Count);
        
    }

    void InActivateFar() {
         inacts = 0;
        List<GameObject> newactives = new List<GameObject>();
        foreach (GameObject obj in actives) {
            if (obj == null) {
                inacts += 1;
                continue;
                
            }
            if (V3.Distance(obj.transform.position, player.transform.position) > 80){
                if (obj.GetComponent<Renderer>().enabled == true) inacts += 1;
                InActonate(obj);
            }
            else {
                newactives.Add(obj);
            }
        }
        actives = newactives;
    }

    void Actonate(GameObject go){
        MonoBehaviour[] monos = go.GetComponents<MonoBehaviour>();
        foreach(MonoBehaviour mono in monos){
            mono.enabled = true;
        }
        foreach(Renderer r in go.GetComponents<Renderer>()) {
            r.enabled = true;
        }
        foreach(Rigidbody2D rb in go.GetComponents<Rigidbody2D>()) {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    void InActonate(GameObject go){
        MonoBehaviour[] monos = go.GetComponents<MonoBehaviour>();
        foreach(MonoBehaviour mono in monos){
            mono.enabled = false;
        }
        foreach(Renderer r in go.GetComponents<Renderer>()) {
            r.enabled = false;
        }
        foreach(Rigidbody2D rb in go.GetComponents<Rigidbody2D>()) {
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

    void Update(){
        if (!initialized) return;
        c++;
        if (c % 50 == 0){
            ActivateClose();
            InActivateFar();
            loggero.GetComponent<TMP_Text>().text = Time.time + " activated " + acts + " inactiavted "+ inacts + " curr active  " +actives.Count;
        }
    }
}
