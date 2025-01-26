using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Video;
using V2 = UnityEngine.Vector2;
using V3 = UnityEngine.Vector3;

public class FollowFriend : MonoBehaviour
{
    public float strength;
    GameObject followed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (strength == 0) strength = 5;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Random.Range(0, 100) == 0){
            Collider2D[] colls = Physics2D.OverlapBoxAll(transform.position, new V2(10, 10), 0);
            float mind = 1000;
            followed = null;
            foreach (Collider2D c in colls){
                //if (c.gameObject.layer)
                float d = V3.Distance(c.gameObject.transform.position, transform.position);
                if (d < 0.5f) continue;
                if (d < mind || (Random.Range(0,2) == 0 && d < 10 &&
                    c.gameObject.layer == LayerMask.NameToLayer("Turtle"))){
                    mind = d;
                    followed = c.gameObject;
                }
            }

        }

        if (followed != null) {
            V3 off = (followed.transform.position - transform.position).normalized;
            GetComponent<Rigidbody2D>().AddForce(off*strength);
            //transform.LookAt(followed);
            transform.rotation = Quaternion.Euler(0, 0, -Mathf.Atan2(off.y, off.x)*Mathf.Rad2Deg) ;
        }
    }
}
