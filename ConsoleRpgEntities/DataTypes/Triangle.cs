namespace ConsoleRpgEntities.DataTypes;

/// <summary>
/// Represents the advantage or disadvantage of a combat triangle.
/// Swords are strong against axes, axes are strong against spears, and spears are strong against swords.
/// Elemental magic is string against light, light is strong against dark, and dark is strong against elemental magic.
/// Bows and healing do not get any advantage or disadvantage.
/// </summary>
public enum Triangle
{
    Disadvantage = -1,
    Default = 0,
    Advantage = 1
}
