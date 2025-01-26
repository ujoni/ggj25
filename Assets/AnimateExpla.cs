using UnityEngine;

public class AnimateExpla : MonoBehaviour
{
    public Sprite[] images;
    float starttime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Toggle()
    {
        GetComponent<UnityEngine.UI.Image>().enabled = !GetComponent<UnityEngine.UI.Image>().enabled;
        if (GetComponent<UnityEngine.UI.Image>().enabled){
            starttime = Time.time;
        }
    }

    // Update is called once per frame
    void Update()
    {
        int p = (int)Mathf.Repeat(Time.time - starttime, 4);
        if (p >= 2) p = 2;
        //if (p == 3) p = 1;
        GetComponent<UnityEngine.UI.Image>().sprite = images[p];

    }
}
