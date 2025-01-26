using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using V2 = UnityEngine.Vector2;
using V3 = UnityEngine.Vector3;
using Unity.Collections;
using System.Net.Http.Headers;
using UnityEngine.ParticleSystemJobs;

public class CreateWorld : MonoBehaviour
{

    public Material maamat;
    public Material boundarymat;
    public GameObject sukeltaja;
    public GameObject[] plants;
    public GameObject helmisimpukka;

    public GameObject[] goodies;
    public float[] depthmins;
    public float[] depthmaxes;
    public int[] goodieamounts;
    public int[] bunches;

    /*public GameObject[] enemies;
    public float[] enemydepthmins;
    public float[] enemydepthmaxes;*/

    List<V2> safes;
    List<V2> safenbrs;



    public int[,] grid;
    public int startx = 75;
    public int starty = 199;
    int SIZEX = 150;
    int SIZEY = 200;

    int KUPLAMIN = 100;
 
    List<V2> holes;

    List<GameObject> WorldObjects;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        WorldObjects = new List<GameObject>();
        Random.InitState(661);

        safes = new List<V2>();
        safenbrs = new List<V2>();
        holes = new List<V2>();

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

        int mains = 3; //Random.Range(1, 3);
        for (int i = 0; i < mains; i++)
        {
            V2 v = new V2(Random.Range(SIZEX / 5, 4 * SIZEX / 5), 1);
            SubdivideThingo(new V2(startx, starty), v);
            if (i == 0) {
                GameObject helmi = GameObject.Instantiate(helmisimpukka);
                helmi.transform.position = CornerPoint(v.x, v.y);
                WorldObjects.Add(helmi);
            }
        }

        for (int i = 0; i < 20; i++)
        {
            CreateRandomSturmPath(Random.Range(15, 40));
            CreateRandomPath();
        }

        for (int i = 0; i < 20; i++)
        {
            CreateRandomSturmPath(Random.Range(5, 15));
            CreateRandomPath();
        }

        /*for (int i = 0; i < 10; i++){
            
        }*/

        // make small goodies
        for (int g = 0; g < goodies.Length; g++)
        {
            if (goodies[g].GetComponent<BigScript>()) continue;
            for (int i = 0; i < goodieamounts[g]; i++)
            {
                
                MakeGoodie(g);
            }
        }

        //grid[startx+1, starty-1] = 0;
        for (int i = 0; i < 20; i++)
        {
            CreateRandomBlob();
        }

        // make big goodies
        for (int g = 0; g < goodies.Length; g++)
        {
            if (!goodies[g].GetComponent<BigScript>()) continue;
            for (int i = 0; i < goodieamounts[g]; i++)
            {
                MakeGoodie(g);
            }
        }

        

        for (int i = 0; i < 120; i++)
        {
            MakePlant();
        }

        RemoveExcessTerrain();
        CreateTerrain();

        /*for (int g = 0; g < enemies.Length; g++){
            for (int i = 0; i < 50; i++){
                MakeEnemy(g);
            }
        }*/

