using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Shop : MonoBehaviour
{

    private ShopData sData = new ShopData();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<UIDocument>().rootVisualElement.dataSource = sData;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
