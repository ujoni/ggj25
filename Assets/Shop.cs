using NUnit.Framework;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

public class Shop : MonoBehaviour
{
    public VisualTreeAsset listItem;
    public ShopData dShop = new();
    private VisualElement root;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Root");
        var itemList = root.Q<ListView>("ItemList");
        itemList.SetBinding("itemsSource", new DataBinding() { dataSourcePath = new PropertyPath("items") });

        for (int i = 0; i < 2; i++)
        {
            TurtleUpgrade u = new()
            {
                level = 1,
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

    // Update is called once per frame
    void Update()
    {

    }
}
