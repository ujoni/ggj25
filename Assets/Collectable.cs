using System.Collections;
using System.Runtime.Serialization.Formatters;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collectable : MonoBehaviour
{
    private bool collected = false;
    public float timeout;

    public CollectableData cData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (Time.time < timeout) return;
        //print("kekk" + collider.GetContact(0).collider.gameObject.name);
        if (!collected && (collider.GetContact(0).collider.gameObject.name == "turtlehead" ||
            collider.gameObject.name == "Sukeltaja" &&
            collider.gameObject.GetComponent<sukeltajascript>().bodycollect))//TryGetComponent<sukeltajascript>(out var turtle))
        {
            collected = true;
            // GetComponent<SpriteRenderer>().color = Color.red;
            collider.gameObject.GetComponent<sukeltajascript>().Collect(cData);
            if (GetComponent<AudioSource>())
            {
                GameObject tempAudio = new GameObject("TempAudio");
                //tempAudio.transform.position = position;

                AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
                audioSource.clip = GetComponent<AudioSource>().clip;
                audioSource.Play();

                // Destroy the temporary GameObject after the clip has finished playing
                Destroy(tempAudio, audioSource.clip.length);
                StartCoroutine(LoadEndScene(audioSource.clip.length + 0.5f));
            }
            Destroy(gameObject);

        }
    }

    private IEnumerator LoadEndScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("EndScene");
    }
}

