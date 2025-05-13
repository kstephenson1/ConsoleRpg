using ConsoleRpgEntities.Models.Units.Abstracts;

namespace ConsoleRpgEntities.Models.Interfaces;

public interface IItem
{
    // Interface that allows items to exist.
    public string Name { get; set; }
    public string Description { get; set; }
    public int MaxDurability { get; set; }
    List<Unit> Units { get; set; }
}
