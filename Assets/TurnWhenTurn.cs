using System.IO.Compression;
using UnityEngine;
using V3 = UnityEngine.Vector3;

public class TurnWhenTurn : MonoBehaviour
{
    // for some reason, most creatures look left initially
    public bool looksright;
    public bool flipped;
    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 10) == 0) {
            if (transform.rotation.eulerAngles.z > 90 && transform.rotation.eulerAngles.z < 270) {
                if (!flipped ^ looksright) {
                    flipped = true ^ looksright;
                    transform.localScale = new V3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
                }
            } else{
                if (flipped ^ looksright){
                    flipped = false ^ looksright;
                    transform.localScale = new V3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
                }
            }
        }
    }
}
