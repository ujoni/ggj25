using UnityEngine;

public class RotationWiggle : MonoBehaviour
{
    float wiggle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wiggle = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float newwiggle = Mathf.Sin(Time.time*5)*15;
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + newwiggle - wiggle);
        wiggle = newwiggle;
    }
}
