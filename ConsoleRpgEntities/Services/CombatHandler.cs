using Spectre.Console;
using ConsoleRpgEntities.Models.UI;
using ConsoleRpgEntities.Models.Interfaces;
using ConsoleRpgEntities.Models.Interfaces.Commands;
using ConsoleRpgEntities.Services.Repositories;
using ConsoleRpgEntities.Models.Units.Abstracts;
using ConsoleRpgEntities.Models.Abilities;

namespace ConsoleRpgEntities.Services;

public class CombatHandler
{
    private readonly AbilityService _abilityService;
    private readonly CommandHandler _commandHandler;
    private readonly UnitService _unitService;
    private readonly UserInterface _userInterface;
    private bool _isPlayersTurn = true;
    public CombatHandler(AbilityService abilityService, CommandHandler commandHandler, UnitService unitService, UserInterface userInterface)
    {
        _abilityService = abilityService;
        _commandHandler = commandHandler;
        _unitService = unitService;
        _userInterface = userInterface;
    }
    public void StartCombat()
    {
        bool isPartyDead = false;
        bool isEnemyDead = false;

        List<Unit> units = _unitService.GetAll().Where(u => u.Stat.HitPoints > 0).ToList();
        List<Unit> party = units.Where(u => !u.UnitType.Contains("Enemy") && u.Stat.HitPoints > 0).ToList();
        List<Unit> enemies = units.Where(u => u.UnitType.Contains("Enemy") && u.Stat.HitPoints > 0).ToList();

        foreach (Unit unit in party)
        {
            unit.IsTurnDepleted = false;
        }

        while (!isPartyDead && !isEnemyDead)
        {
            if (_isPlayersTurn)
            {
                // Asks the user to choose a unit.
                IUnit unit = _userInterface.PartyUnitSelectionMenu.Display("Select unit to control", "[[Exit Game]]");
                if (unit == null) break;

                // If the selected unit is down, restarts
                if (unit.Stat.HitPoints <= 0 || unit.IsTurnDepleted) continue;

                // Asks the user to choose an action for unit.
                ICommand command = _userInterface.CommandMenu.Display(unit, $"Select action for {unit.Name}", "[[Go Back]]");
                if (command == null) continue; // Go back was selected, sends back to unit selection.

                // Takes the command and the unit and handles
                bool commandExecuted = _commandHandler.HandleCommand(command, unit);

                if (commandExecuted)
                    unit.IsTurnDepleted = true;

                // Waits for user input.  Escape leaves the program and any other button loops the process.
                AnsiConsole.MarkupLine($"\nPress [green][[ANY KEY]][/] to continue...");
                ConsoleKey key = Console.ReadKey(true).Key;
            }
            else
            {
                Ability heal = _abilityService.GetAll().Where(a => a.Name == "Heal").FirstOrDefault()!;

                party = units.Where(u => !u.UnitType.Contains("Enemy") && u.Stat.HitPoints > 0).ToList();
                enemies = units.Where(u => u.UnitType.Contains("Enemy")).ToList();

                foreach (Unit enemy in enemies)
                {
                    if (enemy.Stat.HitPoints <= 0) continue;
                    Console.Clear();
                    Unit randomPartyUnit = party[Random.Shared.Next(0, party.Count)];
                    Unit randomEnemyUnit = enemies[Random.Shared.Next(0, enemies.Count)];

                    if (enemy.Abilities.Contains(heal))
                    {
                        Unit mostDamagedUnit = null!;
                        foreach (Unit unit in enemies)
                        {
                            if (mostDamagedUnit == null)
                                mostDamagedUnit = unit;
                            else
                                if (unit.Stat.MaxHitPoints - unit.Stat.HitPoints > mostDamagedUnit.Stat.MaxHitPoints - mostDamagedUnit.Stat.HitPoints)
                                mostDamagedUnit = unit;
                        }
                        // Enemy heals most damaged enemy unit.
                        enemy.UseAbility(heal, mostDamagedUnit!);
                    }
                    else
                    {
                        // Enemy attacks random party unit
                        enemy.Attack(randomPartyUnit);
                    }
                    AnsiConsole.MarkupLine($"\nPress [green][[ANY KEY]][/] to continue...");
                    ConsoleKey key = Console.ReadKey(true).Key;
                }

                _isPlayersTurn = true;
                foreach (Unit unit in party)
                {
                    unit.IsTurnDepleted = false;
                }
            }

            bool hasTurnsLeft = false;
            foreach (Unit unit in party)
            {
                if (!unit.IsTurnDepleted && unit.Stat.HitPoints > 0)
                {
                    hasTurnsLeft = true;
                    break;
                }
            }

            if (!hasTurnsLeft)
            {
                _isPlayersTurn = false;
            }

            isPartyDead = true;
            foreach (Unit unit in party)
            {
                if (unit.Stat.HitPoints > 0)
                {
                    isPartyDead = false;
                    break;
                }
            }

            isEnemyDead = true;
            foreach (Unit unit in enemies)
            {
                if (unit.Stat.HitPoints > 0)
                {
                    isEnemyDead = false;
                    break;
                }
            }
        }

        Console.Clear();
        if (isEnemyDead)
        {
            AnsiConsole.MarkupLine("[green]Congratulations! You Win![/]");
            AnsiConsole.MarkupLine($"\nPress [green][[ANY KEY]][/] to continue...");
            ConsoleKey key = Console.ReadKey(true).Key;
        }

        if (isPartyDead)
        {
            AnsiConsole.MarkupLine("[red]Game Over![/]");
            AnsiConsole.MarkupLine($"\nPress [red][[ANY KEY]][/] to continue...");
            ConsoleKey key = Console.ReadKey(true).Key;
        }
    }


}
