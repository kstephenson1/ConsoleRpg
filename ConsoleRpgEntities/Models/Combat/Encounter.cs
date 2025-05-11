using ConsoleRpgEntities.DataTypes;
using ConsoleRpgEntities.Models.Items.EquippableItems.WeaponItems;
using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Models.Interfaces.ItemBehaviors;

namespace ConsoleRpgEntities.Models.Combat;

public class Encounter
{
    // Encounter represents a combat encounter between two units. It calculates the damage, hit chance, and critical
    // hit chance based on the units' stats and equipped items. It also determines the outcome of the encounter based
    // on the roll of a random number.

    // Random number generator for simulating combat rolls
    private readonly Random _generator = new();

    // Dictionary to hold the weapon triangle advantage values for each weapon type combination
    private readonly Dictionary<Tuple<WeaponType, WeaponType>, int> dict = new();

    public int Roll {  get; private set; }
    public IUnit Unit { get; set; }
    public IUnit Target { get; set; }
    public int Damage => RollDamage();

    public Encounter(IUnit unit, IUnit target)
    {
        // Initialize the encounter with the attacking unit and the defending unit
        Roll = _generator.Next(100) + 1;
        Unit = unit;
        Target = target;

        // Initialize the weapon triangle advantage values for each weapon type combination Swords are strong against
        // axes, axes are strong against spears, and spears are strong against swords Elemental magic is strong
        // against light, light is strong against dark, and dark is strong against elemental magic Bows and healing
        // do not get any advantage or disadvantage.
        Tuple<WeaponType, WeaponType> swordVsAxe = new(WeaponType.Sword, WeaponType.Axe);
        Tuple<WeaponType, WeaponType> swordVsLance = new(WeaponType.Sword, WeaponType.Lance);
        Tuple<WeaponType, WeaponType> axeVsSword = new(WeaponType.Axe, WeaponType.Sword);
        Tuple<WeaponType, WeaponType> axeVsLance = new(WeaponType.Axe, WeaponType.Lance);
        Tuple<WeaponType, WeaponType> LanceVsSword = new(WeaponType.Lance, WeaponType.Sword);
        Tuple<WeaponType, WeaponType> LanceVsAxe = new(WeaponType.Lance, WeaponType.Axe);

        Tuple<WeaponType, WeaponType> eleVsLight = new(WeaponType.Elemental, WeaponType.Light);
        Tuple<WeaponType, WeaponType> eleVsDark = new(WeaponType.Elemental, WeaponType.Dark);
        Tuple<WeaponType, WeaponType> lightVsEle = new(WeaponType.Light, WeaponType.Elemental);
        Tuple<WeaponType, WeaponType> lightVsDark = new(WeaponType.Light, WeaponType.Dark);
        Tuple<WeaponType, WeaponType> darkVsEle = new(WeaponType.Dark, WeaponType.Elemental);
        Tuple<WeaponType, WeaponType> darkVsLight = new(WeaponType.Dark, WeaponType.Light);

        dict.Add(swordVsAxe, 1);
        dict.Add(axeVsLance, 1);
        dict.Add(LanceVsSword, 1);
        dict.Add(swordVsLance , -1);
        dict.Add(LanceVsAxe, -1);
        dict.Add(axeVsSword , -1);

        dict.Add(eleVsLight , 1);
        dict.Add(lightVsDark, 1);
        dict.Add(darkVsEle, 1);
        dict.Add(eleVsDark , -1);
        dict.Add(darkVsLight, -1);
        dict.Add(lightVsEle , -1);
    }

    /// <summary>
    /// RollDamage rolls the dice to determine the damage dealt in the encounter. If the roll is a critical hit,
    /// the damage is doubled. If the roll is a hit, the damage is calculated based on the attacking unit's
    /// attack and the defending unit's resiliance. If the roll is a miss, the damage is zero.
    /// </summary>
    /// <returns></returns>
    public int RollDamage()
    {
        if (IsCrit())
        {
            if (Unit.GetEquippedWeapon() is PhysicalWeaponItem)
            {
                return (int)MathF.Max(GetDamage(), 0) * 3;
            }
            else if (Unit.GetEquippedWeapon() is MagicWeaponItem)
            {
                return (int)MathF.Max(GetMagicDamage(), 0) * 3;
            }
            
        }
        else if (IsHit())
        {
            if (Unit.GetEquippedWeapon() is PhysicalWeaponItem)
            {
                return (int)MathF.Max(GetDamage(), 0);
            }
            else if (Unit.GetEquippedWeapon() is MagicWeaponItem)
            {
                return (int)MathF.Max(GetMagicDamage(), 0);
            }
        }
        return 0;
    }

