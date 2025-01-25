using UnityEngine;

public class MedusaScript : MonoBehaviour
{

    public Sprite[] images;

    float animpos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        animpos += 0.1f;
        float pp = Mathf.PingPong(animpos,5);
        if (pp == 5) pp = 4;

        GetComponent<SpriteRenderer>().sprite = images[(int)pp];
        transform.localPosition = new Vector3(0, -Mathf.Cos(animpos/10*Mathf.PI*2), 0);
    }
}
