using System;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;



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
    private static readonly Dictionary<UpgradeType, float> pricePerType = new()
    {
        { UpgradeType.CarryingCapacity, 5 },
        { UpgradeType.OxygenMaximum, 12 },
        { UpgradeType.Speed, 15 },
        { UpgradeType.Toughness, 6 },
    };

    public int level = 0;
    public UpgradeType type = UpgradeType.None;
    public Sprite icon;

    [SerializeField]
    public float price
    {
        get { return level * pricePerType[type]; }
        set { /* I do not actually care */ }
    }

    public static string LevelToString(int level)
    {
        return "lvl " + level.ToString();
    }

    internal static string TypeToString(UpgradeType type)
    {
        Debug.Log("Converting type");
        switch (type)
        {
            case UpgradeType.None: return "BUG!!";
            case UpgradeType.CarryingCapacity: return "Carrying capacity";
            case UpgradeType.OxygenMaximum: return "Oxygen maximum";
            case UpgradeType.Speed: return "Maximum speed";
            case UpgradeType.Toughness: return "Toughness";
        }
        return "PANIC!!";
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
    public TurtleInventory inventory = new();
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
    public List<TurtleUpgrade> items = new();
    public TurtleInventory inventory = new();
}


public class UIConverters
{
    [UnityEditor.InitializeOnLoadMethod]
    public static void InitConverters()
    {
        ConverterGroup g = new("shop");

        g.AddConverter((ref int level) => TurtleUpgrade.LevelToString(level));
        g.AddConverter((ref UpgradeType type) => TurtleUpgrade.TypeToString(type));

        ConverterGroups.RegisterConverterGroup(g);
    }
}