using System;
using System.Collections.Generic;
using System.Linq;
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
        texturePerType.Add(UpgradeType.CarryingCapacity, Resources.Load<Sprite>("upgrade_carry"));
        texturePerType.Add(UpgradeType.OxygenMaximum, Resources.Load<Sprite>("upgrade_happi"));
        texturePerType.Add(UpgradeType.Speed, Resources.Load<Sprite>("upgrade_speed"));
        texturePerType.Add(UpgradeType.Toughness, Resources.Load<Sprite>("upgrade_power"));
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

    public int ClearSlot(int slot)
    {
        int shells = upgrades[slot].price;
        upgrades[slot].Reset();
        return shells;
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

    public int shells = 0;
    public float depth = 0;

    private float _oxy = 20;
    [SerializeField]
    public float oxygen
    {
        get { return _oxy; }
        set { _oxy = Math.Min(maxOxygen, Math.Max(0, value)); }
    }
    [SerializeField]
    public int maxOxygen
    {
        get { return 20 + inventory.GetTotalLevels(UpgradeType.OxygenMaximum) * 10; }
    }
    [SerializeField]
    public int carryingCapacity
    {
        get { return 20 + inventory.GetTotalLevels(UpgradeType.CarryingCapacity) * 10; }
    }
    [SerializeField]
    public int maxSpeed
    {
        get { return 20 + inventory.GetTotalLevels(UpgradeType.Speed) * 10; }
    }
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

[Serializable]
public class UIState
{
    public bool isBarVisible = true;

    [SerializeField]
    public StyleEnum<DisplayStyle> barDisplayStyle
    {
        get { return isBarVisible ? DisplayStyle.Flex : DisplayStyle.None; }
    }

    [SerializeField]
    public bool isShopVisible
    {
        get { return !isBarVisible; }
    }
}


