using UnityEngine;
//using UnityEngine.InputSystem;

public class CreateBubblesScript : MonoBehaviour
{

    public GameObject bubble;
    GameObject turtle;

    int moodi;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        turtle = GameObject.Find("Sukeltaja");
        moodi = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, turtle.transform.position) > 30) return;
        if(Random.Range(0, 100) == 0) {
            moodi = Random.Range(0, 2);
            if (Random.Range(0,5) == 0){
                moodi = 2;
            }
        }
        if (moodi == 1){
            if(Random.Range(0, 100) == 0) {
                int amt = Random.Range(1, 5);
                for (int i = 0; i < amt; i++){
                    Bubb();
                }
            }
        }
        if (moodi == 2){
            if(Random.Range(0, 5) == 0) {
                Bubb();
            }
        }
    }

    void Bubb(){
        GameObject go = GameObject.Instantiate(bubble);
        go.transform.position = transform.position; 
        float r = Random.Range(1f, 2f);
        go.transform.localScale = new Vector3(r*go.transform.localScale.x,
            r*go.transform.localScale.y, 1);
        Destroy(go, 10 + 20*Random.Range(0f, 1f));
    }
}


