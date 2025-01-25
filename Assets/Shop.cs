using System.Linq;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Shop : MonoBehaviour
{

    private ShopData dShop = new();
    private VisualElement root;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement.Children().First();

        for (int i = 0; i < 2; i++)
        {
            TurtleUpgrade u = new()
            {
                level = 1,
                type = UpgradeType.Speed
            };

            dShop.items.Add(u);
        }
        root.dataSource = dShop.items;

        Debug.Log(root.name);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
