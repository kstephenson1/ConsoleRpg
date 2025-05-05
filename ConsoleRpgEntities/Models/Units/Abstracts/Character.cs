namespace ConsoleRpgEntities.Models.Units.Abstracts;
public abstract class Character : Unit
{
    // The character class stores information for each character.
    public override string ToString()
    {
        return $"{Name},{Class},{Level},{Stat.HitPoints}";
    }
}
