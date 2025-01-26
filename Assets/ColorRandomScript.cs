using UnityEngine;

public class ColorRandomScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        GetComponent<SpriteRenderer>().color = new Color(Random.Range(0.05f,1f),Random.Range(0.05f,1f),Random.Range(0.05f,1f));
        Destroy(this);
    }

}
