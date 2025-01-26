using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEditor.SearchService;
using UnityEngine;
using V2 = UnityEngine.Vector2;
using V3 = UnityEngine.Vector3;

public class MinimapScript : MonoBehaviour
{
    public GameObject block;

    // discrete pos of player
    V2 pos;
    CreateWorld cw;
    GameObject player;

    GameObject[,] grid;
    int left; 
    int top; 
    int width; 
    int height; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        left = -7;
        width = 15;
        top = -7;
        height = 15;

        grid = new GameObject[width, height];

        for (int ix = 0; ix < width; ix ++) {
            for (int iy = 0; iy < height; iy ++) {
                grid[ix, iy] = GameObject.Instantiate(block);
                grid[ix, iy].transform.SetParent(GameObject.Find("Canvas").transform, true);
                grid[ix, iy].transform.position = new V3(1500 + ix*18, 600 + iy*18, 0);
                
                //grid[ix, iy].GetComponent<UnityEngine.UI.Image>().enabled = false;
            }
        }

        pos = new V2(GameObject.Find("WorldCreator").GetComponent<CreateWorld>().startx,
        GameObject.Find("WorldCreator").GetComponent<CreateWorld>().starty);
        player = GameObject.Find("Sukeltaja");
        cw = GameObject.Find("WorldCreator").GetComponent<CreateWorld>();
    }

    
    void Update()
    {
        // update the logical position of player at all times
        V3 playerpos = player.transform.position;
        float mind = 100000;
        V2 newpos  = V2.zero;
        for (int ix = -1; ix < 2; ix ++){
            for (int iy = -1; iy < 2; iy ++){
                V3 physpos = cw.CornerPoint(pos.x + ix, pos.y + iy);
                float d = V3.Distance(physpos, playerpos);
                if (d < mind){
                    mind = d;
                    newpos = new V2(pos.x + ix, pos.y + iy);
                }
            }
        }
        pos = newpos;

        for (int ix = 0; ix < width; ix++){
            for (int iy = 0; iy < height; iy++){
                int val = cw.GetGrid(cw.grid, left + ix + (int)pos.x, top + iy + (int)pos.y);
                if (val == 0) grid[ix, iy].GetComponent<UnityEngine.UI.Image>().color =
                    new Color(0.6f,0.6f,1f);
                if (val == 1) grid[ix, iy].GetComponent<UnityEngine.UI.Image>().color =
                    new Color(0.7f,0.6f,0.4f);
            }
        }

        Collider2D[] colls = Physics2D.OverlapBoxAll(playerpos, new V2(100, 100), 0);
        //print(colls.Length)
        //print(colls.Length);
        HashSet<GameObject> collectables = new HashSet<GameObject>();
        HashSet<GameObject> saukkos = new HashSet<GameObject>();
        HashSet<GameObject> creatures = new HashSet<GameObject>();
        HashSet<GameObject> plants = new HashSet<GameObject>();
        foreach (Collider2D coll in colls) {
            if (coll.GetComponent<Collectable>()){
                //print("yes");
                if (coll.GetComponent<Collectable>().cData.collectableType == CollectableType.BigShell ||
                    coll.GetComponent<Collectable>().cData.collectableType == CollectableType.NormalShell||
                    coll.GetComponent<Collectable>().cData.collectableType == CollectableType.RainbowShell){
                    collectables.Add(coll.gameObject);
                }
            }

            if (coll.gameObject.GetComponent<SaukkoScript>() != null){
                //print("Yes");
                saukkos.Add(coll.gameObject);
            }
            if (coll.gameObject.GetComponent<CreatureScript>() != null){
                //print("Yes");
                creatures.Add(coll.gameObject);
            }

            if (coll.gameObject.GetComponent<CreateBubblesScript>() != null){
                //print("Yes");
                plants.Add(coll.gameObject);
            }

        }

        List<GameObject> dems = collectables.ToList<GameObject>();
        dems.AddRange(saukkos.ToList<GameObject>());
        dems.AddRange(creatures.ToList<GameObject>());
        dems.AddRange(plants.ToList<GameObject>());

        //print(saukkos.Count);
                   

        foreach (GameObject obj in dems ) {
            int closestx = 1000;
            int closesty = 1000;
            float mindo = 10000;

            for (int ix = -1; ix < width+1; ix++){
                for (int iy = -1; iy < height+1; iy++){
                    if (cw.GetGrid(cw.grid, (int)pos.x+left+ix, (int)pos.y+top+iy) != 0) continue;
                    float d = V3.Distance(obj.transform.position,
                        cw.CornerPoint((int)pos.x+left+ix, (int)pos.y+top+iy));
                    if (d < mindo) {
                        mindo = d;
                        closestx = ix;
                        closesty = iy;
                    }
                }
            }
            if (closestx >= 0 && closestx < width && closesty >= 0 && closesty < height){
                if (saukkos.Contains(obj))
                    grid[closestx, closesty].GetComponent<UnityEngine.UI.Image>().color = Color.blue;
                else if (collectables.Contains(obj))
                    grid[closestx, closesty].GetComponent<UnityEngine.UI.Image>().color = Color.green;
                else if (creatures.Contains(obj))
                    grid[closestx, closesty].GetComponent<UnityEngine.UI.Image>().color = Color.red;
                else if (plants.Contains(obj))
                    grid[closestx, closesty].GetComponent<UnityEngine.UI.Image>().color = Color.white;
            }
        }

        grid[-left, -top].GetComponent<UnityEngine.UI.Image>().color = Color.yellow;
        
    }
}
