using UnityEngine;

public class Collectable : MonoBehaviour
{

    public CollectableData cData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("What");
        if (collider.TryGetComponent<Turtle>(out var turtle))
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            turtle.collect(cData);
        }
    }
}
