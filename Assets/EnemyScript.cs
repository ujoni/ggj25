using UnityEngine;
using UnityEngine.Video;
using V3 = UnityEngine.Vector3;

public class EnemyScript : MonoBehaviour
{
    public float damage;

    void OnCollisionEnter2D(Collision2D collider)
    {
        //print("kekk" + collider.GetContact(0).collider.gameObject.name);
        if (collider.gameObject.name == "Sukeltaja")//TryGetComponent<sukeltajascript>(out var turtle))
        {
            
            collider.gameObject.GetComponent<sukeltajascript>().Hurt(damage, ((V3)collider.contacts[0].point - transform.position).normalized);            
        }
    }

    
}
