using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using V2 = UnityEngine.Vector2;

public class Helpers
{
    public static int c;

    /*
    Generate a random position between min and max.

    If slope = 0, then we just accept.
    If slope = 1, then we randomize a position after which we accept.
    If slope = -1, then we randomize a position before which we accept.

    If slope = a, then with probability 1 - |a| we just accept, then we do as if a = normalize(a).
    */
    public static float RandomWithSlope(float min, float max, float slope)
    {
        c = 0;
        //Debug.Log("c value " + c);
        while (true){
            c++;
            //Debug.Log("c value " + c);
            float val = Random.Range(min, max);
            float acc = Random.Range(0f, 1f);
            if (acc + Mathf.Abs(slope) < 1) return val;
            float threshold = Random.Range(min, max);
            //Debug.Log("value " + val);
            //Debug.Log("threshold " + threshold);
            //Debug.Log("check " + (slope < 0 && val <= threshold));
            //Debug.Log("c value " + c);
            if (slope >= 0 && val >= threshold) return val;
            if (slope < 0 && val <= threshold) return val;
            if (c > 10) {
                //Debug.Log("fail");
                return 0;
            }            
        }
    }
    public static T RandomChoice<T>(List<T> list)
    {
        int idx = Random.Range(0, list.Count);
        return list[idx];
    }

    public static V2 TurnLeft(V2 v)
    {
        return new V2(-v.y, v.x);
    }

    public static Transform Root(Transform t){
        while (t.parent != null) {
            t = t.parent;
        }
        return t;
    }
}
