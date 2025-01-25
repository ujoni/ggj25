using System.Numerics;
//using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using V2 = UnityEngine.Vector2;
using V3 = UnityEngine.Vector3;
using UnityEngine.UIElements;
public class sukeltajascript : MonoBehaviour
{

    V2 movedir;
    float speed;
    int sivusuunta;
    float wantrot;

    float animpos;
    public bool bodycollect;
    float lastflip;

    KuplaMittariScript mittari;
    public GameObject bubb;

    public TurtleData dTurtle = new TurtleData();
    public Sprite[] images;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        animpos = 0;

        var ui = FindFirstObjectByType<UIDocument>();
        var turtleBar = ui.rootVisualElement.Children().First().Children().Single(element => element.viewDataKey == "TurtleBar");
        turtleBar.dataSource = dTurtle;

        //mittari = GameObject.Find("Mittari").GetComponent<KuplaMittariScript>();
        speed = 0.2f;
        sivusuunta = 1;
    }

    void Bubb()
    {
        GameObject go = GameObject.Instantiate(bubb);
        go.transform.position = transform.GetChild(0).position + V3.right * 0.7f * sivusuunta;
        go.GetComponent<Rigidbody2D>().linearVelocity = V3.right * sivusuunta * 2 + (V3)movedir * 0.5f + (V3)Random.insideUnitCircle * 0.1f;

        float r = Random.Range(0.7f, 0.9f);
        go.transform.localScale = new V3(r * go.transform.localScale.x,
            r * go.transform.localScale.y, 1);
        Destroy(go, 10 + 20 * Random.Range(0f, 1f));
    }

    void Fart()
    {
        GameObject go = GameObject.Instantiate(bubb);
        go.transform.position = transform.position - transform.right * 1.65f * sivusuunta;
        go.GetComponent<Rigidbody2D>().linearVelocity = V3.left * sivusuunta * 2 - (V3)movedir * 0.5f + (V3)Random.insideUnitCircle * 0.1f;

        float r = Random.Range(0.8f, 1.1f);
        go.transform.localScale = new V3(r * go.transform.localScale.x,
            r * go.transform.localScale.y, 1);
        Destroy(go, 10 + 20 * Random.Range(0f, 1f));
    }

    void MouthBubb(float oxy)
    {
        dTurtle.oxygen -= oxy;

        if (oxy <= 0) return;
        int amt = Random.Range((int)(2 * oxy), (int)(5 * oxy));
        if (amt > 30) amt = 30;
        for (int i = 0; i < amt; i++)
        {
            Bubb();
        }
    }

    void ArseBubb(float oxy)
    {
        dTurtle.oxygen -= oxy;

        int amt = Random.Range((int)(4 * oxy), (int)(8 * oxy));
        if (amt > 30) amt = 30;
        for (int i = 0; i < amt; i++)
        {
            Fart();
        }
    }

    void FixedUpdate()
    {
        if (Random.Range(0, 120) == 0)
        {
            //mittari.Lose(0.2f);

            MouthBubb(0.5f);
        }

        if (Random.Range(0, 800) == 0)
        {
            //mittari.Lose(0.4f);

            ArseBubb(1f);
        }
        AnimationStuff();


        transform.position += (V3)movedir * speed;
        if (movedir.x > 0)
        {
            //GetComponent<SpriteRenderer>().flipX = false;
            transform.localScale = new V3(0.5f, 0.5f, 1);
            if (sivusuunta == -1) lastflip = Time.time;
            sivusuunta = 1;
        }
        if (movedir.x < 0)
        {
            //GetComponent<SpriteRenderer>().flipX = true;
            transform.localScale = new V3(-0.5f, 0.5f, 1);
            if (sivusuunta == 1) lastflip = Time.time;
            sivusuunta = -1;
        }

        bodycollect = Time.time - lastflip < 1;

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

    public void Hurt(float damage, V3 impulse)
    {
        MouthBubb(damage);
        if (damage > 0)
            GetComponent<Rigidbody2D>().AddForce(impulse*1000);
    }

    void AnimationStuff()
    {
        if (movedir != V2.zero)
        {
            animpos += 0.15f;
        }
        else
        {
            animpos += 0.075f;
        }
        //if (animpos > 6) animpos -= 6;
        float animposo = Mathf.Repeat(animpos, 6);

        if (movedir != V2.zero)
        {
            if (animposo < 1) GetComponent<SpriteRenderer>().sprite = images[0];
            else if (animposo < 2) GetComponent<SpriteRenderer>().sprite = images[1];
            else if (animposo < 3) GetComponent<SpriteRenderer>().sprite = images[2];
            else if (animposo < 4) GetComponent<SpriteRenderer>().sprite = images[3];
            else if (animposo < 5) GetComponent<SpriteRenderer>().sprite = images[2];
            else GetComponent<SpriteRenderer>().sprite = images[1];
        }
        else
        {
            if (animposo < 1) GetComponent<SpriteRenderer>().sprite = images[1];
            else if (animposo < 2) GetComponent<SpriteRenderer>().sprite = images[4];
            else if (animposo < 3) GetComponent<SpriteRenderer>().sprite = images[5];
            else if (animposo < 4) GetComponent<SpriteRenderer>().sprite = images[2];
            else if (animposo < 5) GetComponent<SpriteRenderer>().sprite = images[5];
            else GetComponent<SpriteRenderer>().sprite = images[4];
        }
        transform.GetChild(1).localRotation = UnityEngine.Quaternion.Euler(0, 0,
            Mathf.Sin(animpos * 1.5f) * 7 - 25);
    }

    public void EatBubble()
    {
        //mittari.EatBubble();
    }

    // Update is called once per frame
    void Update()
    {
        movedir = V2.zero;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            movedir += V2.up;
            wantrot = sivusuunta == 1 ? 45 : -45;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            movedir += V2.down;
            wantrot = sivusuunta == 1 ? -45 : 45;
        }
        else
        {
            wantrot = 0;
        }
        wantrot += Mathf.Sin(Time.time) * 4f;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movedir += V2.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            movedir += V2.right;
        }
        movedir = movedir.normalized;

    }

    public void Collect(CollectableData c)
    {
        switch (c.collectableType)
        {
            case CollectableType.NormalShell:
                dTurtle.shells += 1;
                break;
            case CollectableType.BigShell:
                dTurtle.shells += 4;
                break;
            case CollectableType.RainbowShell:
                dTurtle.shells += 15;
                break;
            case CollectableType.SmallBubble:
                dTurtle.oxygen = Mathf.Min(dTurtle.maxOxygen, dTurtle.oxygen + 3);
                break;
            case CollectableType.BigBubble:
                dTurtle.oxygen = Mathf.Min(dTurtle.maxOxygen, dTurtle.oxygen + 10);
                break;
        }
    }
}


