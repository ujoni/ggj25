using UnityEngine;

public class RemoveBubblesWhenHit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnCollisionEnter2D(Collision2D coll){
        if (coll.gameObject.layer == LayerMask.NameToLayer("Bubble") &&
            coll.gameObject.layer == LayerMask.NameToLayer("BubbleCO")){
                Destroy(coll.gameObject);
            }
    }
}
