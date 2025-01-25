using UnityEngine;

public class fishscrip : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject != transform.parent.gameObject){
            transform.parent.GetComponent<Rigidbody2D>().AddForce((coll.transform.position - transform.position).normalized*10);
        }
    }
}
