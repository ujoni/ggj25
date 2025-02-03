using UnityEngine;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class ObjectGenerationDescription
{
    public string objectName; // this is the name of the resource
    // how many bunches are generated
    public int minAmount; 
    public int maxAmount;
    // 0...1; where are they generated, 0 means top, 1 means bottom
    public float mindepth;
    public float maxdepth;
    // -1...1; positive slope means there's more of these later
    public float depthslope;
    // how large are bunches
    public int minBunch;
    public int maxBunch;

    public ObjectGenerationDescription()
    {
        SetBunch(1);
        depthslope = 0; 
    }

    // set min and max depth; the scale is so you can use actual depths if you provide level size
    public void SetDepths(float min, float max, float scale) {
        mindepth = min/scale;
        maxdepth = max/scale;
    }
    public void SetDepths(float min, float max) {
        SetDepths(min, max, 1);
    }
    public void SetDepth(float depth, float scale) {
        SetDepths(depth, depth, scale);
    }
    public void SetDepth(float depth) {
        SetDepths(depth, depth, 1);
    }
    public void SetAmounts(int min, int max) {
        minAmount = min;
        maxAmount = max;
    }
    public void SetAmount(int amt) {
        minAmount = amt;
        maxAmount = amt;
    }
    public void SetBunches(int min, int max) {
        minBunch = min;
        maxBunch = max;
    }
    public void SetBunch(int amt) {
        minBunch = amt;
        maxBunch = amt;
    }

    public static void SaveData(ObjectGenerationDescription o, string filePath)
    {
        string json = JsonUtility.ToJson(o, true); // Pretty-print for readability
        File.WriteAllText(filePath, json);
        //Debug.Log("Data Saved to " + filePath);
    }

    public static ObjectGenerationDescription LoadData(string filePath)
    {
        string json = File.ReadAllText(filePath);
        return JsonUtility.FromJson<ObjectGenerationDescription>(json);
    }
}

/*
LevelGenerationDescription is a description of the random generation process
(for now, just always use the hard-coded one) and the info of where
different objects are generated.
*/
[System.Serializable]
public class LevelGenerationDescription
{
    // textual description of the level
    public string description;
    // the level objects
    public List<ObjectGenerationDescription> objects;

    public LevelGenerationDescription(){
        objects = new List<ObjectGenerationDescription>();
    }

    public static void SaveData(LevelGenerationDescription o, string filePath)
    {
        string json = JsonUtility.ToJson(o, true); // Pretty-print for readability
        File.WriteAllText(filePath, json);
        //Debug.Log("Data Saved to " + filePath);
    }

    public static LevelGenerationDescription LoadData(string filePath)
    {
        string json = File.ReadAllText(filePath);
        return JsonUtility.FromJson<LevelGenerationDescription>(json);
    }
}