using UnityEditor.Rendering;
using UnityEngine;
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
            Collider2D[] colls = Physics2D.OverlapBoxAll(transform.position, new V2(15, 15), 0);
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

        if (Random.Range(0, 1000) == 0){
            followed = null;
        }

        /*GameObject player = GameObject.Find("Sukeltaja");
        followed = player;*/
        if (followed != null) {
            V3 off = (followed.transform.position - transform.position).normalized;
            // V3 off = (player.transform.position - transform.position).normalized;
            GetComponent<Rigidbody2D>().AddForce(off * strength);
            //GetComponent<Rigidbody2D>().AddForce(-transform.right * strength);
            //transform.LookAt(followed);


            float r = transform.rotation.eulerAngles.z;
            //float wantrot = Mod(180 + Mathf.Atan2(off.y, off.x)*Mathf.Rad2Deg, r - 180, r + 180);
            float wantrot = 180 + Mathf.Atan2(off.y, off.x)*Mathf.Rad2Deg;

            transform.rotation = //UnityEngine.Quaternion.Euler(0,0,wantrot);
                    UnityEngine.Quaternion.Euler(0, 0, wantrot /*Mathf.MoveTowards(r, wantrot, 30)*/);
        }
    }

    float Mod(float a, float b, float c){
        while (a < b) {
            a += c - b;
        }
        while (a > c) {
            a -= c - b;
        }
        return a;
    }
}
