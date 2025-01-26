using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuScript : MonoBehaviour
{
    private bool startClicked = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<UIDocument>().rootVisualElement.Q<Button>("Start").clicked += () =>
        {
            if (startClicked) return;
            startClicked = true;
            SceneManager.LoadSceneAsync("SampleScene");
        };

        GetComponent<UIDocument>().rootVisualElement.Q<Button>("Exit").clicked += () =>
        {
            Application.Quit();
        };
    }

}
