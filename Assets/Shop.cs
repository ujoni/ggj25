using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

public class Shop : MonoBehaviour
{

    private static readonly List<UpgradeType> UPGRADE_TYPES = new() { UpgradeType.CarryingCapacity, UpgradeType.OxygenMaximum, UpgradeType.Speed, UpgradeType.Toughness };
    public VisualTreeAsset listItem;
    // override value when testing the scene to get different depths
    // real runs yoink the value from the sukeltajascript
    public float currentDepth;
    public ShopData dShop = new();
    private VisualElement root;
    private VisualElement[] inventorySlots = new VisualElement[13];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var sukeltaja = FindFirstObjectByType<sukeltajascript>();

        if (sukeltaja == null)
        {
            var td = new TurtleData();
            td.shells = 100;
            Debug.Log("Using test turtle data. Shells: " + td.shells);
            PopulateShop(td);
        }
    }

    public void PopulateShop(TurtleData turtleData)
    {
        dShop.shells = turtleData.shells;
        dShop.inventory = turtleData.inventory.GetCopy();
        currentDepth = turtleData.depth; // used for item gen

        var uiDocument = GetComponent<UIDocument>();
        uiDocument.enabled = true;

        root = uiDocument.rootVisualElement.Q<VisualElement>("Root");
        var itemList = root.Q<ListView>("ItemList");
        itemList.SetBinding("itemsSource", new DataBinding() { dataSourcePath = new PropertyPath("items") });
        var slots = root.Q<VisualElement>("Inventory").Children();
        foreach (var slot in slots)
        {
            inventorySlots[slot.tabIndex] = slot;
            slot.dataSource = dShop.inventory.upgrades[slot.tabIndex];
            var remove = slot.Q<Button>("Remove");
            remove.clicked += () =>
            {
                dShop.inventory.ClearSlot(slot.tabIndex);
                dShop.shells += 1;
            };
        }


        // TODO: some depth-specific shop population (2 to 5 items?)
        for (int i = 0; i < 5; i++)
        {
            TurtleUpgrade u = new()
            {
                level = i + 1,
                type = RandomSelect(UPGRADE_TYPES),
            };

            var item = listItem.Instantiate();
            var buy = item.Q<Button>();
            buy.clicked += () =>
            {
                Debug.Log("Purchasing " + u.type + ", shells " + dShop.shells + ", price " + u.price);
                if (dShop.shells < u.price) return; // TODO: signal user that they poor
                Debug.Log("Got money");
                // perform purchase
                if (dShop.inventory.AddUpgrade(u))
                {
                    Debug.Log("Added upgrade");
                    dShop.shells -= u.price;
                    itemList.hierarchy.Remove(item);
                }
            };
            itemList.hierarchy.Add(item);
            item.dataSource = u;
        }

        root.dataSource = dShop;
    }

    public void CloseShop()
    {
        var sukeltaja = FindFirstObjectByType<sukeltajascript>();
        if (sukeltaja != null)
        {
            // update turtles possessions
            sukeltaja.dTurtle.inventory = dShop.inventory.GetCopy();
            sukeltaja.dTurtle.shells = dShop.shells;
        }
        var uiDocument = GetComponent<UIDocument>();
        uiDocument.enabled = false;
    }

    private static T RandomSelect<T>(List<T> value)
    {
        var i = UnityEngine.Random.Range(0, value.Count - 1);
        return value[i];
    }

}
