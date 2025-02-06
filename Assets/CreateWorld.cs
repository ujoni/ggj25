using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using V2 = UnityEngine.Vector2;
using V3 = UnityEngine.Vector3;
using UnityEngine.UIElements;
using UnityEditor.SearchService;
using UnityEngine.PlayerLoop;
using UnityEngine.TerrainUtils;
using NUnit.Framework;


public class CreateWorld : MonoBehaviour
{

    public Material maamat;
    public Material boundarymat;
    public GameObject sukeltaja;
    public GameObject[] plants;
    public GameObject helmisimpukka;
    Dictionary<V2, GameObject> terrainMap;
    public GameObject saukko;

    public GameObject[] goodies;
    public float[] depthmins;
    public float[] depthmaxes;
    public int[] goodieamounts;
    public int[] bunches;

    /*public GameObject[] enemies;
    public float[] enemydepthmins;
    public float[] enemydepthmaxes;*/

    int c;
    int generationSize;
    V2 lastensured;

    List<V2> safes;
    List<V2> safenbrs;

    // minimap is used to get the abstract position of player
    MinimapScript minimap;

    public int[,] grid;
    public int startx;
    public int starty;
    int SIZEX = 150;
    int SIZEY = 200;

    List<V2> holes;
    Dictionary<float, List<V2>> holesByHeight;

    List<GameObject> WorldObjects;

    bool loadgoodiesfromfile = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     
        /*for (int i = 0; i < 50; i++){
            UnityEngine.Debug.Log(Helpers.RandomWithSlope(0, 5, -1));
            print(Helpers.c);
        }*/


        startx = (int)(SIZEX/2);
        starty = SIZEY - 1;

        lastensured = new V2(10000, 0);
        c = 0;
        terrainMap = new Dictionary<V2, GameObject>();
        // how far do we generate terrain? this must be big enough so active enemies don't fly outside
        generationSize = 60; //14;
        minimap = GameObject.Find("MiniMap").GetComponent<MinimapScript>();
        float timme = Time.realtimeSinceStartup;
        WorldObjects = new List<GameObject>();
        Random.InitState(12);


        


        safes = new List<V2>();
        safenbrs = new List<V2>();
        holes = new List<V2>();
        holesByHeight = new Dictionary<float, List<V2>>();

        grid = new int[SIZEX, SIZEY];
        for (int ix = 0; ix < SIZEX; ix++)
        {
            for (int iy = 0; iy < SIZEY; iy++)
            {
                grid[ix, iy] = 1; // basic nonempty
            }
        }
        sukeltaja.transform.position = CornerPoint(startx, starty);
        grid[startx, starty] = 0;
        grid[startx, starty - 1] = 0;
        grid[startx - 1, starty - 1] = 0;
        grid[startx + 1, starty - 1] = 0;
        grid[startx, starty - 2] = 0;
        grid[startx - 1, starty - 2] = 0;
        grid[startx + 1, starty - 2] = 0;

        // holes.Add(new V2(startx, starty)); // NOPE
        //SetGrid(grid, startx, starty, 0);

        print("zeroing: " + (Time.realtimeSinceStartup - timme));
        timme = Time.realtimeSinceStartup;

        int mains = 3; //Random.Range(1, 3);
        for (int i = 0; i < mains; i++)
        {
            V2 v = new V2(Random.Range(SIZEX / 5, 4 * SIZEX / 5), 1);
            SubdivideThingo(new V2(startx, starty), v);
            if (i == 0)
            {
                GameObject helmi = GameObject.Instantiate(helmisimpukka);
                helmi.transform.position = CornerPoint(v.x, v.y);
                WorldObjects.Add(helmi);
            }
        }

        print("mains: " + (Time.realtimeSinceStartup - timme));
        timme = Time.realtimeSinceStartup;

        for (int i = 0; i < 20; i++)
        {
            CreateRandomSturmPath(Random.Range(15, 40));
            CreateRandomPath();
        }

        print("sturm paths: " + (Time.realtimeSinceStartup - timme));
        timme = Time.realtimeSinceStartup;

