using UnityEngine;

public class RandomZoomsScript : MonoBehaviour
{
    Vector3 zoomdir;
    public float strength;

    void Start(){
        if (strength == 0) strength = 10;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Random.Range(0, 50) == 0){
            if (false && Random.Range(0,5) > 0) {
                zoomdir = Vector3.zero;
            }else{
                zoomdir = (Vector3)Random.insideUnitCircle;
            }
        }

        GetComponent<Rigidbody2D>().AddForce(zoomdir*strength);
    }
}
