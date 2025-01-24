using System;
using UnityEngine;

[Serializable]
public class TurtleData
{
    public float speed = 0;
    public int collected = 0;
}

[Serializable]
public class CollectableData
{
    public string name = "";
}