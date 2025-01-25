using UnityEngine;

public class GiveBreath : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnCollisionEnter2D(Collision2D coll){
        if (coll.gameObject.name == "Sukeltaja") {
            coll.gameObject.GetComponent<sukeltajascript>().Hurt(-100, Vector3.zero);
        }
    }
}
