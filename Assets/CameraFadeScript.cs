using UnityEngine;

public class CameraFadeScript : MonoBehaviour
{
    GameObject player;
    float top;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        player = GameObject.Find("Sukeltaja");
        // bottom = 0
        top = GameObject.Find("Top").transform.position.y;

    }
    
    void Update()
    {
        float depth = (top - player.transform.position.y) / top;
        if (depth < 0) depth = 0;
        if (depth > 1) depth = 1;
        Color col;
        if (depth < 0.3f)
        {
            col = new Color(1, 1 - UnitClamp(depth, 0, 0.3f), 1 - UnitClamp(depth, 0, 0.3f));
        }
        else
        {
            col = new Color(1 - UnitClamp(depth, 0.3f, 1), UnitClamp(depth, 0.3f, 1), 1 - UnitClamp(depth, 0.3f, 1));
        }
        GetComponent<Light>().color = col;
    }

    float UnitClamp(float a, float b, float c){
        return (Mathf.Clamp(a, b, c) - b)/(c - b);
    }
}
