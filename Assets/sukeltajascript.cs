using System.Numerics;
//using Unity.VisualScripting;
using UnityEngine;
using V2 = UnityEngine.Vector2;
using V3 = UnityEngine.Vector3;

public class sukeltajascript : MonoBehaviour
{

    V2 movedir;
    float speed;
    int sivusuunta;
    float wantrot;

    KuplaMittariScript mittari;
    public GameObject bubb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        mittari = GameObject.Find("Mittari").GetComponent<KuplaMittariScript>();
        speed = 0.2f;   
        sivusuunta = 1;
    }

    void Bubb(){
        GameObject go = GameObject.Instantiate(bubb);
        go.transform.position = transform.GetChild(0).position;
        go.GetComponent<Rigidbody2D>().linearVelocity = V3.right * sivusuunta*2 + (V3)movedir*0.5f;

        float r = Random.Range(0.3f, 0.7f);
        go.transform.localScale = new V3(r*go.transform.localScale.x,
            r*go.transform.localScale.y, 1);
        Destroy(go, 10 + 20*Random.Range(0f, 1f));
    }

    void FixedUpdate(){
        if (Random.Range(0, 100) == 0){
            mittari.Lose(0.2f);
            int amt = Random.Range(3,10);
            for (int i = 0; i < amt; i++){
                Bubb();
            }
        }

        transform.position += (V3)movedir*speed;
        if (movedir.x > 0) {
            //GetComponent<SpriteRenderer>().flipX = false;
            transform.localScale = new V3(0.5f,0.5f,1);
            sivusuunta = 1;
        }
        if (movedir.x < 0) {
            //GetComponent<SpriteRenderer>().flipX = true;
            transform.localScale = new V3(-0.5f,0.5f,1);
            sivusuunta = -1;
        }

        float r = transform.rotation.eulerAngles.z;
        
        //int k = 0; //r < 180 ? -1 : 1;
        // wantrot is always between -90 and 90 in fact
        float otherside = wantrot + 180;

        float end = 0;
        // case where we are in 0..wantrot segment
        if (r < wantrot) end = wantrot; // ? 0 : 360;
        else if (r < otherside) end = wantrot;
        else end = wantrot + 360;
        transform.rotation =
            UnityEngine.Quaternion.Euler(0, 0, Mathf.Lerp(r, end, 0.2f));
        
        


    }

    public void EatBubble(){
        mittari.EatBubble();
    }

    // Update is called once per frame
    void Update()
    {
        movedir = V2.zero;
        if (Input.GetKey(KeyCode.UpArrow)){
            movedir += V2.up;
            wantrot = sivusuunta==1 ? 45 : -45;
        }
        else if (Input.GetKey(KeyCode.DownArrow)){
            movedir += V2.down;
            wantrot = sivusuunta==1 ? -45 : 45;
        }
        else{
            wantrot = 0;
        }
        if (Input.GetKey(KeyCode.LeftArrow)){
            movedir += V2.left;
        }
        if (Input.GetKey(KeyCode.RightArrow)){
            movedir += V2.right;
        }
        movedir = movedir.normalized;

    }
}
