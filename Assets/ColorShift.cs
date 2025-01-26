using UnityEngine;

public class ColorShift : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Color c = GetComponent<SpriteRenderer>().color;
        Color c2 = new Color(c.r + Random.Range(-0.05f, 0.05f),
        c.g + Random.Range(-0.05f, 0.05f), c.b + Random.Range(-0.05f, 0.05f));
        GetComponent<SpriteRenderer>().color = c2;
    }
}
