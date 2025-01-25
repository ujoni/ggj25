using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public enum UpgradeType
{
    None,
    OxygenMaximum,
    Speed,
    Toughness,
    CarryingCapacity,
}

[Serializable]
public class TurtleUpgrade
{
    private static readonly Dictionary<UpgradeType, float> pricePerType = new Dictionary<UpgradeType, float> {
        { UpgradeType.CarryingCapacity, 5 },
        { UpgradeType.OxygenMaximum, 12 },
        { UpgradeType.Speed, 15 },
        { UpgradeType.Toughness, 6 },
    };

    public int level = 0;
    public UpgradeType type = UpgradeType.None;

    public float GetPrice()
    {
        return level * pricePerType[type];
    }
}

[Serializable]
public class TurtleInventory
{
    public TurtleUpgrade[] inventory = new TurtleUpgrade[13];
}

[Serializable]
public class TurtleData
{
    public float speed = 0;
    public int shells = 0;
    public float depth = 0;
    public float oxygen = 100f;
    public float maxOxygen = 100f;
    public float carryingCapacity = 100f;
    public TurtleInventory inventory = new TurtleInventory();
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

[Serializable]
public class ShopData
{
    public List<TurtleUpgrade> items = new List<TurtleUpgrade>();
}