using UnityEngine;

public class CameraLogic : MonoBehaviour
{
GameObject player;
    float top;
    public float backwant;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        player = GameObject.Find("Sukeltaja");
        // bottom = 0
        top = GameObject.Find("Top").transform.position.y;


    }

    // Update is called once per frame
    void Update()
    {
        //float depth = player;
        
        float depth = (top - player.transform.position.y) / 40;
        if (depth < 0.7f) depth = 0.7f;
        if (depth > 1) depth = 1;
        backwant = depth * 30;
    }
}
