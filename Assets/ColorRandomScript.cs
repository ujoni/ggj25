using UnityEngine;

public class ColorRandomScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        GetComponent<SpriteRenderer>().color = new Color(Random.Range(10,255), Random.Range(10,255), Random.Range(10,255));
    }

}
