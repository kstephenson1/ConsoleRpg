using ConsoleRpgEntities.DataTypes;
using ConsoleRpgEntities.Models.Abilities;
using ConsoleRpgEntities.Models.Combat;
using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Models.Interfaces.ItemBehaviors;
using ConsoleRpgEntities.Models.Items;
using ConsoleRpgEntities.Models.Rooms;
using ConsoleRpgEntities.Models.Units.Abstracts;
using ConsoleRpgEntities.Services.Repositories;
using Spectre.Console;

namespace ConsoleRpgEntities.Models.UI.Character;

public class CharacterUI
{
    // CharacterUI helps display character information in a nice little table.

    AbilityService _abilityService;
    ItemService _itemService;
    RoomService _roomService;
    StatService _statService;
    UnitItemService _unitItemService;

    public CharacterUI(
        AbilityService abilityService,
        ItemService itemService,
        RoomService roomService,
        StatService statService,
        UnitItemService unitItemService)
    {
        _abilityService = abilityService;
        _itemService = itemService;
        _roomService = roomService;
        _statService = statService;
        _unitItemService = unitItemService;
    }

    public void DisplayCharacterInfo(IUnit unit) // Displays the character's info
    {
        List<Unit> unitList = [(Unit)unit];
        DisplayCharacterInfo(unitList);
    }

    public void DisplayCharacterInfo(List<Unit> units)
    {
        List<Room> rooms = _roomService.GetAll().ToList();

        foreach (Unit unit in units)
        {
            List<Item> characterItems = new();
            foreach (UnitItem unitItem in unit.UnitItems)
            {
                characterItems.Add(unitItem.Item);
            }
            Room? unitRoom;
            try
            {
                unitRoom = rooms.Where(r => r.Id == unit.CurrentRoom!.Id).FirstOrDefault()!;
            }
            catch { unitRoom = null; }
            DisplayCharacterInfo(unit, unit.Stat, characterItems, unitRoom!, unit.Abilities);
        }
    }

    public void DisplayCharacterInfo(IUnit unit, Stat stat, List<Item> items, Room room, List<Ability> abilities) // Displays the character's info
    {
        // Builds a character table with 2 lines: Name, Level and Class.
        Grid charTable = new Grid().Width(30).AddColumn();
        charTable
            .AddRow(new Text(unit.Name).Centered())
                .AddRow(new Text($"Level {unit.Level} {unit.Class}").Centered());

        // Builds an hp table that contains the health of the character
        Grid hpTable = new Grid().Width(25).AddColumn();
        hpTable
            .AddRow(new Text($"Hit Points:").Centered())
                .AddRow(new Text($"{stat.HitPoints}/{stat.MaxHitPoints}").Centered());

        //Creates a table with the unit's stats.
        Grid invHeader = new Grid().Width(30).AddColumns(3);
        invHeader
            .AddRow(
                new Text($" STR: {stat.Strength}").LeftJustified(),
                new Text($"MAG: {unit.Stat.Magic}").LeftJustified(),
                new Text($"CON: {unit.Stat.Constitution}").LeftJustified())
            .AddRow(
                new Text($" DEX: {stat.Dexterity}").LeftJustified(),
                new Text($"SPD: {unit.Stat.Speed}").LeftJustified(),
                new Text($"LCK: {unit.Stat.Luck}").LeftJustified())
            .AddRow(
                new Text($" DEF: {stat.Defense}").LeftJustified(),
                new Text($"RES: {unit.Stat.Resistance}").LeftJustified(),
                new Text($"MOV: {unit.Stat.Movement}").LeftJustified());


        // Creates an inventory table that lists all the items in the character's inventory.
        Grid invTable = new Grid();
        invTable.AddColumn();

        List<UnitItem> equippedInventory = new();

        //IEquippableWeapon? weapon = unit.GetEquippedWeapon();
        UnitItem weapon = unit.UnitItems.Where(ui => ui.Unit == unit && ui.Slot == EquipmentSlot.Weapon).FirstOrDefault()!;
        if (weapon != null)
        {
            equippedInventory.Add(weapon);
        }

        List<UnitItem> equippedArmor = unit.UnitItems.Where(
            ui => ui.Unit == unit &&
            ui.Slot == EquipmentSlot.Head ||
            ui.Slot == EquipmentSlot.Chest ||
            ui.Slot == EquipmentSlot.Legs ||
            ui.Slot == EquipmentSlot.Feet).ToList()!;

        foreach (UnitItem armor in equippedArmor)
        {
            equippedInventory.Add(armor);
        }

        if (equippedInventory.Any())
        {
            invTable.AddRow("Equipped Items: ");

            foreach (UnitItem unitItem in equippedInventory)
            {
                invTable.AddRow($"{unitItem.ToString()}");
            }
        }
        else
        {
            invTable.AddRow("Equipped Items: ---");
        }

            invTable.AddRow("\nInventory: ");

        //List<Item> unequippedInventory = InventoryHelper.GetUnequippedItemsInInventory(unit);
        List<UnitItem> unequippedInventory = unit.UnitItems.Where( ui => ui.Unit == unit && ui.Slot == EquipmentSlot.None).ToList()!;
        if (unequippedInventory.Count() != 0)
        {
            foreach (UnitItem unitItem in unequippedInventory!)
            {
                invTable.AddRow(" - " + unitItem.ToString());
            }
        }
        else
        {
            invTable.AddRow("(No Items)");
        }

        Grid roomTable = new Grid();
        roomTable.AddColumn();
        roomTable.AddRow("Current Room: " + (unit.CurrentRoom == null ? "null" : unit.CurrentRoom.Name));

        Grid abilityTable = new Grid();
        abilityTable.AddColumn();
        abilityTable.AddRow("Abilities: ");

        if (abilities.Any())
        {
            foreach(var ability in abilities)
            {
                abilityTable.AddRow(" - " + ability.Name);
            }
        }
        abilityTable.AddRow("");

        // Creates a display table that contains all the other tables to create a nice little display.
        Table displayTable = new Table();
        displayTable
            .AddColumn(new TableColumn(charTable))
            .AddColumn(new TableColumn(hpTable))
            .AddRow(roomTable, abilityTable)
            .AddRow(invHeader, invTable);

        // Displays the table to the user.
        AnsiConsole.Write(displayTable);
    }
}