        GameObject.Find("Enabler").GetComponent<EnablerScript>().objects = WorldObjects;
        //GameObject.Find("Enabler").GetComponent<EnablerScript>().Initialize();

    }

    void RemoveExcessTerrain(){
        List<V2> allfulls = new List<V2>();
        for (int ix = 0; ix < SIZEX; ix++)
        {
            for (int iy = 0; iy < SIZEY; iy++)
            {
                bool allfull = true;
                for (int xx = -2; xx <= 2; xx++) {
                    for (int yy = -2; yy <= 2; yy++) {
                        if (GetGrid(grid, ix+xx, iy+yy) == 0){
                            allfull = false;
                            break;
                        }
                    }
                }
                if (allfull) allfulls.Add(new V2(ix, iy));
            }
        }
        foreach(V2 v in allfulls) {
            SetGrid(grid, v.x, v.y, 0);
        }
    }

    void MakeGoodie(int g)
    {
        V2 hole = holes[Random.Range(0, holes.Count)];
        while (SIZEY - hole.y < depthmins[g] || SIZEY - hole.y > depthmaxes[g] ||
            (goodies[g].GetComponent<BigScript>() != null &&
            !BigEnoughHole(hole.x, hole.y) )){
            hole = holes[Random.Range(0, holes.Count)];
        }
        int amt = Random.Range(1, bunches[g] + 1);
        for (int t = 0; t < amt; t++)
        {
            GameObject good = GameObject.Instantiate(goodies[g]);
            WorldObjects.Add(good);
            good.transform.position = CornerPoint(hole.x + Random.Range(-0.5f,0.5f),
            hole.y + Random.Range(-0.5f,0.5f));
        }
    }

    // big enough hole for a big object?
    bool BigEnoughHole(float x, float y){
        int left = 1;
        int right = 1;
        int up = 1; 
        int down = 0;

        int xx = (int)x;
        int yy = (int)y;
        for (int xxx = xx-left; xxx <= xx + right+1; xxx ++){
            for (int yyy = yy-down; yyy <= yy + up+1; yyy ++){
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
        
        V2 v = (hole + n)/2;
        //print(v);
        p.transform.position = CornerPoint(v.x, v.y);
        if (n.x > hole.x) {
            p.transform.rotation = UnityEngine.Quaternion.Euler(0, 0, 80);
        }
        else if (n.x < hole.x) {
            p.transform.rotation = UnityEngine.Quaternion.Euler(0, 0, -80);
        }
        WorldObjects.Add(p);
        
    }

    // from int point to actual world point
    public V3 CornerPoint(float x, float y){
        float c = Mathf.Min(1, (SIZEY - y)/10);
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
                    if (!holes.Contains(curr + new V2(ix, iy))) holes.Add(curr + new V2(ix, iy));
                }
            }
        }
    }
    bool badcoord(int x, int y)
    {
        if (x < 1) return true;
        if (x >= grid.GetLength(0) - 1) return true;
        if (y < 1) return true;
        if (y >= grid.GetLength(1) - 1) return true;
        return false;
    }

    void SetGrid(int[,] grid, float x, float y, int v)
    {
        x = (int)x;
        y = (int)y;
        if (badcoord((int)x, (int)y)) return;
        grid[(int)x, (int)y] = v;
        if (!holes.Contains(new V2(x, y))) holes.Add(new V2(x, y));
    }

    public int GetGrid(int[,] grid, float x, float y){
        if (badcoord((int)x, (int)y))return 1;
        
        return grid[(int)x, (int)y];
    }

    // go through non-0 components 
    void CreateTerrain()
    {

        float uvscale = 0.1f;

        for (int ix = 0; ix < SIZEX; ix++)
        {
            for (int iy = 0; iy < SIZEY; iy++)
            {
                if (grid[ix, iy] == 1)
                {
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
                    for (int i = 0; i < 4; i++)
                    {
                        V2 c = corners[i];
                        V2 c2 = corners[(i + 1) % 4];
                        bool cinning = false;
                        bool c2inning = false;

                        V2 checkprev = sidechecks[(i + 3) % 4];
                        V2 checkcurr = sidechecks[i];
                        V2 checknext = sidechecks[(i + 1) % 4];

                        // checks for the prev, curr and next side... for emptiness
                        bool checkocurr = GetGrid(grid, (int)checkcurr.x, (int)checkcurr.y) == 0;
                        bool checkonext = GetGrid(grid, (int)checknext.x, (int)checknext.y) == 0;
                        bool checkoprev = GetGrid(grid, (int)checkprev.x, (int)checkprev.y) == 0;

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

                        // if this side has stuff, just make a straight, i.e. only add corner point
                        if (!checkocurr)
                        {
                            pts.Add(CornerPoint(c.x, c.y));
                            uvs.Add((V2)CornerPointUV(c.x, c.y) * uvscale);
                            norms.Add(V3.back);

                        }
                        // otherwise make a nice curvy side
                        else
                        {

                            // physical corner points
                            for (int j = 0; j < 25; j++)
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
                                pts.Add(CornerPoint(p.x, p.y));
                                uvs.Add(CornerPointUV(p.x, p.y) * uvscale);
                                norms.Add(V3.back);
                            }

                            // we also want to make boundary
                            List<V3> boundaryvertices = new List<V3>();
                            List<V2> boundaryuvs = new List<V2>();
                            List<V3> boundarynorms = new List<V3>();
                            List<V3> actuals = pts.GetRange(pts.Count-25, 25);
                            for (int ii = 0; ii < 25; ii++) {
                                boundaryvertices.Add(actuals[ii]);
                                V3 innn = V3.MoveTowards(actuals[ii], center, 0.4f);
                                boundaryvertices.Add(innn);
                                boundaryuvs.Add((V2)actuals[ii]);
                                boundaryuvs.Add((V2)innn);
                                boundarynorms.Add(Vector3.forward);
                                boundarynorms.Add(Vector3.forward);
                            }
                            List<int> boundarytriangles = new List<int>();
                            for (int ii = 0; ii < 24; ii++) {
                                boundarytriangles.Add(2*ii);
                                boundarytriangles.Add(2*ii+2);
                                boundarytriangles.Add(2*ii+1);
                                boundarytriangles.Add(2*ii+1);
                                boundarytriangles.Add(2*ii+2);
                                boundarytriangles.Add(2*ii+3);

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
                    PolygonCollider2D pc = polygon.AddComponent<PolygonCollider2D>();
                    pc.points = pts.GetRange(1, pts.Count - 1).Select(v3 => (V2)v3).ToArray();
                }
            }
        }
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
