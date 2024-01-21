using System;

class Player
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Mana { get; set; }
    public int MaxMana { get; set; }
    public int Attack { get; set; }
    public int MagicDamage { get; set; }
    public int Luck { get; set; } // New stat for luck
    public bool IsBehindRock { get; set; } // New property for boss special attack

    public bool IsAlive => Health > 0;
}

class Warrior : Player
{
    public Warrior(string name)
    {
        Name = name;
        MaxHealth = 60;
        Health = MaxHealth;
        MaxMana = 10;
        Mana = MaxMana;
        Attack = 15;
        MagicDamage = 5;
        Luck = 5; // Set initial luck for the Warrior
    }

    public void BerserkerStrike(Enemy enemy)
    {
        if (Health >= 10)
        {
            Console.WriteLine($"{Name} uses Berserker Strike!");
            int damage = new Random().Next(1, Attack + 1) * 2; // 100% more damage
            enemy.Health -= damage;
            Health -= 10;
            Console.WriteLine($"{Name} loses 10 health but deals {damage} damage to {enemy.Name}.");
        }
        else
        {
            Console.WriteLine($"{Name} doesn't have enough health to use Berserker Strike. Performing a normal attack.");
            int damage = new Random().Next(1, Attack + 1);
            enemy.Health -= damage;
            Console.WriteLine($"{Name} attacks {enemy.Name} for {damage} damage.");
        }
    }
}

class Mage : Player
{
    public Mage(string name)
    {
        Name = name;
        MaxHealth = 40;
        Health = MaxHealth;
        MaxMana = 30;
        Mana = MaxMana;
        Attack = 8;
        MagicDamage = 10;
        Luck = 5; // Set initial luck for the Mage
    }

    public void CastSpell(Enemy enemy)
    {
        if (Mana >= 15)
        {
            Console.WriteLine($"{Name} casts a powerful spell!");
            int damage = new Random().Next(1, MagicDamage + 1) * 2;
            enemy.Health -= damage;
            Mana -= 15;
            Console.WriteLine($"{Name} uses 15 mana but deals {damage} damage to {enemy.Name}.");
        }
        else
        {
            Console.WriteLine($"{Name} doesn't have enough mana to cast a spell. Performing a normal attack.");
            int damage = new Random().Next(1, Attack + 1);
            enemy.Health -= damage;
            Console.WriteLine($"{Name} attacks {enemy.Name} for {damage} damage.");
        }
    }
}

class Enemy
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Attack { get; set; }

    public bool IsAlive => Health > 0;
}

class Boss : Enemy
{
    public int TurnsUntilSpecialAttack { get; set; }
    public bool IsSpecialAttackCharging { get; set; }

    public void PerformSpecialAttack(Player player)
    {
        if (TurnsUntilSpecialAttack == 0 && IsSpecialAttackCharging)
        {
            Console.WriteLine("The boss releases a devastating magic attack!");

            // Check if the player is behind a rock
            if (player.IsBehindRock)
            {
                Console.WriteLine("You hide behind a rock and evade the attack!");
            }
            else
            {
                Console.WriteLine("The magic attack hits you and you are defeated!");
                player.Health = 0;
            }

            IsSpecialAttackCharging = false;
        }
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Welcome to the DungeonOfMenevor!");

        Console.Write("Enter your character's name: ");
        string playerName = Console.ReadLine();

        Console.WriteLine("Choose your class (Warrior/Mage): ");
        string playerClass = Console.ReadLine().ToLower();

        Player player;

        switch (playerClass)
        {
            case "warrior":
                player = new Warrior(playerName);
                break;
            case "mage":
                player = new Mage(playerName);
                break;
            default:
                Console.WriteLine("Invalid class. Defaulting to Warrior.");
                player = new Warrior(playerName);
                break;
        }

        int currentRoomX = 0;
        int currentRoomY = 0;

        while (true)
        {
            Console.WriteLine($"\nYou are in room ({currentRoomX}, {currentRoomY}). Choose your direction (north/south/east/west): ");
            string direction = Console.ReadLine().ToLower();

            if (direction == "north" && currentRoomY < 5)
            {
                currentRoomY++;
            }
            else if (direction == "south" && currentRoomY > -5)
            {
                currentRoomY--;
            }
            else if (direction == "east" && currentRoomX < 5)
            {
                currentRoomX++;
            }
            else if (direction == "west" && currentRoomX > -5)
            {
                currentRoomX--;
            }
            else
            {
                Console.WriteLine("You can't go in that direction. Try another direction.");
                continue;
            }

            if (new Random().Next(1, 31) == 30) // 1 in 30 chance to encounter a health shrine
            {
                Console.WriteLine("You found a health shrine! Restoring health and mana.");
                player.Health = player.MaxHealth;
                player.Mana = player.MaxMana;
            }
            else // Trap room or enemy room
            {
                if (new Random().Next(1, 7) == 6) // 1 in 6 chance for a trap
                {
                    TrapRoom(player);
                }
                else
                {
                    Enemy enemy = GenerateEnemy();
                    Combat(player, enemy);
                }
            }
        }
    }