    /// <summary>
    /// IsCrit checks if the roll is a critical hit. A critical hit is determined by the roll being greater than
    /// the displayed critical hit chance. The displayed critical hit chance is calculated based on the attacking
    /// unit's crit and the defending unit's crit avoid.
    /// </summary>
    /// <returns></returns>
    public bool IsCrit()
    {
        bool crit = Roll > MathF.Abs(GetDisplayedCrit() - 100);
        return crit;
    }

    /// <summary>
    /// IsHit checks if the roll is a hit. A hit is determined by the roll being greater than the displayed hit chance.
    /// The displayed hit chance is calculated based on the attacking unit's hit and the defending unit's avoid.
    /// </summary>
    /// <returns></returns>
    public bool IsHit()
    {
        bool hit = Roll > MathF.Abs(GetDisplayedHit() - 100);
        return hit;
    }

    /// <summary>
    /// GetTriangleDamageModifier checks the weapon triangle advantage of the attacking unit's weapon against
    /// the defending unit's weapon. If the attacking unit's weapon has an advantage against the defending
    /// unit's weapon, the damage is increased by 1. If the attacking unit's weapon has a disadvantage
    /// against the defending unit's weapon, the damage is decreased by 1. If the attacking unit's weapon
    /// and the defending unit's weapon are the same type, the damage is not modified.
    /// </summary>
    /// <returns></returns>
    private int GetTriangleDamageModifier()
    {
        if (Unit.GetEquippedWeapon() == null || Target.GetEquippedWeapon() == null) return 0;

        Tuple<WeaponType,WeaponType> weapons = new(Unit.GetEquippedWeapon()!.WeaponType, Target.GetEquippedWeapon()!.WeaponType);
        dict.TryGetValue(weapons, out int value);
        if (value != 0) return value;
        return 0;
    }

    /// <summary>
    /// GetTriangleHitModifier checks the weapon triangle advantage of the attacking unit's weapon against
    /// the defending unit's weapon. If the attacking unit's weapon has an advantage against the defending
    /// unit's weapon, the hit chance is increased by 15. If the attacking unit's weapon has a disadvantage
    /// against the defending unit's weapon, the hit chance is decreased by 15. If the attacking unit's weapon
    /// and the defending unit's weapon are the same type, the hit chance is not modified.
    /// </summary>
    /// <returns></returns>
    private int GetTriangleHitModifier()
    {
        return GetTriangleDamageModifier() * 15;
    }

    /// <summary>
    /// GetAttack calculates the physical attack damage of the attacking unit. The attack damage is calculated based on
    /// the attacking unit's strength and the equipped weapon's might. 
    /// </summary>
    /// <returns></returns>
    public int GetAttack()
    {
        int weaponEfficiency = 1; // for future implementation.
        return Unit.Stat.Strength + weaponEfficiency * (Unit.GetEquippedWeapon()!.Might + GetTriangleDamageModifier());
    }

    /// <summary>
    /// GetMagicAttack calculates the magic attack damage of the attacking unit. The magic attack damage is calculated based on
    /// the attacking unit's magic and the equipped weapon's might. 
    /// </summary>
    /// <returns></returns>
    public int GetMagicAttack()
    {
        int weaponEfficiency = 1; // for future implementation.
        return Unit.Stat.Magic + weaponEfficiency * (Unit.GetEquippedWeapon()!.Might + GetTriangleDamageModifier());
    }

    /// <summary>
    /// GetPhysicalResiliance calculates the physical resiliance of the defending unit. The physical resiliance is calculated
    /// based on the defending unit's defense and the equipped armor's defense.
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public int GetTargetPhysicalResiliance()
    {
        int defense = Target.Stat.Defense;
        List<IEquippableArmor> equippedArmor = InventoryHelper.GetEquippedArmor(Target);
        foreach (IEquippableArmor armor in equippedArmor)
        {
            defense += armor.Defense;
        }
        return defense;
    }

    /// <summary>
    /// GetMagicResiliance calculates the magic resiliance of the defending unit. The magic resiliance is calculated
    /// based on the defending unit's resistance and the equipped armor's resistance.
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public int GetTargetMagicResiliance()
    {
        int resiliance = Target.Stat.Resistance;
        List<IEquippableArmor> equippedArmor = InventoryHelper.GetEquippedArmor(Target);
        foreach(IEquippableArmor armor in equippedArmor)
        {
            resiliance += armor.Resistance;
        }
        return resiliance;
    }

