using System.Collections.Generic;
using UnityEngine;
using V2 = UnityEngine.Vector2;
using V3 = UnityEngine.Vector3;
using TMPro;

// enabler script activates and inactivates all creatures based on distance from player
// never destroys or creates anything though
public class EnablerScript : MonoBehaviour
{
    public List<GameObject> objects;
    public List<GameObject> actives;
    GameObject player;
    int acts;
    int inacts;
    int c ;
    bool initialized = false;
    //GameObject loggero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start(){
        //loggero = GameObject.Find("Loggero");
        //Initialize();
    }

    public void Add(GameObject obj){
        objects.Add(obj);
        InActonate(obj);
    }
    public void Initialize()
    {

        actives = new List<GameObject>();
        initialized = true;
        player = GameObject.Find("Sukeltaja");
        c = 0;
         
         foreach(GameObject obj in objects){
            //print(obj);
            InActonate(obj);
         }
         //print(rootobjects.Count);
         ActivateClose();
    }

    void ActivateClose() {
        acts = 0;
        Collider2D[] colls = Physics2D.OverlapBoxAll(player.transform.position, new V2(80, 80), 0);
        foreach (Collider2D coll in colls) {
            GameObject obj = Helpers.Root(coll.transform).gameObject;
            if (objects.Contains(obj)){
                //if (coll.gameObject.GetComponent<Renderer>().enabled == false) acts += 1;
                Actonate(obj);
                
                if (!actives.Contains(obj)) actives.Add(obj);
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
                //if (obj.GetComponent<Renderer>().enabled == true) inacts += 1;
                InActonate(obj);
            }
            else {
                newactives.Add(obj);
            }
        }
        actives = newactives;
    }

    void Actonate(GameObject go){
        MonoBehaviour[] monos = go.GetComponentsInChildren<MonoBehaviour>();
        foreach(MonoBehaviour mono in monos){
            mono.enabled = true;
        }
        foreach(Renderer r in go.GetComponentsInChildren<Renderer>()) {
            r.enabled = true;
        }
        foreach(Rigidbody2D rb in go.GetComponentsInChildren<Rigidbody2D>()) {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    void InActonate(GameObject go){
        MonoBehaviour[] monos = go.GetComponentsInChildren<MonoBehaviour>();
        foreach(MonoBehaviour mono in monos){
            mono.enabled = false;
        }
        foreach(Renderer r in go.GetComponentsInChildren<Renderer>()) {
            r.enabled = false;
        }
        foreach(Rigidbody2D rb in go.GetComponentsInChildren<Rigidbody2D>()) {
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

    void Update(){
        if (!initialized) return;
        c++;
        if (c % 50 == 0){
            ActivateClose();
            InActivateFar();
            //loggero.GetComponent<TMP_Text>().text = Time.time + " activated " + acts + " inactiavted "+ inacts + " curr active  " +actives.Count;
        }
    }
}
