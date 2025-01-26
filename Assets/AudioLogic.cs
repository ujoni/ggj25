using System;
using UnityEngine;
using UnityEngine.Video;
using V2 = UnityEngine.Vector2;
using V3 = UnityEngine.Vector3;
public class AudioLogic : MonoBehaviour
{
    GameObject player;
    float top;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        player = GameObject.Find("Sukeltaja");
        // bottom = 0
        top = GameObject.Find("Top").transform.position.y;

        transform.Find("Ambient").GetComponent<AudioSource>().volume = 0.7f;

    }

    // Update is called once per frame
    void Update()
    {
        //float depth = player;
        
        float depth = (top - player.transform.position.y) / top;
        if (depth < 0) depth = 0;
        if (depth > 1) depth = 1;

        float depth2_vol = UnitClamp(depth, 0.1f, 0.2f);
        transform.Find("Depth2").GetComponent<AudioSource>().volume = depth2_vol * 0.7f;

        float depth3_vol = UnitClamp(depth, 0.2f, 0.4f);
        transform.Find("Depth3").GetComponent<AudioSource>().volume = depth3_vol * 0.7f;

        Collider2D[] colls = Physics2D.OverlapBoxAll(player.transform.position, new V2(80, 80), 0);

        float d1 = 1000;
        float d2 = 1000;
        foreach (Collider2D coll in colls) {
            if (coll.GetComponent<CreatureScript>()) {
                d1 = Mathf.Min(d1, V3.Distance(player.transform.position, coll.transform.position));
            }
            if (coll.GetComponent<CreatureScript>() && coll.GetComponent<DangerDanger>()) {
                d2 = Mathf.Min(d2, V3.Distance(player.transform.position, coll.transform.position));
            }
        }
        float intense1_vol = UnitClamp(40 - d1, 0, 40);
        float intense2_vol = UnitClamp(40 - d2, 0, 40);
        intense1_vol = Mathf.Max(0, Mathf.Min(intense1_vol, (1 - intense2_vol*3)));
        transform.Find("Intense").GetComponent<AudioSource>().volume = intense1_vol;
        transform.Find("Intense2").GetComponent<AudioSource>().volume = intense2_vol;

        if (Time.time > player.GetComponent<sukeltajascript>().conchaaudio + 2.5f) {  
            transform.Find("Collect").GetComponent<AudioSource>().volume = 0;
        } else if (Time.time < player.GetComponent<sukeltajascript>().conchaaudio) {
            transform.Find("Collect").GetComponent<AudioSource>().volume = 0.7f;
        } else {
            float linear = (Time.time - player.GetComponent<sukeltajascript>().conchaaudio) / 2.5f;
            transform.Find("Collect").GetComponent<AudioSource>().volume = 0.7f * (1 - linear);
        }
    }

    float UnitClamp(float a, float b, float c){
        return (Mathf.Clamp(a, b, c) - b)/(c - b);
    }
}
