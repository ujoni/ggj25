using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Unity.Multiplayer.Center.Common;
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
    private static readonly Dictionary<UpgradeType, int> pricePerType = new()
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
    public int price
    {
        get { return level * pricePerType[type]; }
    }

    [SerializeField]
    public StyleEnum<Visibility> visible
    {
        get { return type != UpgradeType.None ? new(Visibility.Visible) : new(Visibility.Hidden); }
    }

    public void Reset()
    {
        level = 0;
        type = UpgradeType.None;
    }

    public void CopyTo(ref TurtleUpgrade another)
    {
        another.level = level;
        another.type = type;
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
        texturePerType.Add(UpgradeType.CarryingCapacity, asset);
        texturePerType.Add(UpgradeType.OxygenMaximum, asset);
        texturePerType.Add(UpgradeType.Speed, asset);
        texturePerType.Add(UpgradeType.Toughness, asset);
        texturePerType.Add(UpgradeType.None, null);
    }
}


[Serializable]
public class TurtleInventory
{
    public TurtleUpgrade[] upgrades = new TurtleUpgrade[13]{
        new(), new(), new(), new(), new(), new(), new(), new(), new(), new(), new(), new(), new(),
    };

    public bool AddUpgrade(TurtleUpgrade u)
    {
        var slot = NextSlot();
        if (slot == -1)
        {
            Debug.Log("AddUpgrade: did not have space");
            return false;
        }
        Debug.Log("AddUpgrade: slot is " + slot);
        // copy instead of set - allows our "pretty" UI to catch the value changes
        var cur = upgrades[slot];
        cur.type = u.type;
        cur.level = u.level;
        return true;
    }

    public void ClearSlot(int slot)
    {
        upgrades[slot].Reset();
    }

    public int GetTotalLevels(UpgradeType forType)
    {
        return upgrades.Where(u => u.type == forType).Aggregate(0, (acc, u) => acc + u.level);
    }

    public TurtleInventory GetCopy()
    {
        TurtleInventory n = new();
        for (int i = 0; i < upgrades.Count(); i++)
        {
            if (upgrades[i] != null) upgrades[i].CopyTo(ref n.upgrades[i]);
            else n.upgrades[i] = new();
        }
        return n;
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
    public int shells = 0;
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