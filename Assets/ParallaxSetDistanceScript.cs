using UnityEngine;
using V3 = UnityEngine.Vector3;

public class ParallaxSetDistanceScript : MonoBehaviour
{
    float initdepth;
    GameObject player;
    GameObject top;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Sukeltaja");
        top = GameObject.Find("Top");
        initdepth = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        float diff = top.transform.position.y - player.transform.position.y;
        if (diff > 100) diff = 100;
        if (diff < 30) diff = 30;
        diff = diff - 30;
        transform.position = new V3(transform.position.x,
            transform.position.y, 1 + (diff / 70 * initdepth + 0.03f));
    }
}
