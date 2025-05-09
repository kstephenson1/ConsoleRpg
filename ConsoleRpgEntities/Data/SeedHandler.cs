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

    private List<Room> _rooms = new();

    private readonly FlyAbility _abilityFly = new();
    private readonly HealAbility _abilityHeal = new();
    private readonly StealAbility _abilitySteal = new();
    private readonly TauntAbility _abilityTaunt = new();

    private ItemPotion _itemPotion;
    private ItemBook _itemBook;
    private ItemLockpick _itemLockpick;

    private PhysicalWeaponItem _itemWoodenSword;
    private PhysicalWeaponItem _itemIronSword;
    private PhysicalWeaponItem _itemBronzeSword;
    private PhysicalWeaponItem _itemSteelSword;
    private PhysicalWeaponItem _itemSilverSword;
    private PhysicalWeaponItem _itemObsidianSword;
    private PhysicalWeaponItem _itemTitaniumSword;
    private PhysicalWeaponItem _itemCobaltSword;
    private PhysicalWeaponItem _itemInconelSword;
    private PhysicalWeaponItem _itemDarkSteelSword;
    private PhysicalWeaponItem _itemStarMetalSword;
    private PhysicalWeaponItem _itemTungstenSword;

    private PhysicalWeaponItem _itemWoodenAxe;
    private PhysicalWeaponItem _itemIronAxe;
    private PhysicalWeaponItem _itemBronzeAxe;
    private PhysicalWeaponItem _itemSteelAxe;
    private PhysicalWeaponItem _itemSilverAxe;
    private PhysicalWeaponItem _itemObsidianAxe;
    private PhysicalWeaponItem _itemTitaniumAxe;
    private PhysicalWeaponItem _itemCobaltAxe;
    private PhysicalWeaponItem _itemInconelAxe;
    private PhysicalWeaponItem _itemDarkSteelAxe;
    private PhysicalWeaponItem _itemStarMetalAxe;
    private PhysicalWeaponItem _itemTungstenAxe;

    private PhysicalWeaponItem _itemWoodenLance;
    private PhysicalWeaponItem _itemIronLance;
    private PhysicalWeaponItem _itemBronzeLance;
    private PhysicalWeaponItem _itemSteelLance;
    private PhysicalWeaponItem _itemSilverLance;
    private PhysicalWeaponItem _itemObsidianLance;
    private PhysicalWeaponItem _itemTitaniumLance;
    private PhysicalWeaponItem _itemCobaltLance;
    private PhysicalWeaponItem _itemInconelLance;
    private PhysicalWeaponItem _itemDarkSteelLance;
    private PhysicalWeaponItem _itemStarMetalLance;
    private PhysicalWeaponItem _itemTungstenLance;

    private PhysicalWeaponItem _itemWoodenBow;
    private PhysicalWeaponItem _itemIronBow;
    private PhysicalWeaponItem _itemBronzeBow;
    private PhysicalWeaponItem _itemSteelBow;
    private PhysicalWeaponItem _itemSilverBow;
    private PhysicalWeaponItem _itemObsidianBow;
    private PhysicalWeaponItem _itemTitaniumBow;
    private PhysicalWeaponItem _itemCobaltBow;
    private PhysicalWeaponItem _itemInconelBow;
    private PhysicalWeaponItem _itemDarkSteelBow;
    private PhysicalWeaponItem _itemStarMetalBow;
    private PhysicalWeaponItem _itemTungstenBow;

    private MagicWeaponItem _itemFireStaff;
    private MagicWeaponItem _itemIceStaff;
    private MagicWeaponItem _itemWindStaff;
    private MagicWeaponItem _itemLightningStaff;
    private MagicWeaponItem _itemHolyStaff;
    private MagicWeaponItem _itemDarkStaff;
    private MagicWeaponItem _itemHealingStaff;

    private HeadArmorItem _itemHood;
    private ChestArmorItem _itemShirt;
    private ChestArmorItem _itemCloak;
    private LegArmorItem _itemPants;
    private FeetArmorItem _itemShoes;

    private HeadArmorItem _itemCap;
    private ChestArmorItem _itemTunic;
    private LegArmorItem _itemStuddedPants;
    private FeetArmorItem _itemBoots;

    private HeadArmorItem _itemHelm;
    private ChestArmorItem _itemPlate;
    private LegArmorItem _itemGreaves;
    private FeetArmorItem _itemSabatons;

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
        _itemPotion = new();
        _itemLockpick = new();
        _itemBook = new();

        // Physical Weapons
        _itemWoodenSword = new("Wooden Sword", "A sword made of wood, usually for training.", WeaponType.Sword, Rank.E, 12, 1, 100, 0, 1, 1, 1);
        _itemIronSword = new("Iron Sword", "A common sword made of Iron.", WeaponType.Sword, Rank.E, 30, 5, 90, 0, 1, 4, 1);
        _itemBronzeSword = new("Bronze Sword", "A common sword made of Bronze.", WeaponType.Sword, Rank.E, 36, 6, 85, 0, 1, 4, 1);
        _itemSteelSword = new("Steel Sword", "A sword that weighs more and hits harder than iron.", WeaponType.Sword, Rank.E, 48, 8, 80, 0, 1, 5, 1);
        _itemSilverSword = new("Silver Sword", "A heavy, brittle sword that hits hard.", WeaponType.Sword, Rank.E, 18, 11, 60, 10, 1, 6, 1);
        _itemObsidianSword = new("Obsidian Sword", "A lightweight but brittle blade that can do some serious damage.", WeaponType.Sword, Rank.E, 24, 2, 90, 30, 1, 4, 1);
        _itemTitaniumSword = new("Titanium Sword", "A very lightweight sword that yields a sharp blade.", WeaponType.Sword, Rank.E, 42, 3, 100, 5, 1, 8, 1);
        _itemCobaltSword = new("Cobalt Sword", "A beautiful blue sword forged of cobalt.", WeaponType.Sword, Rank.E, 48, 5, 80, 0, 1, 10, 1);
        _itemInconelSword = new("Inconel Sword", "A shiny and durable sword made of a nickel alloy.", WeaponType.Sword, Rank.E, 54, 4, 75, 0, 1, 13, 1);
        _itemDarkSteelSword = new("Dark Steel Sword", "A sword forged by volcanic heat that is rivaled by few.", WeaponType.Sword, Rank.E, 54, 4, 70, 10, 1, 14, 1);
        _itemStarMetalSword = new("Star Metal Sword", "A dependable sword only used by the best, and richest, of fighters.", WeaponType.Sword, Rank.E, 60, 4, 65, 0, 1, 15, 1);
        _itemTungstenSword = new("Tungsten Sword", "A heavy hitting weapon that is almost impossible to wield.", WeaponType.Sword, Rank.E, 60, 10, 40, 10, 1, 20, 1);

        _itemWoodenAxe = new("Wooden Axe", "A axe made of wood, usually for training.", WeaponType.Axe, Rank.E, 12, 1, 100, 0, 1, 1, 1);
        _itemIronAxe = new("Iron Axe", "A common axe made of Iron.", WeaponType.Axe, Rank.E, 30, 4, 90, 0, 1, 8, 1);
        _itemBronzeAxe = new("Bronze Axe", "A common axe made of Bronze.", WeaponType.Axe, Rank.E, 36, 4, 85, 0, 1, 9, 1);
        _itemSteelAxe = new("Steel Axe", "A axe that weighs more and hits harder than iron.", WeaponType.Axe, Rank.E, 48, 5, 80, 0, 1, 11, 1);
        _itemSilverAxe = new("Silver Axe", "A heavy, brittle axe that hits hard.", WeaponType.Axe, Rank.E, 18, 6, 60, 10, 1, 14, 1);
        _itemObsidianAxe = new("Obsidian Axe", "A lightweight but brittle axe that can do some serious damage.", WeaponType.Axe, Rank.E, 24, 2, 90, 30, 1, 7, 1);
        _itemTitaniumAxe = new("Titanium Axe", "A very lightweight axe that yields a sharp blade.", WeaponType.Axe, Rank.E, 42, 3, 100, 5, 1, 11, 1);
        _itemCobaltAxe = new("Cobalt Axe", "A beautiful blue axe forged of cobalt.", WeaponType.Axe, Rank.E, 48, 5, 80, 0, 1, 13, 1);
        _itemInconelAxe = new("Inconel Axe", "A shiny and durable axe made of a nickel alloy.", WeaponType.Axe, Rank.E, 54, 4, 75, 0, 1, 16, 1);
        _itemDarkSteelAxe = new("Dark Steel Axe", "A axe forged by volcanic heat that is rivaled by few.", WeaponType.Axe, Rank.E, 54, 4, 70, 10, 1, 17, 1);
        _itemStarMetalAxe = new("Star Metal Axe", "A dependable axe only used by the best, and richest, of fighters.", WeaponType.Axe, Rank.E, 60, 4, 65, 0, 1, 18, 1);
        _itemTungstenAxe = new("Tungsten Axe", "A heavy hitting weapon that is almost impossible to wield.", WeaponType.Axe, Rank.E, 60, 10, 40, 10, 1, 23, 1);

        _itemWoodenLance = new("Wooden Lance", "A lance made of wood, usually for training.", WeaponType.Lance, Rank.E, 12, 1, 100, 0, 1, 1, 1);
        _itemIronLance = new("Iron Lance", "A common lance made of Iron.", WeaponType.Lance, Rank.E, 30, 4, 90, 0, 1, 6, 1);
        _itemBronzeLance = new("Bronze Lance", "A common lance made of Bronze.", WeaponType.Lance, Rank.E, 36, 4, 85, 0, 1, 7, 1);
        _itemSteelLance = new("Steel Lance", "A lance that weighs more and hits harder than iron.", WeaponType.Lance, Rank.E, 48, 5, 80, 0, 1, 9, 1);
        _itemSilverLance = new("Silver Lance", "A heavy, brittle lance that hits hard.", WeaponType.Lance, Rank.E, 18, 6, 60, 10, 1, 12, 1);
        _itemObsidianLance = new("Obsidian Lance", "A lightweight but brittle lance that can do some serious damage.", WeaponType.Lance, Rank.E, 24, 2, 90, 30, 1, 5, 1);
        _itemTitaniumLance = new("Titanium Lance", "A very lightweight lance that yields a sharp blade.", WeaponType.Lance, Rank.E, 42, 3, 100, 5, 1, 9, 1);
        _itemCobaltLance = new("Cobalt Lance", "A beautiful blue lance forged of cobalt.", WeaponType.Lance, Rank.E, 48, 5, 80, 0, 1, 11, 1);
        _itemInconelLance = new("Inconel Lance", "A shiny and durable lance made of a nickel alloy.", WeaponType.Lance, Rank.E, 54, 4, 75, 0, 1, 14, 1);
        _itemDarkSteelLance = new("Dark Steel Lance", "A lance forged by volcanic heat that is rivaled by few.", WeaponType.Lance, Rank.E, 54, 4, 70, 10, 1, 15, 1);
        _itemStarMetalLance = new("Star Metal Lance", "A dependable lance only used by the best, and richest, of fighters.", WeaponType.Lance, Rank.E, 60, 4, 65, 0, 1, 16, 1);
        _itemTungstenLance = new("Tungsten Lance", "A heavy hitting weapon that is almost impossible to wield.", WeaponType.Lance, Rank.E, 60, 10, 40, 10, 1, 21, 1);

        _itemWoodenBow = new("Wooden Bow", "A bow made of wood, usually for target practice.", WeaponType.Bow, Rank.E, 12, 1, 100, 0, 2, 1, 1);
        _itemIronBow = new("Iron Bow", "A common bow made of Iron.", WeaponType.Bow, Rank.E, 30, 4, 90, 0, 2, 6, 1);
        _itemBronzeBow = new("Bronze Bow", "A common bow made of Bronze.", WeaponType.Bow, Rank.E, 36, 4, 85, 0, 2, 7, 1);
        _itemSteelBow = new("Steel Bow", "A bow that weighs more and hits harder than iron.", WeaponType.Bow, Rank.E, 48, 5, 80, 0, 2, 9, 1);
        _itemSilverBow = new("Silver Bow", "A heavy, brittle bow that hits hard.", WeaponType.Bow, Rank.E, 18, 6, 60, 10, 2, 12, 1);
        _itemObsidianBow = new("Obsidian Bow", "A lightweight but brittle bow that can do some serious damage.", WeaponType.Bow, Rank.E, 24, 2, 90, 30, 2, 5, 1);
        _itemTitaniumBow = new("Titanium Bow", "A very lightweight bow that yields a sharp blade.", WeaponType.Bow, Rank.E, 42, 3, 100, 5, 2, 9, 1);
        _itemCobaltBow = new("Cobalt Bow", "A beautiful blue bow forged of cobalt.", WeaponType.Bow, Rank.E, 48, 5, 80, 0, 2, 11, 1);
        _itemInconelBow = new("Inconel Bow", "A shiny and durable bow made of a nickel alloy.", WeaponType.Bow, Rank.E, 54, 4, 75, 0, 2, 14, 1);
        _itemDarkSteelBow = new("Dark Steel Bow", "A bow forged by volcanic heat that is rivaled by few.", WeaponType.Bow, Rank.E, 54, 4, 70, 10, 2, 15, 1);
        _itemStarMetalBow = new("Star Metal Bow", "A dependable bow only used by the best, and richest, of fighters.", WeaponType.Bow, Rank.E, 60, 4, 65, 0, 2, 16, 1);
        _itemTungstenBow = new("Tungsten Bow", "A bow that is almost impossible to wield but shoots with great strength.", WeaponType.Bow, Rank.E, 60, 10, 40, 10, 2, 21, 1);

        // Magic Weapons
        _itemFireStaff = new("Fire Staff", "A staff that stays on fire, some how.", WeaponType.Elemental, Rank.E, 30, 2, 85, 0, 1, 1, 1);
        _itemIceStaff = new("Ice Staff", "A staff encased in a layer of frost.", WeaponType.Elemental, Rank.E, 30, 2, 75, 0, 1, 1, 1);
        _itemWindStaff = new("Wind Staff", "A staff surrounded by a mysterious breeze.", WeaponType.Elemental, Rank.E, 30, 2, 90, 10, 1, 1, 1);
        _itemLightningStaff = new("Lightning Staff", "A staff full of discharging static.", WeaponType.Elemental, Rank.E, 30, 2, 80, 5, 1, 1, 1);
        _itemHolyStaff = new("Holy Staff", "A staff filled with the holy word.", WeaponType.Light, Rank.E, 30, 2, 100, 15, 1, 1, 1);
        _itemDarkStaff = new("Dark Staff", "A staff surrounded with vile intentions.", WeaponType.Dark, Rank.E, 30, 2, 70, 0, 1, 1, 1);
        _itemHealingStaff = new("Healing Staff", "A staff containing healing magic.", WeaponType.Heal, Rank.E, 30, 2, 100, 0, 1, 1, 1);

        _itemHood = new("Hood", "A basic hood", ArmorType.Head, Rank.E, 30, 0, 2, 1, 1);
        _itemShirt = new("Shirt", "A basic shirt", ArmorType.Chest, Rank.E, 30, 0, 2, 1, 1);
        _itemCloak = new("Cloak", "A basic cloak", ArmorType.Chest, Rank.E, 30, 0, 2, 1, 1);
        _itemPants = new("Pants", "A basic pants", ArmorType.Legs, Rank.E, 30, 0, 2, 1, 1);
        _itemShoes = new("Shoes", "A basic shoes", ArmorType.Feet, Rank.E, 30, 0, 2, 1, 1);
        _itemCap = new("Leather Cap", "A basic leather cap", ArmorType.Head, Rank.E, 30, 1, 1, 2, 1);
        _itemTunic = new("Leather Tunic", "A basic leather tunic", ArmorType.Chest, Rank.E, 30, 1, 1, 2, 1);
        _itemStuddedPants = new("Studded Pants", "A basic studded pants", ArmorType.Legs, Rank.E, 30, 1, 1, 2, 1);
        _itemBoots = new("Leather Boots", "A basic leather boots", ArmorType.Feet, Rank.E, 30, 1, 1, 2, 1);
        _itemHelm = new("Helm", "A basic plate helm", ArmorType.Head, Rank.E, 30, 2, 0, 3, 1);
        _itemPlate = new("Plate Armor", "A basic plate armor", ArmorType.Chest, Rank.E, 30, 2, 0, 3, 1);
        _itemGreaves = new("Greaves", "A basic plate greaves", ArmorType.Legs, Rank.E, 30, 2, 0, 3, 1);
        _itemSabatons = new("Sabatons", "A basic plate sabatons", ArmorType.Feet, Rank.E, 30, 2, 0, 3, 1);

        _itemService.Add( new List<Item>() {
            // Consumable Items
            _itemPotion, _itemLockpick, _itemBook,

            // Physical Weapons
            _itemWoodenSword, _itemIronSword, _itemBronzeSword, _itemSteelSword, _itemSilverSword, _itemObsidianSword, _itemTitaniumSword, _itemCobaltSword, _itemInconelSword, _itemDarkSteelSword, _itemStarMetalSword, _itemTungstenSword,
            _itemWoodenAxe, _itemIronAxe, _itemBronzeAxe, _itemSteelAxe, _itemSilverAxe, _itemObsidianAxe, _itemTitaniumAxe, _itemCobaltAxe, _itemInconelAxe, _itemDarkSteelAxe, _itemStarMetalAxe, _itemTungstenAxe,
            _itemWoodenLance, _itemIronLance, _itemBronzeLance, _itemSteelLance, _itemSilverLance, _itemObsidianLance, _itemTitaniumLance, _itemCobaltLance, _itemInconelLance, _itemDarkSteelLance, _itemStarMetalLance, _itemTungstenLance,
            _itemWoodenBow, _itemIronBow, _itemBronzeBow, _itemSteelBow, _itemSilverBow, _itemObsidianBow, _itemTitaniumBow, _itemCobaltBow, _itemInconelBow, _itemDarkSteelBow, _itemStarMetalBow, _itemTungstenBow,
            
            // Magic Weapons
            _itemFireStaff, _itemIceStaff, _itemWindStaff, _itemLightningStaff, _itemHolyStaff, _itemDarkStaff, _itemHealingStaff,

            // Armor
            _itemHood, _itemShirt, _itemCloak, _itemPants, _itemShoes,
            _itemCap, _itemTunic, _itemStuddedPants, _itemBoots,
            _itemHelm, _itemPlate, _itemGreaves, _itemSabatons }
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
        InventoryHelper.AddItemToInventory(unit, _itemIronSword, EquipmentSlot.Weapon);
        InventoryHelper.AddItemToInventory(unit, _itemTunic, EquipmentSlot.Chest);
        InventoryHelper.AddItemToInventory(unit, _itemPotion);
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
        InventoryHelper.AddItemToInventory(unit, _itemDarkStaff, EquipmentSlot.Weapon);
        InventoryHelper.AddItemToInventory(unit, _itemHood, EquipmentSlot.Head);
        InventoryHelper.AddItemToInventory(unit, _itemPotion, EquipmentSlot.Weapon);
        InventoryHelper.AddItemToInventory(unit, _itemBook, EquipmentSlot.Weapon);


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
        InventoryHelper.AddItemToInventory(unit, _itemLockpick);
        InventoryHelper.AddItemToInventory(unit, _itemObsidianSword, EquipmentSlot.Weapon);
        InventoryHelper.AddItemToInventory(unit, _itemPants, EquipmentSlot.Legs);
        InventoryHelper.AddItemToInventory(unit, _itemShoes, EquipmentSlot.Feet);
        InventoryHelper.AddItemToInventory(unit, _itemPotion);
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
        InventoryHelper.AddItemToInventory(unit, _itemPotion);
        InventoryHelper.AddItemToInventory(unit, _itemPlate, EquipmentSlot.Chest);
        InventoryHelper.AddItemToInventory(unit, _itemGreaves, EquipmentSlot.Legs);
        InventoryHelper.AddItemToInventory(unit, _itemHolyStaff, EquipmentSlot.Weapon);

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
        InventoryHelper.AddItemToInventory(unit, _itemPotion);
        InventoryHelper.AddItemToInventory(unit, _itemSteelSword, EquipmentSlot.Weapon);
        InventoryHelper.AddItemToInventory(unit, _itemHelm, EquipmentSlot.Head);
        InventoryHelper.AddItemToInventory(unit, _itemPlate, EquipmentSlot.Chest);
        InventoryHelper.AddItemToInventory(unit, _itemGreaves, EquipmentSlot.Legs);
        InventoryHelper.AddItemToInventory(unit, _itemSabatons, EquipmentSlot.Feet);
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
        InventoryHelper.AddItemToInventory(unit, _itemIronAxe, EquipmentSlot.Weapon);
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
        InventoryHelper.AddItemToInventory(unit, _itemIronSword, EquipmentSlot.Weapon);
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
        InventoryHelper.AddItemToInventory(unit, _itemIronBow, EquipmentSlot.Weapon);

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

        InventoryHelper.AddItemToInventory(unit, _itemLightningStaff, EquipmentSlot.Weapon);
        InventoryHelper.AddItemToInventory(unit, _itemPotion);
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
        InventoryHelper.AddItemToInventory(unit, _itemPotion);
        InventoryHelper.AddItemToInventory(unit, _itemPlate, EquipmentSlot.Chest);
        InventoryHelper.AddItemToInventory(unit, _itemHolyStaff, EquipmentSlot.Weapon);
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