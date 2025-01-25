using UnityEngine;

public class driftscript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float xx = PerlinNoise3D(transform.position.x, transform.position.y, Time.time/10) - 0.5f;
        float yy = PerlinNoise3D(transform.position.y, transform.position.x+1000.23f, Time.time/10) - 0.5f;
        //print (new Vector2(xx, yy));
        GetComponent<Rigidbody2D>().AddForce(new Vector2(xx, yy));
    }

    public static float PerlinNoise3D(float x, float y, float z)
    {
        // Combine 2D Perlin Noise with offsets to simulate 3D
        float xy = Mathf.PerlinNoise(x+345.2f, y);
        float xz = Mathf.PerlinNoise(x, z+34545.2f);
        float yz = Mathf.PerlinNoise(y+3.4f, z+ 4);
        float yx = Mathf.PerlinNoise(y, x+65.2f);
        float zx = Mathf.PerlinNoise(z+ 65.1f, x);
        float zy = Mathf.PerlinNoise(z, y);

        // Average the noise values to create 3D-like noise
        return (xy + xz + yz + yx + zx + zy) / 6f;
    }
}
