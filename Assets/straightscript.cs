using UnityEngine;

public class straightscript : MonoBehaviour
{
    void FixedUpdate(){
        // case where we are in 0..wantrot segment
        float end;
        if (transform.rotation.eulerAngles.z > 180) end = 360;
        else end = 0;
        transform.rotation =
            UnityEngine.Quaternion.Euler(0, 0, Mathf.Lerp(transform.rotation.eulerAngles.z, end, 0.1f));
    }
}
