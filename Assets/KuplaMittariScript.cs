using System;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class KuplaMittariScript : MonoBehaviour
{

    public int max;
    public float curr;
    public GameObject bubb;
    public List<GameObject> Bubbles;
    
    void Awake(){
        Bubbles = new List<GameObject>();
        BubbleCountFix();
    }

    public void Lose(float amt) {
        curr -= amt;
        if (curr == 0) curr = 0;
        BubbleCountFix();
    }

    void BubbleCountFix(){
        while (Bubbles.Count < max){
            GameObject bu = GameObject.Instantiate(bubb, GameObject.Find("Canvas").transform);
            Bubbles.Add(bu);
        }
        while (Bubbles.Count > max) {
            GameObject.Destroy(Bubbles[Bubbles.Count-1]);
            Bubbles.RemoveAt(Bubbles.Count-1);
        }
        curr = Mathf.Min(curr, max);

        for(int i = 0; i < Bubbles.Count; i++){
            Bubbles[i].transform.position = new Vector3(50 + i*30, 50, 0);
            if (i >= curr) {
                Bubbles[i].GetComponent<UnityEngine.UI.Image>().color = new Color32(255, 255, 255, 50);
            }
            else {
                Bubbles[i].GetComponent<UnityEngine.UI.Image>().color = new Color32(255, 255, 255, 255);
            }
        }

    }

    public void EatBubble(){
        curr += 1;
        curr = Mathf.Min(curr, max);
        BubbleCountFix();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