        for (int i = 0; i < 20; i++)
        {
            CreateRandomSturmPath(Random.Range(5, 15));
            CreateRandomPath();
        }

        print("short sturm paths: " + (Time.realtimeSinceStartup - timme));
        timme = Time.realtimeSinceStartup;

        /*for (int i = 0; i < 10; i++){
            
        }*/

        MakeGoodies(false);

        print("goodies: " + (Time.realtimeSinceStartup - timme));
        timme = Time.realtimeSinceStartup;

        //grid[startx+1, starty-1] = 0;
        for (int i = 0; i < 20; i++)
        {
            CreateRandomBlob();
        }

        print("blobs: " + (Time.realtimeSinceStartup - timme));
        timme = Time.realtimeSinceStartup;

        // make big goodies
        MakeGoodies(true);

        print("big goodies: " + (Time.realtimeSinceStartup - timme));
        timme = Time.realtimeSinceStartup;

        for (int i = 0; i < 120; i++)
        {
            MakePlant();
        }

        print("plants: " + (Time.realtimeSinceStartup - timme));
        timme = Time.realtimeSinceStartup;

        RemoveExcessTerrain();

        print("zeroing: " + (Time.realtimeSinceStartup - timme));
        timme = Time.realtimeSinceStartup;

        //CreateTerrain();
        print("creating terrain: " + (Time.realtimeSinceStartup - timme));
        timme = Time.realtimeSinceStartup;

        /*for (int g = 0; g < enemies.Length; g++){
            for (int i = 0; i < 50; i++){
                MakeEnemy(g);
            }
        }*/

        GameObject go = GameObject.Instantiate(saukko);
        WorldObjects.Add(go);
        go.transform.position = CornerPoint(startx, starty - 1.5f);

        GameObject.Find("Enabler").GetComponent<EnablerScript>().objects = WorldObjects;
        GameObject.Find("Enabler").GetComponent<EnablerScript>().Initialize();

        EnsureTerrainNear(startx, starty, 0);

