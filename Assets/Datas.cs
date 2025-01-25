using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
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

    private static readonly Dictionary<UpgradeType, Sprite> texturePerType = new();

    public int level = 0;
    public UpgradeType type = UpgradeType.None;

    [SerializeField]
    public Sprite icon
    {
        get
        {
            return texturePerType[type];
        }
    }

    [SerializeField]
    public float price
    {
        get { return level * pricePerType[type]; }
    }

    public static string LevelToString(int level)
    {
        return "lvl " + level.ToString();
    }

    public static string TypeToString(UpgradeType type)
    {
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

    [UnityEngine.RuntimeInitializeOnLoadMethod]
    public static void LoadIcons()
    {
        Debug.Log("Loading icons like a baus");
        var asset = Resources.Load<Sprite>("ball");
        Debug.Log("Loaded asset " + asset);
        texturePerType.Add(UpgradeType.CarryingCapacity, asset);
        texturePerType.Add(UpgradeType.OxygenMaximum, asset);
        texturePerType.Add(UpgradeType.Speed, asset);
        texturePerType.Add(UpgradeType.Toughness, asset);
    }
}


[Serializable]
public class TurtleInventory
{
    public TurtleUpgrade[] upgrades = new TurtleUpgrade[13];

    public bool AddUpgrade(TurtleUpgrade u)
    {
        var slot = NextSlot();
        if (slot == -1)
        {
            Debug.Log("AddUpgrade: did not have space");
            return false;
        }
        Debug.Log("AddUpgrade: slot is " + slot);
        upgrades[slot] = u;
        return true;
    }


    private int NextSlot()
    {
        return upgrades.ToList().FindIndex(u => u.type == UpgradeType.None);
    }
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