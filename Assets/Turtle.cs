using System;
using System.Linq;
using System.Runtime.Serialization;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.U2D.IK;
using UnityEngine.UIElements;

[Serializable]
public class Turtle : MonoBehaviour
{

    public VisualTreeAsset turtleDisplay;
    private Vector3 dir = Vector3.zero;

    private Vector3 vel = Vector3.zero;
    private KeyCode key;

    private TurtleData tData = new TurtleData();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var ui = FindFirstObjectByType<UIDocument>();
        var turtleBar = ui.rootVisualElement.Children().First().Children().Single(element => element.viewDataKey == "TurtleBar");
        turtleBar.dataSource = tData;
    }

    void FixedUpdate()
    {
        GetComponent<Transform>().position += vel;
        tData.oxygen -= 0.02f;
    }

    // Update is called once per frame
    void Update()
    {
        KeyCode cur = KeyCode.None;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            dir = Vector3.left;
            cur = KeyCode.LeftArrow;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            dir = Vector3.right;
            cur = KeyCode.RightArrow;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            dir = Vector3.up;
            cur = KeyCode.UpArrow;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            dir = Vector3.down;
            cur = KeyCode.DownArrow;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            GetComponent<Transform>().position = Vector3.zero;
            tData.speed = 0;
            vel = Vector3.zero;
        }

        if (cur != KeyCode.None && key == cur)
        {
            tData.speed = Math.Min(0.5f, tData.speed + 0.1f * Time.deltaTime);
        }
        else
        {
            tData.speed = 0;
        }

        key = cur;

        vel = (vel + dir * tData.speed) * 0.95f;
        if (vel.magnitude > 0.8f)
        {
            vel = vel.normalized;
        }
    }


    public void Collect(CollectableData c)
    {
        switch (c.collectableType)
        {
            case CollectableType.NormalShell:
                tData.shells += 1;
                break;
            case CollectableType.SmallBubble:
                tData.oxygen = Math.Min(tData.maxOxygen, tData.oxygen + 5);
                break;
        }
    }
}
