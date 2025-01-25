using UnityEngine;

public class ImeKupliaScript : MonoBehaviour
{
    sukeltajascript s;
    void Awake(){
        s = GameObject.Find("Sukeltaja").GetComponent<sukeltajascript>();
    }

    void OnCollisionEnter2D(Collision2D coll){
        GameObject.Destroy(coll.gameObject);
        s.EatBubble();
    }
}
