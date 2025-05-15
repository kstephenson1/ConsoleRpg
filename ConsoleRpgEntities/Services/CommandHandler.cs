using ConsoleRpgEntities.Models.Commands.ItemCommands;
using ConsoleRpgEntities.Models.Commands.UnitCommands;
using ConsoleRpgEntities.Models.UI;
using ConsoleRpgEntities.Models.Units.Abstracts;
using ConsoleRpgEntities.Services.DataHelpers;
using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Models.Interfaces.Commands;
using ConsoleRpgEntities.Models.Interfaces.ItemBehaviors;
using ConsoleRpgEntities.Models.Interfaces.UnitBehaviors;
using ConsoleRpgEntities.Models.Commands.AbilityCommands;
using ConsoleRpgEntities.Models.Abilities;
using ConsoleRpgEntities.Services.Repositories;
using ConsoleRpgEntities.Models.Abilities.UnitAbilities;

namespace ConsoleRpgEntities.Services;

public class CommandHandler
{
    private readonly UnitItemService _unitItemService ;
    private UserInterface _userInterface;
    public CommandHandler(UnitItemService unitItemService, UserInterface userInterface)
    {
        _unitItemService = unitItemService;
        _userInterface = userInterface;
    }
    public bool HandleCommand(ICommand command, IUnit unit)
	{
        // If the unit is able to move, the unit moves.
        if (command.GetType() == typeof(MoveCommand))
        {
            unit.Move();
            return true;
        }

        // If the unit has a usable item, it can use an item.
        else if (command.GetType() == typeof(UseItemCommand))
        {
            if (unit.UnitItems!.Count > 0)
            {
                // Shows a list of items that are in the selected unit's inventory and asks the user to select an item.
                IItem item = _userInterface.InventoryMenu.Display(unit, $"Select item for {unit.Name}.", "[[Go Back]]");

                // Item is null if the user selects go back.
                if (item != null)
                {
                    // Checks the items to see what commands are allowed, displays those commands to the user and asks for a selection
                    ICommand itemCommand = _userInterface.ItemCommandMenu.Display(unit, item, $"Select action for {unit.Name} to use on {item.Name}", "[[Go Back]]");

                    // Command is null if the user selects "Go Back"
                    if (itemCommand != null)
                    {
                        // The selected command is executed by the selected unit.
                        switch (itemCommand)
                        {
                            case UnequipCommand:
                                unit.Unequip((item as IEquippableItem)!);
                                return true;
                            case EquipCommand:
                                unit.Equip((item as IEquippableItem)!);
                                return true;
                            case UseItemCommand:
                                unit.UseItem(item);
                                return true;
                            case TradeItemCommand:
                                IUnit tradeTarget = _userInterface.PartyUnitSelectionMenu.Display($"Select unit to trade {item} to.", "[[Go Back]]");
                                unit.TradeItem(item, tradeTarget, _unitItemService);
                                _unitItemService.Commit();
                                return true;
                            case DropItemCommand:
                                unit.DropItem(item);
                                return true;
                            default:
                                break;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine($"{unit.Name} has no usable items!");
            }

        }// If the unit is able to attack, it attacks.
        else if (command.GetType() == typeof(AttackCommand))
        {
            if (unit is IAttack && InventoryHelper.GetEquippedWeapon((Unit)unit) != null)
            {
                IUnit targetUnit = _userInterface.EnemyUnitSelectionMenu.Display($"Select unit being attacked by {unit.Name}", "[[Go Back]]");
                if (targetUnit != null)
                {
                    unit.Attack(targetUnit);
                    return true;
                }
            }
            else
            {
                Console.WriteLine($"{unit.Name} cannot attack because they do not have a weapon equipped!");
            }
        }
        // If the unit is able to cast spells, it casts a spell.
        else if (command.GetType() == typeof(CastCommand))
        {
            string spell = Input.GetString($"Enter name of spell being cast by {unit.Name}: ");
            ((ICastable)unit).Cast(spell);
            return true;
        }
        else if (command.GetType() == typeof(AbilityCommand))
        {
            Ability ability = _userInterface.AbilitySelectionMenu.Display(unit, "Select ability to use", "[[Go Back]]");
            IUnit target;

            if (ability is HealAbility)
                target = _userInterface.PartyUnitSelectionMenu.Display($"Select target for {ability.Name}.", "[[Go Back]]");
            else
                target = _userInterface.EnemyUnitSelectionMenu.Display($"Select target for {ability.Name}.", "[[Go Back]]");

            unit.UseAbility(ability, target);
            return true;
        }

        return false;
    }
}
