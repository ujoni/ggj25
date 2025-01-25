using UnityEngine;

public class Collectable : MonoBehaviour
{
    private bool collected = false;

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
        if (!collected && collider.TryGetComponent<Turtle>(out var turtle))
        {
            collected = true;
            GetComponent<SpriteRenderer>().color = Color.red;
            turtle.Collect(cData);
        }
    }
}

