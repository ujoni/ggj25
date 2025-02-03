using UnityEngine;

public class ObjectGenTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        return;
        ObjectGenerationDescription o0 = new ObjectGenerationDescription();
        o0.objectName = "Naki";
        o0.SetDepths(3, 200, 200);
        o0.SetAmount(400);

        ObjectGenerationDescription o1 = new ObjectGenerationDescription();
        o1.objectName = "BigNaki";
        o1.SetDepths(30, 200, 200);
        o1.SetAmount(100);

        ObjectGenerationDescription o2 = new ObjectGenerationDescription();
        o2.objectName = "RainbowNaki";
        o2.SetDepths(100, 200, 200);
        o2.SetAmount(30);

        ObjectGenerationDescription o3 = new ObjectGenerationDescription();
        o3.objectName = "Submarine";
        o3.SetDepths(3, 200, 200);
        o3.SetAmount(40);

        ObjectGenerationDescription o4 = new ObjectGenerationDescription();
        o4.objectName = "Medusa";
        o4.SetDepths(25, 200, 200);
        o4.SetAmount(40);

        ObjectGenerationDescription o5 = new ObjectGenerationDescription();
        o5.objectName = "Kala";
        o5.SetDepths(3, 200, 200);
        o5.SetAmount(30);
        o5.SetBunch(30);

        ObjectGenerationDescription o6 = new ObjectGenerationDescription();
        o6.objectName = "Piraija";
        o6.SetDepths(100, 200, 200);
        o6.SetAmount(30);
        o6.SetBunch(30);

        ObjectGenerationDescription o7 = new ObjectGenerationDescription();
        o7.objectName = "Piraija";
        o7.SetDepths(40, 200, 200);
        o7.SetAmount(30);

        ObjectGenerationDescription o8 = new ObjectGenerationDescription();
        o8.objectName = "Shark";
        o8.SetDepths(140, 200, 200);
        o8.SetAmount(20);

        string p = Application.persistentDataPath;
        //ObjectGenerationDescription.SaveData(o, p + "/test.ogd");

        LevelGenerationDescription l = new LevelGenerationDescription();
        l.description = "GGJ level.";
        l.objects.Add(o0);
        l.objects.Add(o1);
        l.objects.Add(o2);
        l.objects.Add(o3);
        l.objects.Add(o4);
        l.objects.Add(o5);
        l.objects.Add(o6);
        l.objects.Add(o7);
        l.objects.Add(o8);
        LevelGenerationDescription.SaveData(l, p + "/test.lgd");
    }
}
