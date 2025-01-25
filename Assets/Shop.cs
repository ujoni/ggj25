using System.Linq;
using NUnit.Framework;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

public class Shop : MonoBehaviour
{
    public VisualTreeAsset listItem;
    public ShopData dShop = new();
    public TurtleData turtleDataRef = new();
    private VisualElement root;
    private VisualElement[] inventorySlots = new VisualElement[13];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var sukeltaja = FindFirstObjectByType<sukeltajascript>();

        if (sukeltaja != null)
        {
            Debug.Log("Using real turtle data");
            turtleDataRef = sukeltaja.dTurtle;
        }
        else
        {
            Debug.Log("Using test turtle data");
        }

        root = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Root");
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
            };
        }



        // TODO: some depth-specific shop population (2 to 5 items?)
        for (int i = 0; i < 5; i++)
        {
            TurtleUpgrade u = new()
            {
                level = i + 1,
                type = UpgradeType.Speed
            };

            var item = listItem.Instantiate();
            var buy = item.Q<Button>();
            buy.clicked += () =>
            {
                if (dShop.inventory.AddUpgrade(u)) itemList.hierarchy.Remove(item);
            };
            itemList.hierarchy.Add(item);
            item.dataSource = u;
        }

    }
}
