using System;
using UnityEngine;

[Serializable]
public class TurtleData
{
    public float speed = 0;
    public int shells = 0;
    public float depth = 0;
    public float oxygen = 100f;
    public float maxOxygen = 100f;
}

public enum CollectableType
{
    None,
    NormalShell,
    BigShell,
    RainbowShell,
    SmallBubble,
    BigBubble,
}

[Serializable]
public class CollectableData
{
    public CollectableType collectableType = CollectableType.None;
}