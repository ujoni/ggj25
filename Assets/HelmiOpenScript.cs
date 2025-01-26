using UnityEngine;

public class HelmiOpenScript : MonoBehaviour
{
    public Sprite[] dems;
    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().sprite = dems[(int)(0.5f + Mathf.PingPong(Time.time, 1.99f))];
    }
}
