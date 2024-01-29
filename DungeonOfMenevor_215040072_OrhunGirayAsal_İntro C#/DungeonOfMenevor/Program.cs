using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Welcome to the DungeonOfMenevor!");
        Console.Write("Enter your name: ");
        string playerName = Console.ReadLine();

        Console.WriteLine($"Hello, {playerName}! Let's begin your adventure.");

        int[] playerPos = { 0, 0 };
        bool hasItem = false;
        int maxTries = 2;
        bool doorTalked = false;

        while (true)
        {
            PrintBoard(playerPos);

            if (playerPos[0] == 2 && playerPos[1] == 3 && !hasItem)
            {
                Console.WriteLine("You found a Stone Tablet.");
                Console.Write("Do you want to take it? (yes/no): ");
                string takeItem = Console.ReadLine().ToLower();

                if (takeItem == "yes")
                {
                    hasItem = true;
                    Console.WriteLine("You took the Stone Tablet with a picture of an Egg.");
                }
                else
                {
                    Console.WriteLine("You left the Stone Tablet.");
                }
            }
            else if (playerPos[0] == 3 && playerPos[1] == 3 && !doorTalked)
            {
                Console.WriteLine("You encounter a Talking Door. What do you want to do?");
                Console.WriteLine("1. Talk to the door");
                Console.WriteLine("2. Ignore the door");

                Console.Write("Choose an option (1 or 2): ");
                string doorOption = Console.ReadLine();

                switch (doorOption)
                {
                    case "1":
                        Console.WriteLine("The Talking Door speaks: 'I have a riddle for you. Answer correctly, and the way forward opens.'");
                        Console.WriteLine("Riddle Options:");

                        // Create riddle options array
                        string[] riddleOptions = GetRiddleOptions(hasItem);

                        // Display shuffled options
                        for (int i = 0; i < riddleOptions.Length; i++)
                        {
                            Console.WriteLine($"{i + 1}. {riddleOptions[i]}");
                        }

                        bool correctAnswer = false;

                        for (int tries = maxTries; tries > 0; tries--)
                        {
                            Console.Write($"Your answer ({tries} tries remaining): ");
                            string doorAnswer = Console.ReadLine().Trim().ToLower();

                            if (doorAnswer == "1" && hasItem && riddleOptions[0] == "An egg")
                            {
                                Console.WriteLine("The door says: 'Correct! You win.'");
                                doorTalked = true;
                                correctAnswer = true;
                                break;
                                return;
                            }
                            else
                            {
                                if (tries > 1)
                                {
                                    Console.WriteLine("The door says: 'Incorrect! Try again.'");
                                }
                                else
                                {
                                    Console.WriteLine("The door says: 'Out of tries. You have died.'");
                                }
                            }
                        }

                        if (!correctAnswer)
                        {
                            // End the game if the player answered incorrectly and ran out of tries.
                            Console.WriteLine("Game over. You have died.");
                            return;
                        }
                        break;
                    case "2":
                        Console.WriteLine("You decided not to talk to the door.");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }

            Console.Write("Available moves: ");
            if (playerPos[0] > 0)
                Console.Write("'north' ");
            if (playerPos[0] < 4)
                Console.Write("'south' ");
            if (playerPos[1] > 0)
                Console.Write("'west' ");
            if (playerPos[1] < 4)
                Console.Write("'east' ");

            Console.WriteLine("\nType the direction to move: ");
            string move = Console.ReadLine().ToLower();

            bool isValidMove = false;

            switch (move)
            {
                case "north" when playerPos[0] > 0:
                    playerPos[0]--;
                    isValidMove = true;
                    break;
                case "south" when playerPos[0] < 4:
                    playerPos[0]++;
                    isValidMove = true;
                    break;
                case "west" when playerPos[1] > 0:
                    playerPos[1]--;
                    isValidMove = true;
                    break;
                case "east" when playerPos[1] < 4:
                    playerPos[1]++;
                    isValidMove = true;
                    break;
                default:
                    Console.WriteLine("Invalid move. Try again.");
                    break;
            }

            if (isValidMove)
            {
                Console.WriteLine($"You moved {move}.");
            }
        }
    }

    static void PrintBoard(int[] playerPos)
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (i == playerPos[0] && j == playerPos[1])
                {
                    Console.Write("P | ");
                }
                else
                {
                    Console.Write("  | ");
                }
            }
            Console.WriteLine("\n---------------------");
        }
    }

    static string[] ShuffleOptions(params string[] options)
    {
        Random random = new Random();
        int n = options.Length;
        while (n > 1)
        {
            int k = random.Next(n--);
            string temp = options[n];
            options[n] = options[k];
            options[k] = temp;
        }
        return options;
    }

    static string[] GetRiddleOptions(bool hasItem)
    {
        string[] riddleOptions;
        if (hasItem)
        {
            // If the player has the Stone Tablet, include the "An egg" option
            riddleOptions = ShuffleOptions("An egg", "A Wood", "A Door", "A Lightstick", "A Leaf");
        }
        else
        {
            // If the player doesn't have the Stone Tablet, exclude the "An egg" option
            riddleOptions = ShuffleOptions("A Wood", "A Door", "A Lightstick", "A Leaf");
        }
        return riddleOptions;
    }
}
