using UnityEngine.UIElements;

public class UIConverters
{
    [UnityEditor.InitializeOnLoadMethod]
    public static void InitConverters()
    {
        ConverterGroup shopGroup = new("shop");
        shopGroup.AddConverter((ref int level) => TurtleUpgrade.LevelToString(level));
        shopGroup.AddConverter((ref UpgradeType type) => TurtleUpgrade.TypeToString(type));
        ConverterGroups.RegisterConverterGroup(shopGroup);
    }
}