        //GameObject[] allObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        //print(allObjects.Length);

    }

    void MakeGoodies(bool bigs){
        if (loadgoodiesfromfile) {
            string p = Application.persistentDataPath;
            LevelGenerationDescription l = LevelGenerationDescription.LoadData(p + "/test.lgd");
            print("Loading level with description " + l.description);
            for (int g = 0; g < l.objects.Count; g++) {
                print("LevelObjects/" + l.objects[g].objectName);
                GameObject goodie = (GameObject)Resources.Load("LevelObjects/" + l.objects[g].objectName);
                print(goodie);
                if (bigs ^ (goodie.GetComponent<BigScript>() != null || goodie.GetComponent<PlantScript>() != null)) continue;
                int amount = Random.Range(l.objects[g].minAmount, l.objects[g].maxAmount+1);
                print(amount);
                for (int i = 0; i < amount; i++) {
                    MakeGoodie(goodie, l.objects[g]);
                }
            }
        }
        else
        {
            for (int g = 0; g < goodies.Length; g++)
            {
                if (bigs ^ (goodies[g].GetComponent<BigScript>() != null)) continue;
                for (int i = 0; i < goodieamounts[g]; i++)
                {
                    MakeGoodie(g);
                }
            }
        }
    }

    void FixedUpdate() {
        c++;
        // if (c % 50 != 0) return;
        // should check that minimap actually thinks its position is correct...
        // or alternatively we could make minimap jump right to correct position
        EnsureTerrainNear((int)minimap.pos.x, (int)minimap.pos.y, 10);
    }

    void RemoveExcessTerrain()
    {
        List<V2> allfulls = new List<V2>();
        for (int ix = 0; ix < SIZEX; ix++)
        {
            for (int iy = 0; iy < SIZEY; iy++)
            {
                bool safe = true;
                if (ix == 0 || ix == SIZEX-1 || iy == 0 || iy == SIZEY-1) safe = false;
                bool allfull = true;
                for (int xx = -1; xx <= 1; xx++)
                {
                    for (int yy = -1; yy <= 1; yy++)
                    {
                        
                        if (GetGrid(grid, ix + xx, iy + yy, safe) == 0)
                        {
                            allfull = false;
                            break;
                        }
                    }
                }
                if (allfull) allfulls.Add(new V2(ix, iy));
            }
        }
        foreach (V2 v in allfulls)
        {
            SetGrid(grid, v.x, v.y, 0, true, false);
        }
    }

    void MakeGoodie(int g)
    {
        V2 hole = holes[Random.Range(0, holes.Count)];
        while (SIZEY - hole.y < depthmins[g] || SIZEY - hole.y > depthmaxes[g] ||
            (goodies[g].GetComponent<BigScript>() != null &&
            !BigEnoughHole(hole.x, hole.y)))
        {
            hole = holes[Random.Range(0, holes.Count)];
        }
        int amt = Random.Range(1, bunches[g] + 1);
        for (int t = 0; t < amt; t++)
        {
            GameObject good = GameObject.Instantiate(goodies[g]);
            WorldObjects.Add(good);
            good.transform.position = CornerPoint(hole.x + Random.Range(-0.5f, 0.5f),
            hole.y + Random.Range(-0.5f, 0.5f));
        }
    }

    void MakeGoodie(GameObject g, ObjectGenerationDescription ogd)
    {
        print("making goodies");
        print(ogd.objectName);
        print(ogd.mindepth);
        print(ogd.maxdepth);
        print(ogd.depthslope);
        int depth = (int)(SIZEY - 1 -
        Helpers.RandomWithSlope(ogd.mindepth, ogd.maxdepth, ogd.depthslope) * (SIZEY - 1));
        print(depth);

        if (!holesByHeight.ContainsKey(depth))
        {
            print("Failed to instantiate " + ogd.objectName + " at height " + depth + ".");
            return;
        }
        bool isplant = g.GetComponent<PlantScript>() != null;
        V2 pos = Helpers.RandomChoice(holesByHeight[depth]);
        int amt = Random.Range(ogd.minBunch, ogd.maxBunch + 1);
        for (int t = 0; t < amt; t++)
        {
            GameObject good = GameObject.Instantiate(g);
            WorldObjects.Add(good);
            if (isplant)
            {
                print("is plant");
                if (ogd.minBunch != 1 || ogd.maxBunch != 1) print("Plant bunches don't work.");
                V2 n = nbrof(pos, pos + V2.up);
                print(n);
                /*if (GetGrid(grid, n.x, n.y) != 1) {
                    MakeGoodie(g, ogd);
                    return;
                }*/
                while (GetGrid(grid, n.x, n.y) != 1)
                {
                    print("retry");
                    pos = Helpers.RandomChoice(holesByHeight[depth]);
                    n = nbrof(pos, pos + V2.up);
                }
                V2 v = (pos + n) / 2 + Random.Range(-0.45f, 0.45f) * Helpers.TurnLeft(n - pos);
                good.transform.position = CornerPoint(v.x, v.y);
                if (n.x > pos.x)
                {
                    good.transform.rotation = UnityEngine.Quaternion.Euler(0, 0, 80);
                }
                else if (n.x < pos.x)
                {
                    good.transform.rotation = UnityEngine.Quaternion.Euler(0, 0, -80);
                }
            }
            else
            {
                good.transform.position = CornerPoint(pos.x + Random.Range(-0.45f, 0.45f),
                    pos.y + Random.Range(-0.45f, 0.45f));
            }
        }

        /*V2 hole = holes[Random.Range(0, holes.Count)];
        while (SIZEY - hole.y < depthmins[g] * SIZEY || SIZEY - hole.y > depthmaxes[g] ||
            (goodies[g].GetComponent<BigScript>() != null &&
            !BigEnoughHole(hole.x, hole.y)))
        {
            hole = holes[Random.Range(0, holes.Count)];
        }
        int amt = Random.Range(1, bunches[g] + 1);
        for (int t = 0; t < amt; t++)
        {
            GameObject good = GameObject.Instantiate(goodies[g]);
            WorldObjects.Add(good);
            good.transform.position = CornerPoint(hole.x + Random.Range(-0.5f, 0.5f),
            hole.y + Random.Range(-0.5f, 0.5f));
        }*/

    }

    // big enough hole for a big object?
    bool BigEnoughHole(float x, float y)
    {
        int left = 1;
        int right = 1;
        int up = 1;
        int down = 0;

        int xx = (int)x;
        int yy = (int)y;
        for (int xxx = xx - left; xxx <= xx + right + 1; xxx++)
        {
            for (int yyy = yy - down; yyy <= yy + up + 1; yyy++)
            {
                if (GetGrid(grid, xxx, yyy) != 0) return false;
            }
        }
        return true;
        /*for (int xxx = xx - XX + 1; xxx < xx + 1; xxx ++){
            for (int yyy = yy - YY + 1; yyy < yy + 1; yyy ++){
                bool allok = true;
                for (int xxxx = xxx; xxxx < xxx + XX; xxxx ++){
                    for (int yyyy = yyy; yyyy < yyy + YY; yyyy ++){
                        if (GetGrid(grid, xxxx, yyyy) != 0) {
                             allok = false;
                        }
                    }
                }
                if (allok) return true;
            }
        }*/
    }

    /*void MakeEnemy(int g){

        V2 hole = holes[Random.Range(0, holes.Count)];
        while (SIZEY - hole.y < enemydepthmins[g] || SIZEY - hole.y > enemydepthmaxes[g])
            hole = holes[Random.Range(0, holes.Count)];
        GameObject good = GameObject.Instantiate(enemies[g]);
        good.transform.position = CornerPoint(hole.x + Random.Range(-0.5f,0.5f),
            hole.y + Random.Range(-0.5f,0.5f));
    }*/

    void MakePlant()
    {
        if (loadgoodiesfromfile) return;
        V2 hole = holes[Random.Range(0, holes.Count)];
        V2 n = nbrof(hole, hole + V2.up);
        float kuplamin = SIZEY * Random.Range(0f, 1f);
        while (hole.y < kuplamin || GetGrid(grid, n.x, n.y) != 1)
        {
            hole = holes[Random.Range(0, holes.Count)];
            n = nbrof(hole, hole + V2.up);
            kuplamin = SIZEY * Random.Range(0f, 1f);
        }
        // print(GetGrid(grid, hole.x, hole.y) == 0);
        safes.Add(hole);
        safenbrs.Add(n);

        GameObject p = GameObject.Instantiate(plants[Random.Range(0, plants.Length)]);
        //print("gimmel" + Helpers.TurnLeft(n - hole));
        V2 v = (hole + n) / 2 + Random.Range(-0.45f, 0.45f) * Helpers.TurnLeft(n - hole);
        //print(v);
        //print(v);
        p.transform.position = CornerPoint(v.x, v.y);
        if (n.x > hole.x)
        {
            p.transform.rotation = UnityEngine.Quaternion.Euler(0, 0, 80);
        }
        else if (n.x < hole.x)
        {
            p.transform.rotation = UnityEngine.Quaternion.Euler(0, 0, -80);
        }
        WorldObjects.Add(p);

    }

    // from int point to actual world point
    public V3 CornerPoint(float x, float y)
    {
        float c = Mathf.Min(1, (SIZEY - y) / 10);
        float d = 0.9f;

        return new V3(x * 10 + Mathf.PerlinNoise(y * 4, x * 4 + 120) * 2 * d +
            (Mathf.PerlinNoise(y / 8, x / 8 + 150) * 7f + Mathf.PerlinNoise(y / 20, x / 20 + 120) * 20f) * c,

            y * 10 + Mathf.PerlinNoise(x * 4, y * 4) * 2 * d +
            (Mathf.PerlinNoise(y / 8, x / 8) * 7f + Mathf.PerlinNoise(y / 20, x / 20 + 16) * 20) * c,

            0);
    }

    // slightly diff
    V3 CornerPointUV(float x, float y)
    {
        float c = Mathf.Min(1, (SIZEY - y) / 10) * 1.1f;
        float d = 0.9f;

        return new V3(x * 10 + Mathf.PerlinNoise(y * 4, x * 4 + 120) * 2 * d +
            (Mathf.PerlinNoise(y / 6, x / 6 + 150) * 7.5f + Mathf.PerlinNoise(y / 16, x / 16 + 120) * 27f) * c,

            y * 10 + Mathf.PerlinNoise(x * 4, y * 4) * 2 * d +
            (Mathf.PerlinNoise(y / 6, x / 6) * 7.5f + Mathf.PerlinNoise(y / 16 + 1, x / 16 + 16) * 27) * c,

            0);
    }

    void CreateRandomSturmPath(int le)
    {
        var curr = holes[Random.Range(0, holes.Count)];
        var offs = Random.insideUnitCircle * le;
        //offs += new V2(0, -Random.Range(0, 10));
        V2 end = new V2((int)(curr.x + offs.x), (int)(curr.y + offs.y));
        end = intogrid(end);
        SubdivideThingo(curr, end);
    }

    V2 intogrid(V2 v)
    {
        if (v.y > SIZEY - 2) v = new V2(v.x, SIZEY - 2);
        if (v.y < 1) v = new V2(v.x, 1);
        if (v.x > SIZEX - 2) v = new V2(SIZEX - 2, v.y);
        if (v.x < 1) v = new V2(1, v.y);
        return v;
    }
    void SubdivideThingo(V2 a, V2 b)
    {
        a = intogrid(a);
        b = intogrid(b);
        if (V2.Distance(a, b) < 1)
        {
            a = round(a);
            b = round(b);
            SetGrid(grid, a.x, a.y, 0);
            //if (!holes.Contains(a)) holes.Add(a);
            SetGrid(grid, b.x, b.y, 0);
            //if (!holes.Contains(b)) holes.Add(b);
            return;
        }
        float disto = V2.Distance(a, b);
        float multo = disto / 2;
        if (disto < 4) multo = disto / 8;
        multo = Mathf.Min(multo, 13);
        V2 c = (a + b) / 2 + Random.insideUnitCircle * multo;
        c = intogrid(c);
        SubdivideThingo(a, c);
        SubdivideThingo(c, b);
    }

    V2 round(V2 input)
    {
        return new V2(Mathf.Round(input.x), Mathf.Round(input.y));
    }

    void CreateRandomPath()
    {
        V2 curr = V2.zero;
        V2 prev;
        while (true)
        {
            prev = holes[Random.Range(0, holes.Count)];
            curr = nbrof(prev, prev);
            if (GetGrid(grid, curr.x, curr.y) != 0) break;
        }
        //curr = nbrof(curr, prev);

        int len = Random.Range(5, 25);
        for (int i = 0; i < len; i++)
        {

            //if (grid[(int)curr.x, (int)curr.y] != 0) holes.Add(curr);
            //grid[(int)curr.x, (int)curr.y] = 0;
            SetGrid(grid, curr.x, curr.y, 0);

            curr = nbrof(curr, prev);
            prev = curr;
        }
    }

    void CreateRandomBlob()
    {
        var curr = holes[Random.Range(0, holes.Count)];
        float rad = Random.Range(2f, 4f);
        for (int ix = -10; ix <= 10; ix++)
        {
            for (int iy = -10; iy <= 10; iy++)
            {
                if (new V2(ix, iy).magnitude + Mathf.PerlinNoise(ix, iy) * 0.3f < rad)
                {
                    SetGrid(grid, curr.x + ix, curr.y + iy, 0);
                    AddHole(curr + new V2(ix, iy));
                    
                }
            }
        }
    }

    void AddHole(V2 at) {
        int depth = (int)at.y;
        if (!holesByHeight.ContainsKey(depth)) holesByHeight[depth] = new List<V2>();
        if (holesByHeight[(int)at.y].Contains(at)) return;
        holes.Add(at);
        holesByHeight[(int)at.y].Add(at);
    }
    bool badcoord(int x, int y, bool allowboundaries)
    {

        int abi = allowboundaries ? 1 : 0;
        if (x < 1 - abi) return true;
        if (x >= grid.GetLength(0) - 1 + abi) return true;
        if (y < 1 - abi) return true;
        if (y >= grid.GetLength(1) - 1 + abi) return true;
        return false;
    }

    bool badcoord(int x, int y){
        return badcoord(x, y, false);
    }

    void SetGrid(int[,] grid, float x, float y, int v, bool safe, bool addholes)
    {
        int xx = (int)x;
        int yy = (int)y;
        if (!safe){
            
            if (badcoord(xx, yy)) return;
        }
        grid[xx, yy] = v;
        if (addholes && v == 0)
        {
            AddHole(new V2(xx, yy));
        }
    }
    void SetGrid(int[,] grid, float x, float y, int v, bool safe){
        SetGrid(grid, x, y, v, safe, true);
    }
    void SetGrid(int[,] grid, float x, float y, int v){
        SetGrid(grid, x, y, v, false, true);
    }

    public int GetGrid(int[,] grid, float x, float y, bool safe)
    {
        if (!safe && badcoord((int)x, (int)y)) return y >= SIZEY ? 0 : 1;
        return grid[(int)x, (int)y];
    }
    public int GetGrid(int[,] grid, float x, float y){
        return GetGrid(grid, x, y, false);
    }

    void CreateTerrain()
    {
        for (int ix = 0; ix < SIZEX; ix++)
        {
            for (int iy = 0; iy < SIZEY; iy++)
            {
                EnsureTerrainAt(ix, iy);
            }
        }
    }

    // ensure terrain near the given coordinates, and remove other terrain
    // assumes that this is the only function used to generate terrain!
    void EnsureTerrainNear(int ix, int iy, int count){
        // only load at most 10;
        if (count == 0) count = 10000;
        float starttime = Time.realtimeSinceStartup;
        float timetojust = 0;
        // if (lastensured == new V2(ix, iy)) return;
        if (V2.Distance(new V2(ix, iy), lastensured) > 1.01f || true) {
            for (int xx = -generationSize; xx <= generationSize; xx++){
                for (int yy = -generationSize; yy <= generationSize; yy++){
                    if (count <= 0) break;
                    V2 pos = new V2(ix + xx, iy + yy);
                    if (terrainMap.ContainsKey(pos))
                    {
                        continue;
                    }
                    else
                    {
                        float t = Time.realtimeSinceStartup;
                        bool success = EnsureTerrainAt(ix + xx, iy + yy);
                        if (success) {
                            timetojust += Time.realtimeSinceStartup - t;
                            count -= 1;
                        }
                        //if (success) terrainMap[pos] =
                    }
                }
            }
            List<V2> todelete = new List<V2>();
            foreach (V2 key in terrainMap.Keys) {
                if (Mathf.Abs(key.x - ix) > generationSize || Mathf.Abs(key.y - iy) > generationSize)
                {
                    Destroy(terrainMap[key]);
                    todelete.Add(key);
                }
            }
            foreach(V2 v in todelete){
                terrainMap.Remove(v);
            }
            lastensured = new V2(ix, iy);
            //print(timetojust + " " + (Time.realtimeSinceStartup - starttime));
        } else {
/*
            for (int xx = -generationSize; xx <= generationSize; xx++) {
                for (int yy = -generationSize; xx <= generationSize; xx++)
                    V2 pos = new V2(ix + xx, iy + yy);
                    if 
                    if (terrainMap.ContainsKey(pos))
                    {
                        continue;
                    }
                    else
                    {
                        EnsureTerrainAt(ix + xx, iy + yy);
                        //if (success) terrainMap[pos] =
                    }
                }
            }
            */
        }
    }

    /*void DeleteTerrainAt(V2 v) {
        if (terrainMap.ContainsKey(v)) {
            Destroy(terrainMap[v]);
            //terrainMap[v] = null;
        }
    }*/

    // add terrain at position
    // return whether added something
    bool EnsureTerrainAt(int ix, int iy)
    {
        //return;
        float uvscale = 0.1f;
        if (badcoord(ix, iy, true)) return false; // true means, boundaries are not bad coordinates
        if (GetGrid(grid, ix, iy, true) == 1)
        {
            //if (Surrounded(ix, iy)) continue;
            bool surrounded = Surrounded(ix, iy);

            V3 center = CornerPoint(ix, iy);
            List<V3> pts = new List<V3>();
            List<V2> uvs = new List<V2>();
            List<V3> norms = new List<V3>();
            pts.Add(center);
            uvs.Add((CornerPointUV(ix, iy)) * uvscale);
            norms.Add(V3.back);
            List<V2> corners = new List<V2>{
                new V2(ix-0.5f, iy-0.5f),
                new V2(ix-0.5f, iy+0.5f),
                new V2(ix+0.5f, iy+0.5f),
                new V2(ix+0.5f, iy-0.5f)};
            List<V2> sidechecks = new List<V2>{
                new V2(ix-1, iy),
                new V2(ix, iy+1),
                new V2(ix+1, iy),
                new V2(ix, iy-1)};
            List<V2> diagchecks = new List<V2>{
                new V2(ix-1, iy-1),
                new V2(ix-1, iy+1),
                new V2(ix+1, iy+1),
                new V2(ix+1, iy-1)};
            List<GameObject> boundarysquiggles = new List<GameObject>();
            for (int i = 0; i < 4; i++)
            {
                V2 c = corners[i];
                V2 c2 = corners[(i + 1) % 4];
                bool cinning = false;
                bool c2inning = false;
                bool couting = false;
                //bool c2outing = false;

                V2 checkprev = sidechecks[(i + 3) % 4];
                V2 checkcurr = sidechecks[i];
                V2 checknext = sidechecks[(i + 1) % 4];

                // checks the diagonal cell in direction of current main corner point
                V2 checkdiag = diagchecks[i];

                // checks for the prev, curr and next side... for emptiness
                bool checkocurr = GetGrid(grid, (int)checkcurr.x, (int)checkcurr.y) == 0;
                bool checkonext = GetGrid(grid, (int)checknext.x, (int)checknext.y) == 0;
                bool checkoprev = GetGrid(grid, (int)checkprev.x, (int)checkprev.y) == 0;
                bool checkodiag = GetGrid(grid, (int)checkdiag.x, (int)checkdiag.y) == 0;

                // if c2 side nbrs empty, move inward
                // TODO THIS WILL CHANGE
                if (checkocurr && checkonext)
                {
                    //c2 = V2.Lerp(c2, new V2(ix, iy), 0.2f);
                    c2inning = true;
                }
                if (checkocurr && checkoprev)
                {
                    //c = V2.Lerp(c, new V2(ix, iy), 0.2f);
                    cinning = true;
                }

                /*if (!checkocurr && !checkonext)
                {
                    //c2 = V2.Lerp(c2, new V2(ix, iy), 0.2f);
                    c2outing = true;
                }*/
                if (!checkocurr && !checkoprev && !checkodiag)
                {
                    //c = V2.Lerp(c, new V2(ix, iy), 0.2f);
                    couting = true;
                }

                // if this side has stuff, just make a straight, i.e. only add corner point
                if (!checkocurr && c.y != SIZEY - 1)
                {
                    if (couting) c = V2.MoveTowards(c, new V2(ix, iy), -0.2f);
                    pts.Add(CornerPoint(c.x, c.y));
                    uvs.Add((V2)CornerPointUV(c.x, c.y) * uvscale);
                    norms.Add(V3.back);

                }
                // otherwise make a nice curvy side
                else
                {
                    V3 lastpoint = Vector3.zero;
                    // physical corner points
                    for (int j = 0; j <= 25; j++)
                    {
                        float inamt = 0;
                        if (cinning && j < 5)
                        {
                            inamt = 5 - j;
                            //inamt = 3;
                        }
                        if (c2inning && j >= 21)
                        {
                            inamt = j - 20;
                        }
                        V2 p = V2.Lerp(c, c2, j / 25f);
                        p = V2.MoveTowards(p, new V2(ix, iy), Mathf.Pow(inamt, 1.09f) * 0.02f);
                        if (j < 25)
                        {
                            pts.Add(CornerPoint(p.x, p.y));
                            uvs.Add(CornerPointUV(p.x, p.y) * uvscale);
                            norms.Add(V3.back);
                        }
                        else lastpoint = p;
                    }


                    // we also want to make boundary
                    List<V3> boundaryvertices = new List<V3>();
                    List<V2> boundaryuvs = new List<V2>();
                    List<V3> boundarynorms = new List<V3>();
                    List<V3> actuals = pts.GetRange(pts.Count - 25, 25);
                    actuals.Add(lastpoint);
                    for (int ii = 0; ii < 25; ii++)
                    {
                        boundaryvertices.Add(actuals[ii] - Vector3.forward * 0.05f);
                        V3 innn = V3.MoveTowards(actuals[ii], center, 0.4f);
                        boundaryvertices.Add(innn - Vector3.forward * 0.05f);
                        boundaryuvs.Add((V2)actuals[ii]);
                        boundaryuvs.Add((V2)innn);
                        boundarynorms.Add(Vector3.forward);
                        boundarynorms.Add(Vector3.forward);
                    }
                    List<int> boundarytriangles = new List<int>();
                    for (int ii = 0; ii < 24; ii++)
                    {
                        boundarytriangles.Add(2 * ii);
                        boundarytriangles.Add(2 * ii + 2);
                        boundarytriangles.Add(2 * ii + 1);
                        boundarytriangles.Add(2 * ii + 1);
                        boundarytriangles.Add(2 * ii + 2);
                        boundarytriangles.Add(2 * ii + 3);

                    }


                    Mesh mb = new Mesh();
                    mb.vertices = boundaryvertices.ToArray();
                    mb.uv = boundaryuvs.ToArray();
                    mb.triangles = boundarytriangles.ToArray();
                    mb.normals = boundarynorms.ToArray();
                    mb.RecalculateBounds();
                    GameObject boundary = new GameObject();
                    boundary.name = "kikkoman";
                    boundary.AddComponent<MeshFilter>().mesh = mb;
                    MeshRenderer mbr = boundary.AddComponent<MeshRenderer>();
                    mbr.material = boundarymat;
                    boundarysquiggles.Add(boundary);

                }
            }

            List<int> triangles = new List<int>();
            for (int i = 1; i < pts.Count; i++)
            {
                triangles.Add(0);
                triangles.Add(i);
                int ii = i + 1;
                if (ii == pts.Count) ii = 1; // wrap to 1 not 0
                triangles.Add(ii);
            }
            Mesh m = new Mesh();
            m.vertices = pts.ToArray();
            m.uv = uvs.ToArray();
            m.triangles = triangles.ToArray();
            m.normals = norms.ToArray();
            GameObject polygon = new GameObject();
            polygon.AddComponent<MeshFilter>().mesh = m;
            MeshRenderer mr = polygon.AddComponent<MeshRenderer>();
            mr.material = maamat;
            m.RecalculateBounds();
            //if (!surrounded) {
            PolygonCollider2D pc = polygon.AddComponent<PolygonCollider2D>();
            pc.points = pts.GetRange(1, pts.Count - 1).Select(v3 => (V2)v3).ToArray();
            //}
            foreach (GameObject b in boundarysquiggles){
                b.transform.SetParent(polygon.transform, true);
            }
            terrainMap[new V2(ix, iy)] = polygon;
            return true;
        }
        return false;
    }

    bool Surrounded(int ix, int iy) {
        return GetGrid(grid, ix-1, iy) != 0 && GetGrid(grid, ix+1, iy) != 0 &&
            GetGrid(grid, ix, iy-1) != 0 && GetGrid(grid, ix, iy+1) != 0;
    }

    void CreatePolygonForComponent()
    {

    }

    V2 nbrof(V2 v, V2 prev)
    {
        V2 u = v;

        /*if (v != prev && Random.Range(0, 2) == 0) {
            return v + (v - prev);
        }*/
        //else {
        int r = Random.Range(0, 6);
        if (r == 0) u = v + V2.right;
        else if (r == 1) u = v + V2.left;
        else if (r == 2) u = v + V2.up;
        else u = v + V2.down;
        //}
        // retry if same as previous
        if (u == prev) return nbrof(v, prev);
        if (badcoord((int)u.x, (int)u.y)) return v;
        return u;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
