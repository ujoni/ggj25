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

                if (name == "Helmi(Clone)") SceneManager.LoadScene("EndScene");
                else
                {
                    GameObject tempAudio = new GameObject("TempAudio");
                    //tempAudio.transform.position = position;

                    AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
                    audioSource.clip = GetComponent<AudioSource>().clip;
                    audioSource.Play();
                    // Destroy the temporary GameObject after the clip has finished playing
                    Destroy(tempAudio, audioSource.clip.length);
                }
            }
            Destroy(gameObject);

        }
    }

}

