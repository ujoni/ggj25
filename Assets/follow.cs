using UnityEngine;
using V3 = UnityEngine.Vector3;

public class follow : MonoBehaviour
{
    public float speed;
    public GameObject followed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var wannabe = followed.transform.position - V3.forward*30;
        
        
        float distance = V3.Distance(transform.position, wannabe);
        var distanceover = Mathf.Max(0, distance);
        float ss = 0;
        ss = 0.02f + distanceover; 
        transform.position = V3.MoveTowards(transform.position, wannabe, ss);

        transform.LookAt(followed.transform.position);

    }
}