    static Enemy GenerateEnemy()
    {
        string[] enemyNames = { "Goblin", "Orc", "Troll", "Dragon" };
        string enemyName = enemyNames[new Random().Next(0, enemyNames.Length)];
        int enemyHealth = new Random().Next(30, 61);
        int enemyAttack = new Random().Next(8, 18);
        return new Enemy { Name = enemyName, Health = enemyHealth, MaxHealth = enemyHealth, Attack = enemyAttack };
    }

    static void PrintInfo(Player player, Enemy enemy)
    {
        Console.WriteLine($"\n{player.Name} ({player.GetType().Name}) - Health: {player.Health}/{player.MaxHealth} | Mana: {player.Mana}/{player.MaxMana}");
        Console.WriteLine($"{enemy.Name} - Health: {enemy.Health}/{enemy.MaxHealth}\n");
    }

    static void Combat(Player player, Enemy enemy)
    {
        Console.WriteLine($"A wild {enemy.Name} appears!");

        while (player.IsAlive && enemy.IsAlive)
        {
            PrintInfo(player, enemy);

            Console.Write("Choose your action (attack/skill): ");
            string action = Console.ReadLine().ToLower();

            switch (action)
            {
                case "attack":
                    int playerAttack = new Random().Next(1, player.Attack + 1);
                    Console.WriteLine($"{player.Name} attacks {enemy.Name} for {playerAttack} damage.");
                    enemy.Health -= playerAttack;
                    break;
                case "skill":
                    if (player is Warrior warrior)
                    {
                        warrior.BerserkerStrike(enemy);
                    }
                    else if (player is Mage mage)
                    {
                        mage.CastSpell(enemy);
                    }
                    break;
                default:
                    Console.WriteLine("Invalid action. Performing a normal attack.");
                    int defaultAttack = new Random().Next(1, player.Attack + 1);
                    Console.WriteLine($"{player.Name} attacks {enemy.Name} for {defaultAttack} damage.");
                    enemy.Health -= defaultAttack;
                    break;
            }

            if (!enemy.IsAlive)
            {
                Console.WriteLine($"You defeated {enemy.Name}!");
                break;
            }

            int enemyAttack = new Random().Next(1, enemy.Attack + 1);
            Console.WriteLine($"{enemy.Name} attacks {player.Name} for {enemyAttack} damage.");
            player.Health -= enemyAttack;

            if (!player.IsAlive)
            {
                Console.WriteLine($"{player.Name} has been defeated.");
                break;
            }
        }
    }

    static void TrapRoom(Player player)
    {
        Console.WriteLine("You entered a trap room!");

        // Two options with stat checks
        Console.WriteLine("Choose an option:");
        Console.WriteLine("1. Try to disarm the trap (requires luck)");
        Console.WriteLine("2. Dodge the trap (requires agility)");

        int option = int.Parse(Console.ReadLine());

        bool success = false;
        int roll = new Random().Next(1, 101); // Roll a number between 1 and 100

        switch (option)
        {
            case 1: // Disarm the trap (requires luck)
                Console.WriteLine($"{player.Name} attempts to disarm the trap...");

                if (roll <= player.Luck * 2) // Adjust the percentage as needed
                {
                    Console.WriteLine($"Success! {player.Name} disarms the trap.");
                    success = true;
                }
                else
                {
                    Console.WriteLine($"Failure! The trap activates.");
                }
                break;

            case 2: // Dodge the trap (requires agility)
                Console.WriteLine($"{player.Name} attempts to dodge the trap...");

                if (roll <= player.Luck * 2) // Adjust the percentage as needed
                {
                    Console.WriteLine($"Success! {player.Name} dodges the trap.");
                    success = true;
                }
                else
                {
                    Console.WriteLine($"Failure! The trap activates.");
                }
                break;

            default:
                Console.WriteLine("Invalid option. The trap activates.");
                break;
        }

        if (!success)
        {
            int trapDamage = new Random().Next(10, 21);
            player.Health -= trapDamage;

            Console.WriteLine($"You triggered the trap and take {trapDamage} damage.");

            if (player.Health <= 0)
            {
                Console.WriteLine($"{player.Name} has been defeated by the trap. Game Over!");
                Environment.Exit(0);
            }
        }

    }
}

