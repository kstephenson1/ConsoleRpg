using ConsoleRpgEntities.DataTypes;
using ConsoleRpgEntities.Models.Abilities.UnitAbilities;
using ConsoleRpgEntities.Models.Combat;
using ConsoleRpgEntities.Models.Dungeons;
using ConsoleRpgEntities.Models.Items;
using ConsoleRpgEntities.Models.Items.ConsumableItems;
using ConsoleRpgEntities.Models.Items.EquippableItems.ArmorItems;
using ConsoleRpgEntities.Models.Items.EquippableItems.WeaponItems;
using ConsoleRpgEntities.Models.Rooms;
using ConsoleRpgEntities.Models.Units.Abstracts;
using ConsoleRpgEntities.Models.Units.Characters;
using ConsoleRpgEntities.Models.Units.Monsters;
using ConsoleRpgEntities.Services;
using ConsoleRpgEntities.Services.Repositories;
using Spectre.Console;

namespace ConsoleRpgEntities.Data;

public class SeedHandler
{
    private readonly DungeonService _dungeonService;
    private readonly RoomService _roomService;
    private readonly UnitService _unitService;
    private readonly StatService _statService;
    private readonly ItemService _itemService;
    private readonly AbilityService _abilityService;
    private readonly UnitItemService _unitItemService;

    private readonly RoomFactory _roomFactory;

    private readonly List<Room> _rooms = new();

