using UnityEngine;

public class RemoveExcessives : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (GameObject sub in GameObject.FindGameObjectsWithTag("Submarine")) {
            //print("sub");
            if (sub != gameObject && Vector3.Distance(sub.transform.position, gameObject.transform.position) < 35) {
                if (sub.transform.position.y < gameObject.transform.position.y) {
                    Destroy(gameObject);
                }
            }
        }
    }
}