    /// <summary>
    /// GetDamage calculates the physical damage dealt in the encounter. The damage is calculated based on the attacking
    /// unit's attack and the defending unit's physical resiliance. The damage is modified by the weapon triangle advantage.
    /// </summary>
    /// <returns></returns>
    private int GetDamage()
    {
        return GetAttack() - GetTargetPhysicalResiliance();
    }

    /// <summary>
    /// GetMagicDamage calculates the magic damage dealt in the encounter. The damage is calculated based on the attacking
    /// unit's magic attack and the defending unit's magic resiliance. The damage is modified by the weapon triangle advantage.
    /// </summary>
    /// <returns></returns>
    private int GetMagicDamage()
    {
        return GetMagicAttack() - GetTargetMagicResiliance();
    }

    /// <summary>
    /// GetAttackSpeed calculates the attack speed of the attacking unit. The attack speed is calculated based on the
    /// attacking unit's speed and the equipped weapon's weight. If the weapon's weight is greater than the unit's
    /// constitution, the attack speed is decreased by the difference between the weapon's weight and the unit's
    /// constitution. If the attack speed is less than 0, the attack speed is set to 0.
    /// </summary>
    /// <returns></returns>
    private int GetAttackSpeed()
    {
        return (int)MathF.Max(Unit.Stat.Speed - (int)MathF.Max(Unit.GetEquippedWeapon()!.Weight - Unit.Stat.Constitution, 0), 0);
    }

    /// <summary>
    /// GetHit calculates the hit chance of the attacking unit. The hit chance is calculated based on the attacking
    /// unit's hit, the attacking unit's dexterity, the attacking unit's luck, and the weapon triangle advantage.
    /// The hit chance is modified by the weapon triangle advantage. If the hit chance is less than 0, the hit chance
    /// is set to 0. The hit chance is displayed as a percentage.
    /// </summary>
    /// <returns></returns>
    private int GetHit()
    {
        return (int)MathF.Max(Unit.GetEquippedWeapon()!.Hit + 2 * Unit.Stat.Dexterity + Unit.Stat.Luck / 2 + GetTriangleHitModifier(), 0);
    }

    /// <summary>
    /// GetAvoid calculates the avoid chance of the defending unit. The avoid chance is calculated based on the defending
    /// unit's attack speed, the defending unit's luck, and the terrain modifier. The avoid chance is displayed as a percentage.
    /// If the avoid chance is less than 0, the avoid chance is set to 0.
    /// </summary>
    /// <returns></returns>
    private int GetAvoid()
    {
        int terrainAvoidModifier = 0;// for future implementation.
        return (int)MathF.Max(2 * GetAttackSpeed() + Unit.Stat.Luck + terrainAvoidModifier, 0);
    }

    /// <summary>
    /// GetDisplayedHit calculates the displayed hit chance of the attacking unit. The displayed hit chance is calculated
    /// based on the attacking unit's hit and the defending unit's avoid. The displayed hit chance is displayed as a
    /// percentage. If the displayed hit chance is less than 0, the displayed hit chance is set to 0.
    /// </summary>
    /// <returns></returns>
    public int GetDisplayedHit()
    {
        return (int)MathF.Max(GetHit() - GetAvoid(), 0);
    }

    /// <summary>
    /// GetCrit calculates the critical hit chance of the attacking unit. The critical hit chance is calculated based on
    /// the attacking unit's crit and the attacking unit's dexterity. The critical hit chance is displayed as a
    /// percentage. If the critical hit chance is less than 0, the critical hit chance is set to 0.
    /// </summary>
    /// <returns></returns>
    private int GetCrit()
    {
        return (int)(MathF.Max(Unit.GetEquippedWeapon()!.Crit + Unit.Stat.Dexterity * 2, 0) - GetTargetCritAvoid());
    }

    /// <summary>
    /// GetCritAvoid calculates the critical hit avoid chance of the defending unit. The critical hit avoid chance is
    /// calculated based on the defending unit's luck. The critical hit avoid chance is displayed as a percentage.
    /// If the critical hit avoid chance is less than 0, the critical hit avoid chance is set to 0.
    /// </summary>
    /// <returns></returns>
    private int GetTargetCritAvoid()
    {
        return (int)MathF.Max(Target.Stat.Luck, 0);
    }

    /// <summary>
    /// GetDisplayedCrit calculates the displayed critical hit chance of the attacking unit. The displayed critical hit
    /// chance is calculated based on the attacking unit's crit and the defending unit's crit avoid. The displayed
    /// critical hit chance is displayed as a percentage. If the displayed critical hit chance is less than 0,
    /// the displayed critical hit chance is set to 0.
    /// </summary>
    /// <returns></returns>
    public int GetDisplayedCrit()
    {
        return (int)MathF.Max(GetHit() / 100 * GetCrit(), 0);
    }
}