    private readonly FlyAbility _abilityFly = new();
    private readonly HealAbility _abilityHeal = new();
    private readonly StealAbility _abilitySteal = new();
    private readonly TauntAbility _abilityTaunt = new();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public SeedHandler(
        RoomFactory roomFactory,
        DungeonService dungeonService,
        RoomService roomService,
        UnitService unitService,
        StatService statService,
        ItemService itemService,
        AbilityService abilityService,
        UnitItemService unitItemService
        )
    {
        _roomFactory = roomFactory;
        _dungeonService = dungeonService;
        _roomService = roomService;
        _unitService = unitService;
        _statService = statService;
        _itemService = itemService;
        _abilityService = abilityService;
        _unitItemService = unitItemService;
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public void SeedDatabase()
    {
        // Checks if the database is empty before seeding. If it is empty, it generates the items, dungeons, abilities,
        // and characters. If the database is not empty, it skips the seeding process. A cute 'lil loading screen was
        // added to make it look like the game is loading.
        if (!_itemService.GetAll().Any())
        {
            DisplaySeedProgressBar();
            GenerateItems();
            _itemService.Commit();
        }

        if (!_dungeonService.GetAll().Any())
        {
            GenerateDungeons();
            _dungeonService.Commit();
        }

        if (!_abilityService.GetAll().Any())
        {
            GenerateAbilities();
            _abilityService.Commit();

        }

        if (!_unitService.GetAll().Any())
        {
            GenerateCharacters();
            _unitService.Commit();
        }
    }

    private void GenerateItems()
    {
        // Generates the items for the game. The items are added to the database and saved.

        // Consumable Items
        ItemPotion potion = new();
        ItemLockpick lockpick = new();
        ItemBook book = new();

        // Physical Weapons
        PhysicalWeaponItem itemWoodenSword = new("Wooden Sword", "A sword made of wood, usually for training.", WeaponType.Sword, Rank.E, 12, 1, 100, 0, 1, 1, 1);
        PhysicalWeaponItem itemIronSword = new("Iron Sword", "A common sword made of Iron.", WeaponType.Sword, Rank.E, 30, 5, 90, 0, 1, 4, 1);
        PhysicalWeaponItem itemBronzeSword = new("Bronze Sword", "A common sword made of Bronze.", WeaponType.Sword, Rank.E, 36, 6, 85, 0, 1, 4, 1);
        PhysicalWeaponItem itemSteelSword = new("Steel Sword", "A sword that weighs more and hits harder than iron.", WeaponType.Sword, Rank.E, 48, 8, 80, 0, 1, 5, 1);
        PhysicalWeaponItem itemSilverSword = new("Silver Sword", "A heavy, brittle sword that hits hard.", WeaponType.Sword, Rank.E, 18, 11, 60, 10, 1, 6, 1);
        PhysicalWeaponItem itemObsidianSword = new("Obsidian Sword", "A lightweight but brittle blade that can do some serious damage.", WeaponType.Sword, Rank.E, 24, 2, 90, 30, 1, 4, 1);
        PhysicalWeaponItem itemTitaniumSword = new("Titanium Sword", "A very lightweight sword that yields a sharp blade.", WeaponType.Sword, Rank.E, 42, 3, 100, 5, 1, 8, 1);
        PhysicalWeaponItem itemCobaltSword = new("Cobalt Sword", "A beautiful blue sword forged of cobalt.", WeaponType.Sword, Rank.E, 48, 5, 80, 0, 1, 10, 1);
        PhysicalWeaponItem itemInconelSword = new("Inconel Sword", "A shiny and durable sword made of a nickel alloy.", WeaponType.Sword, Rank.E, 54, 4, 75, 0, 1, 13, 1);
        PhysicalWeaponItem itemDarkSteelSword = new("Dark Steel Sword", "A sword forged by volcanic heat that is rivaled by few.", WeaponType.Sword, Rank.E, 54, 4, 70, 10, 1, 14, 1);
        PhysicalWeaponItem itemStarMetalSword = new("Star Metal Sword", "A dependable sword only used by the best, and richest, of fighters.", WeaponType.Sword, Rank.E, 60, 4, 65, 0, 1, 15, 1);
        PhysicalWeaponItem itemTungstenSword = new("Tungsten Sword", "A heavy hitting weapon that is almost impossible to wield.", WeaponType.Sword, Rank.E, 60, 10, 40, 10, 1, 20, 1);

        PhysicalWeaponItem itemWoodenAxe = new("Wooden Axe", "A axe made of wood, usually for training.", WeaponType.Axe, Rank.E, 12, 1, 100, 0, 1, 1, 1);
        PhysicalWeaponItem itemIronAxe = new("Iron Axe", "A common axe made of Iron.", WeaponType.Axe, Rank.E, 30, 4, 90, 0, 1, 8, 1);
        PhysicalWeaponItem itemBronzeAxe = new("Bronze Axe", "A common axe made of Bronze.", WeaponType.Axe, Rank.E, 36, 4, 85, 0, 1, 9, 1);
        PhysicalWeaponItem itemSteelAxe = new("Steel Axe", "A axe that weighs more and hits harder than iron.", WeaponType.Axe, Rank.E, 48, 5, 80, 0, 1, 11, 1);
        PhysicalWeaponItem itemSilverAxe = new("Silver Axe", "A heavy, brittle axe that hits hard.", WeaponType.Axe, Rank.E, 18, 6, 60, 10, 1, 14, 1);
        PhysicalWeaponItem itemObsidianAxe = new("Obsidian Axe", "A lightweight but brittle axe that can do some serious damage.", WeaponType.Axe, Rank.E, 24, 2, 90, 30, 1, 7, 1);
        PhysicalWeaponItem itemTitaniumAxe = new("Titanium Axe", "A very lightweight axe that yields a sharp blade.", WeaponType.Axe, Rank.E, 42, 3, 100, 5, 1, 11, 1);
        PhysicalWeaponItem itemCobaltAxe = new("Cobalt Axe", "A beautiful blue axe forged of cobalt.", WeaponType.Axe, Rank.E, 48, 5, 80, 0, 1, 13, 1);
        PhysicalWeaponItem itemInconelAxe = new("Inconel Axe", "A shiny and durable axe made of a nickel alloy.", WeaponType.Axe, Rank.E, 54, 4, 75, 0, 1, 16, 1);
        PhysicalWeaponItem itemDarkSteelAxe = new("Dark Steel Axe", "A axe forged by volcanic heat that is rivaled by few.", WeaponType.Axe, Rank.E, 54, 4, 70, 10, 1, 17, 1);
        PhysicalWeaponItem itemStarMetalAxe = new("Star Metal Axe", "A dependable axe only used by the best, and richest, of fighters.", WeaponType.Axe, Rank.E, 60, 4, 65, 0, 1, 18, 1);
        PhysicalWeaponItem itemTungstenAxe = new("Tungsten Axe", "A heavy hitting weapon that is almost impossible to wield.", WeaponType.Axe, Rank.E, 60, 10, 40, 10, 1, 23, 1);

        PhysicalWeaponItem itemWoodenLance = new("Wooden Lance", "A lance made of wood, usually for training.", WeaponType.Lance, Rank.E, 12, 1, 100, 0, 1, 1, 1);
        PhysicalWeaponItem itemIronLance = new("Iron Lance", "A common lance made of Iron.", WeaponType.Lance, Rank.E, 30, 4, 90, 0, 1, 6, 1);
        PhysicalWeaponItem itemBronzeLance = new("Bronze Lance", "A common lance made of Bronze.", WeaponType.Lance, Rank.E, 36, 4, 85, 0, 1, 7, 1);
        PhysicalWeaponItem itemSteelLance = new("Steel Lance", "A lance that weighs more and hits harder than iron.", WeaponType.Lance, Rank.E, 48, 5, 80, 0, 1, 9, 1);
        PhysicalWeaponItem itemSilverLance = new("Silver Lance", "A heavy, brittle lance that hits hard.", WeaponType.Lance, Rank.E, 18, 6, 60, 10, 1, 12, 1);
        PhysicalWeaponItem itemObsidianLance = new("Obsidian Lance", "A lightweight but brittle lance that can do some serious damage.", WeaponType.Lance, Rank.E, 24, 2, 90, 30, 1, 5, 1);
        PhysicalWeaponItem itemTitaniumLance = new("Titanium Lance", "A very lightweight lance that yields a sharp blade.", WeaponType.Lance, Rank.E, 42, 3, 100, 5, 1, 9, 1);
        PhysicalWeaponItem itemCobaltLance = new("Cobalt Lance", "A beautiful blue lance forged of cobalt.", WeaponType.Lance, Rank.E, 48, 5, 80, 0, 1, 11, 1);
        PhysicalWeaponItem itemInconelLance = new("Inconel Lance", "A shiny and durable lance made of a nickel alloy.", WeaponType.Lance, Rank.E, 54, 4, 75, 0, 1, 14, 1);
        PhysicalWeaponItem itemDarkSteelLance = new("Dark Steel Lance", "A lance forged by volcanic heat that is rivaled by few.", WeaponType.Lance, Rank.E, 54, 4, 70, 10, 1, 15, 1);
        PhysicalWeaponItem itemStarMetalLance = new("Star Metal Lance", "A dependable lance only used by the best, and richest, of fighters.", WeaponType.Lance, Rank.E, 60, 4, 65, 0, 1, 16, 1);
        PhysicalWeaponItem itemTungstenLance = new("Tungsten Lance", "A heavy hitting weapon that is almost impossible to wield.", WeaponType.Lance, Rank.E, 60, 10, 40, 10, 1, 21, 1);

        PhysicalWeaponItem itemWoodenBow = new("Wooden Bow", "A bow made of wood, usually for target practice.", WeaponType.Bow, Rank.E, 12, 1, 100, 0, 2, 1, 1);
        PhysicalWeaponItem itemIronBow = new("Iron Bow", "A common bow made of Iron.", WeaponType.Bow, Rank.E, 30, 4, 90, 0, 2, 6, 1);
        PhysicalWeaponItem itemBronzeBow = new("Bronze Bow", "A common bow made of Bronze.", WeaponType.Bow, Rank.E, 36, 4, 85, 0, 2, 7, 1);
        PhysicalWeaponItem itemSteelBow = new("Steel Bow", "A bow that weighs more and hits harder than iron.", WeaponType.Bow, Rank.E, 48, 5, 80, 0, 2, 9, 1);
        PhysicalWeaponItem itemSilverBow = new("Silver Bow", "A heavy, brittle bow that hits hard.", WeaponType.Bow, Rank.E, 18, 6, 60, 10, 2, 12, 1);
        PhysicalWeaponItem itemObsidianBow = new("Obsidian Bow", "A lightweight but brittle bow that can do some serious damage.", WeaponType.Bow, Rank.E, 24, 2, 90, 30, 2, 5, 1);
        PhysicalWeaponItem itemTitaniumBow = new("Titanium Bow", "A very lightweight bow that yields a sharp blade.", WeaponType.Bow, Rank.E, 42, 3, 100, 5, 2, 9, 1);
        PhysicalWeaponItem itemCobaltBow = new("Cobalt Bow", "A beautiful blue bow forged of cobalt.", WeaponType.Bow, Rank.E, 48, 5, 80, 0, 2, 11, 1);
        PhysicalWeaponItem itemInconelBow = new("Inconel Bow", "A shiny and durable bow made of a nickel alloy.", WeaponType.Bow, Rank.E, 54, 4, 75, 0, 2, 14, 1);
        PhysicalWeaponItem itemDarkSteelBow = new("Dark Steel Bow", "A bow forged by volcanic heat that is rivaled by few.", WeaponType.Bow, Rank.E, 54, 4, 70, 10, 2, 15, 1);
        PhysicalWeaponItem itemStarMetalBow = new("Star Metal Bow", "A dependable bow only used by the best, and richest, of fighters.", WeaponType.Bow, Rank.E, 60, 4, 65, 0, 2, 16, 1);
        PhysicalWeaponItem itemTungstenBow = new("Tungsten Bow", "A bow that is almost impossible to wield but shoots with great strength.", WeaponType.Bow, Rank.E, 60, 10, 40, 10, 2, 21, 1);

        // Magic Weapons
        MagicWeaponItem itemFireStaff = new("Fire Staff", "A staff that stays on fire, some how.", WeaponType.Elemental, Rank.E, 30, 2, 85, 0, 1, 1, 1);
        MagicWeaponItem itemIceStaff = new("Ice Staff", "A staff encased in a layer of frost.", WeaponType.Elemental, Rank.E, 30, 2, 75, 0, 1, 1, 1);
        MagicWeaponItem itemWindStaff = new("Wind Staff", "A staff surrounded by a mysterious breeze.", WeaponType.Elemental, Rank.E, 30, 2, 90, 10, 1, 1, 1);
        MagicWeaponItem itemLightningStaff = new("Lightning Staff", "A staff full of discharging static.", WeaponType.Elemental, Rank.E, 30, 2, 80, 5, 1, 1, 1);
        MagicWeaponItem itemHolyStaff = new("Holy Staff", "A staff filled with the holy word.", WeaponType.Light, Rank.E, 30, 2, 100, 15, 1, 1, 1);
        MagicWeaponItem itemDarkStaff = new("Dark Staff", "A staff surrounded with vile intentions.", WeaponType.Dark, Rank.E, 30, 2, 70, 0, 1, 1, 1);
        MagicWeaponItem itemHealingStaff = new("Healing Staff", "A staff containing healing magic.", WeaponType.Heal, Rank.E, 30, 2, 100, 0, 1, 1, 1);

        HeadArmorItem itemHood = new("Hood", "A basic hood", ArmorType.Head, Rank.E, 30, 0, 2, 1, 1);
        ChestArmorItem itemShirt = new("Shirt", "A basic shirt", ArmorType.Chest, Rank.E, 30, 0, 2, 1, 1);
        ChestArmorItem itemCloak = new("Cloak", "A basic cloak", ArmorType.Chest, Rank.E, 30, 0, 2, 1, 1);
        LegArmorItem itemPants = new("Pants", "A basic pants", ArmorType.Legs, Rank.E, 30, 0, 2, 1, 1);
        FeetArmorItem itemShoes = new("Shoes", "A basic shoes", ArmorType.Feet, Rank.E, 30, 0, 2, 1, 1);
        HeadArmorItem itemCap = new("Leather Cap", "A basic leather cap", ArmorType.Head, Rank.E, 30, 1, 1, 2, 1);
        ChestArmorItem itemTunic = new("Leather Tunic", "A basic leather tunic", ArmorType.Chest, Rank.E, 30, 1, 1, 2, 1);
        LegArmorItem itemStuddedPants = new("Studded Pants", "A basic studded pants", ArmorType.Legs, Rank.E, 30, 1, 1, 2, 1);
        FeetArmorItem itemBoots = new("Leather Boots", "A basic leather boots", ArmorType.Feet, Rank.E, 30, 1, 1, 2, 1);
        HeadArmorItem itemHelm = new("Helm", "A basic plate helm", ArmorType.Head, Rank.E, 30, 2, 0, 3, 1);
        ChestArmorItem itemPlate = new("Plate Armor", "A basic plate armor", ArmorType.Chest, Rank.E, 30, 2, 0, 3, 1);
        LegArmorItem itemGreaves = new("Greaves", "A basic plate greaves", ArmorType.Legs, Rank.E, 30, 2, 0, 3, 1);
        FeetArmorItem itemSabatons = new("Sabatons", "A basic plate sabatons", ArmorType.Feet, Rank.E, 30, 2, 0, 3, 1);

        _itemService.Add( new List<Item>() {
            // Consumable Items
            potion, lockpick, book,

            // Physical Weapons
            itemWoodenSword, itemIronSword, itemBronzeSword, itemSteelSword, itemSilverSword, itemObsidianSword, itemTitaniumSword, itemCobaltSword, itemInconelSword, itemDarkSteelSword, itemStarMetalSword, itemTungstenSword,
            itemWoodenAxe, itemIronAxe, itemBronzeAxe, itemSteelAxe, itemSilverAxe, itemObsidianAxe, itemTitaniumAxe, itemCobaltAxe, itemInconelAxe, itemDarkSteelAxe, itemStarMetalAxe, itemTungstenAxe,
            itemWoodenLance, itemIronLance, itemBronzeLance, itemSteelLance, itemSilverLance, itemObsidianLance, itemTitaniumLance, itemCobaltLance, itemInconelLance, itemDarkSteelLance, itemStarMetalLance, itemTungstenLance,
            itemWoodenBow, itemIronBow, itemBronzeBow, itemSteelBow, itemSilverBow, itemObsidianBow, itemTitaniumBow, itemCobaltBow, itemInconelBow, itemDarkSteelBow, itemStarMetalBow, itemTungstenBow,
            
            // Magic Weapons
            itemFireStaff, itemIceStaff, itemWindStaff, itemLightningStaff, itemHolyStaff, itemDarkStaff, itemHealingStaff,

            // Armor
            itemHood, itemShirt, itemCloak, itemPants, itemShoes,
            itemCap, itemTunic, itemStuddedPants, itemBoots,
            itemHelm, itemPlate, itemGreaves, itemSabatons }
        );

        _itemService.Commit();
    }
    private void GenerateCharacters()
    {
        Unit unit = new Fighter
        {
            Name = "John, Brave",
            Class = "Fighter",
            Level = 1
        };
        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Iron Sword"), EquipmentSlot.Weapon);
        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Leather Tunic"), EquipmentSlot.Chest);
        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Potion"));
        unit.Stat = new Stat
        {
            HitPoints = 28,
            MaxHitPoints = 28,
            Movement = 5,
            Constitution = 7,
            Strength = 10,
            Magic = 6,
            Dexterity = 7,
            Speed = 7,
            Luck = 8,
            Defense = 6,
            Resistance = 2
        };
        unit.CurrentRoom = GetRandomRoom();

        _unitService.Add(unit);
        _statService.Add(unit.Stat);
        foreach (UnitItem unitItem in unit.UnitItems)
            _unitItemService.Add(unitItem);


        unit = new Wizard
        {
            Name = "Jane",
            Class = "Wizard",
            Level = 2
        };
        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Dark Staff"), EquipmentSlot.Weapon);
        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Hood"), EquipmentSlot.Head);
        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Potion"), EquipmentSlot.Weapon);
        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Book"), EquipmentSlot.Weapon);


        unit.Stat = new Stat
        {
            HitPoints = 25,
            MaxHitPoints = 25,
            Movement = 5,
            Constitution = 4,
            Strength = 5,
            Magic = 11,
            Dexterity = 8,
            Speed = 7,
            Luck = 9,
            Defense = 3,
            Resistance = 5
        };
        unit.CurrentRoom = GetRandomRoom();
        _unitService.Add(unit);
        _statService.Add(unit.Stat);
        foreach (UnitItem unitItem in unit.UnitItems)
            _unitItemService.Add(unitItem);

        unit = new Rogue
        {
            Name = "Bob, Sneaky",
            Class = "Rogue",
            Level = 3
        };
        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Lockpick"));
        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Obsidian Sword"), EquipmentSlot.Weapon);
        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Pants"), EquipmentSlot.Legs);
        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Shoes"), EquipmentSlot.Feet);
        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Potion"));
        unit.Abilities.Add(_abilitySteal);
        unit.Stat = new Stat
        {
            HitPoints = 26,
            MaxHitPoints = 26,
            Movement = 6,
            Constitution = 5,
            Strength = 8,
            Magic = 11,
            Dexterity = 8,
            Speed = 12,
            Luck = 12,
            Defense = 9,
            Resistance = 8
        };
        unit.CurrentRoom = GetRandomRoom();
        _unitService.Add(unit);
        _statService.Add(unit.Stat);
        foreach (UnitItem unitItem in unit.UnitItems)
            _unitItemService.Add(unitItem);

        unit = new Cleric
        {
            Name = "Alice",
            Class = "Cleric",
            Level = 4
        };
        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Potion"));
        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Plate Armor"), EquipmentSlot.Chest);
        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Greaves"), EquipmentSlot.Legs);
        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Holy Staff"), EquipmentSlot.Weapon);

        unit.Abilities.Add(_abilityHeal);
        unit.Stat = new Stat
        {
            HitPoints = 27,
            MaxHitPoints = 27,
            Movement = 5,
            Constitution = 4,
            Strength = 7,
            Magic = 9,
            Dexterity = 7,
            Speed = 7,
            Luck = 10,
            Defense = 6,
            Resistance = 7
        };
        unit.CurrentRoom = GetRandomRoom();
        _unitService.Add(unit);
        _statService.Add(unit.Stat);
        foreach (UnitItem unitItem in unit.UnitItems)
            _unitItemService.Add(unitItem);

        unit = new Knight
        {
            Name = "Reginald III, Sir",
            Class = "Knight",
            Level = 5
        };
        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Potion"));
        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Steel Sword"), EquipmentSlot.Weapon);
        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Helm"), EquipmentSlot.Head);
        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Plate Armor"), EquipmentSlot.Chest);
        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Greaves"), EquipmentSlot.Legs);
        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Sabatons"), EquipmentSlot.Feet);
        unit.Abilities.Add(_abilityTaunt);
        unit.Stat = new Stat
        {
            HitPoints = 30,
            MaxHitPoints = 30,
            Movement = 4,
            Constitution = 10,
            Strength = 10,
            Magic = 9,
            Dexterity = 7,
            Speed = 5,
            Luck = 10,
            Defense = 13,
            Resistance = 5
        };
        unit.CurrentRoom = GetRandomRoom();
        _unitService.Add(unit);
        _statService.Add(unit.Stat);
        foreach (UnitItem unitItem in unit.UnitItems)
            _unitItemService.Add(unitItem);

        unit = new EnemyGhost
        {
            Name = "Poltergeist",
            Class = "Ghost",
            Level = 1
        };
        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Iron Axe"), EquipmentSlot.Weapon);
        unit.Stat = new Stat
        {
            HitPoints = 25,
            MaxHitPoints = 25,
            Movement = 5,
            Constitution = 3,
            Strength = 8,
            Magic = 6,
            Dexterity = 7,
            Speed = 8,
            Luck = 8,
            Defense = 5,
            Resistance = 4
        };
        unit.Abilities.Add(_abilityFly);
        unit.CurrentRoom = GetRandomRoom();
        _unitService.Add(unit);
        _statService.Add(unit.Stat);
        foreach (UnitItem unitItem in unit.UnitItems)
            _unitItemService.Add(unitItem);

        unit = new EnemyGoblin
        {
            Name = "Ruthless Treasure-Gather",
            Class = "Goblin",
            Level = 2
        };
        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Iron Sword"), EquipmentSlot.Weapon);
        unit.Stat = new Stat
        {
            HitPoints = 28,
            MaxHitPoints = 28,
            Movement = 5,
            Constitution = 5,
            Strength = 9,
            Magic = 7,
            Dexterity = 7,
            Speed = 8,
            Luck = 9,
            Defense = 6,
            Resistance = 2
        };
        unit.CurrentRoom = GetRandomRoom();
        _unitService.Add(unit);
        _statService.Add(unit.Stat);
        foreach (UnitItem unitItem in unit.UnitItems)
            _unitItemService.Add(unitItem);

        unit = new EnemyArcher
        {
            Name = "Sniper",
            Class = "Archer",
            Level = 3
        };
        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Iron Bow"), EquipmentSlot.Weapon);

        unit.Stat = new Stat
        {
            HitPoints = 27,
            MaxHitPoints = 27,
            Movement = 5,
            Constitution = 6,
            Strength = 9,
            Magic = 7,
            Dexterity = 7,
            Speed = 8,
            Luck = 9,
            Defense = 6,
            Resistance = 2
        };
        unit.CurrentRoom = GetRandomRoom();
        _unitService.Add(unit);
        _statService.Add(unit.Stat);
        foreach (UnitItem unitItem in unit.UnitItems)
            _unitItemService.Add(unitItem);

        unit = new EnemyMage
        {
            Name = "Studious Spellcaster",
            Class = "Mage",
            Level = 4
        };

        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Lightning Staff"), EquipmentSlot.Weapon);
        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Potion"));
        unit.Stat = new Stat
        {
            HitPoints = 26,
            MaxHitPoints = 26,
            Movement = 5,
            Constitution = 4,
            Strength = 6,
            Magic = 10,
            Dexterity = 8,
            Speed = 9,
            Luck = 9,
            Defense = 6,
            Resistance = 7
        };
        unit.CurrentRoom = GetRandomRoom();
        _unitService.Add(unit);
        _statService.Add(unit.Stat);
        foreach (UnitItem unitItem in unit.UnitItems)
            _unitItemService.Add(unitItem);

        unit = new EnemyCleric
        {
            Name = "Doctor of the Fallen",
            Class = "Cleric",
            Level = 5
        };
        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Potion"));
        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Plate Armor"), EquipmentSlot.Chest);
        InventoryHelper.AddItemToInventory(unit, _itemService.Get("Holy Staff"), EquipmentSlot.Weapon);
        unit.Abilities.Add(_abilityHeal);
        unit.Stat = new Stat
        {
            HitPoints = 29,
            MaxHitPoints = 29,
            Movement = 5,
            Constitution = 4,
            Strength = 8,
            Magic = 11,
            Dexterity = 8,
            Speed = 8,
            Luck = 8,
            Defense = 7,
            Resistance = 6
        };
        unit.CurrentRoom = GetRandomRoom();
        _unitService.Add(unit);
        _statService.Add(unit.Stat);
        foreach (UnitItem unitItem in unit.UnitItems)
            _unitItemService.Add(unitItem);
    }

    private void GenerateAbilities()
    {
        // Generates the abilities for the game.
        _abilityService.Add(_abilityFly);
        _abilityService.Add(_abilityHeal);
        _abilityService.Add(_abilitySteal);
        _abilityService.Add(_abilityTaunt);
    }

    private void GenerateDungeons()
    {
        // Generates the dungeons and rooms for the game.
        Dungeon dungeon = new()
        {
            Name = "Intro Dungeon",
            Description = "The first dungeon in the game"
        };
        Room entrance = _roomFactory.CreateRoom("intro.entrance");
        Room jail = _roomFactory.CreateRoom("intro.jail");
        Room kitchen = _roomFactory.CreateRoom("intro.kitchen");
        Room hallway = _roomFactory.CreateRoom("intro.hallway");
        Room library = _roomFactory.CreateRoom("intro.entrance");
        Room dwelling = _roomFactory.CreateRoom("intro.dwelling");
        Room dwelling2 = _roomFactory.CreateRoom("intro.dwelling2");
        entrance.AddAdjacentRoom(jail, Direction.West);
        entrance.AddAdjacentRoom(kitchen, Direction.East);
        entrance.AddAdjacentRoom(hallway, Direction.North);
        hallway.AddAdjacentRoom(dwelling2, Direction.West);
        hallway.AddAdjacentRoom(library, Direction.East);
        hallway.AddAdjacentRoom(dwelling, Direction.North);
        _rooms.AddRange<Room>(entrance, jail, kitchen, hallway, library, dwelling, dwelling2);

        dungeon.StartingRoom = entrance;

        _dungeonService.Add(dungeon);

        foreach (Room room in _rooms)
        {
            _roomService.Add(room);
        }
    }

    private Room GetRandomRoom()
    {
        // Returns a random room from the list of rooms.
        Random numberGenerator = new();
        int random = numberGenerator.Next(0, 7);
        return _rooms[random];
    }

    private void DisplaySeedProgressBar()
    {
        // Displays a progress bar while the database is being seeded. The progress bar is displayed using the
        // Spectre.Console library.
        AnsiConsole.Progress()
        .AutoRefresh(true)
        .AutoClear(false)
        .HideCompleted(false)
        .Columns(new ProgressColumn[]
        {
            new TaskDescriptionColumn(),
            new ProgressBarColumn(),
            new PercentageColumn(),
            new RemainingTimeColumn(),
            new SpinnerColumn(),
            new DownloadedColumn(),
            new TransferSpeedColumn(),
        })
        .Start(ctx =>
        {
            double progress = 55;
            ProgressTask taskTotal = ctx.AddTask("[white][[Seeding Database]][/]", true, 21716);
            ProgressTask taskItems = ctx.AddTask("[white]Generating Items[/]", true, 8362);
            ProgressTask taskRooms = ctx.AddTask("[white]Generating Rooms[/]", true, 1091);
            ProgressTask taskDungeon = ctx.AddTask("[white]Generating Dungeon[/]", true, 850);
            ProgressTask taskAbilities = ctx.AddTask("[white]Generating Abilties[/]", true, 159);
            ProgressTask taskUnits = ctx.AddTask("[white]Generating Units[/]", true, 7643);
            ProgressTask taskStats = ctx.AddTask("[white]Generating Stats[/]", true, 2410);
            ProgressTask taskInventory = ctx.AddTask("[white]Generating Inventory[/]", true, 1201);

            while (!ctx.IsFinished)
            {
                Thread.Sleep(10);
                taskTotal.Increment(progress * 1.30);
                if (!taskItems.IsFinished)
                {
                    taskItems.Increment(progress);
                    taskRooms.Increment(progress / 2);
                    taskDungeon.Increment(progress / 4);
                }
                else if (!taskRooms.IsFinished)
                {
                    taskRooms.Increment(progress);
                    taskDungeon.Increment(progress / 2);
                    taskAbilities.Increment(progress / 4);
                }
                else if (!taskDungeon.IsFinished)
                {
                    taskDungeon.Increment(progress);
                    taskAbilities.Increment(progress/2);
                    taskUnits.Increment(progress/4);
                }
                else if (!taskAbilities.IsFinished)
                {
                    taskAbilities.Increment(progress);
                    taskUnits.Increment(progress/2);
                    taskStats.Increment(progress/4);
                }
                else if (!taskUnits.IsFinished)
                {
                    taskUnits.Increment(progress);
                    taskStats.Increment(progress/2);
                    taskInventory.Increment(progress/4);
                }
                else if (!taskStats.IsFinished)
                {
                    taskStats.Increment(progress);
                    taskInventory.Increment(progress/2);
                }
                else if (!taskInventory.IsFinished)
                {
                    taskInventory.Increment(progress);
                }
            }
        });
    }
}