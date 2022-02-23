//Skeleton Program for the examination
//this code should be used in conjunction with the Preliminary Material


//Version Number 1.0

using System;
using System.IO;

class Program
{
    public struct ShipType
    {
        public string Name;
        public int Size;
    }

    const string TrainingGame = "Training.txt";

    private static void GetRowColumn(ref int Row, ref int Column)
    {
        //Added input validation for use inputting coordinates
        bool cvalid = true;
        bool rvalid = true;

        Console.WriteLine();
        do
        {
            Console.Write("Please enter column: ");
            try
            {
                Column = Convert.ToInt32(Console.ReadLine());
            }
            catch
            {
                Column = -1;
            }
            if (Column < 0 || Column > 9)
            {
                cvalid = false;
                Console.WriteLine("Please enter a valid choice!");
                Console.WriteLine();
            }
            else
            {
                cvalid = true;
            }
        } while (cvalid == false);

        do
        {
            Console.Write("Please enter row: ");
            try
            {
                Row = Convert.ToInt32(Console.ReadLine());
            }
            catch
            {
                Row = -1;
            }
            if (Row < 0 || Row > 9)
            {
                rvalid = false;
                Console.WriteLine("Please enter a valid choice!");
                Console.WriteLine();
            }
            else
            {
                rvalid = true;
            }
        } while (rvalid == false);
        Console.WriteLine();
    }

    private static void MakePlayerMove(ref char[,] Board, ref ShipType[] Ships, ref int Aleft, ref int Bleft, ref int Sleft, ref int Dleft, ref int Pleft, ref double misses, ref double hits, ref bool GameLost)
    {
        //Added a check for if ships have been sunk.
        int Row = 0;
        int Column = 0;
        GetRowColumn(ref Row, ref Column);
        if (Board[Row, Column] == 'm' || Board[Row, Column] == 'h' || Board[Row, Column] == 'n')
        {
            Console.WriteLine("Sorry, you have already shot at the square (" + Column + "," + Row + "). Please try again.");
        }
        else if (Board[Row, Column] == '-')
        {
            bool nearmiss = false;
            for (int r = -1; r <= 1; r++)
            {
                for (int c = -1; c <= 1; c++)
                {
                    if (r == 0 && c == 0) continue;
                    try
                    {
                        switch (Board[Row + r, Column + c])
                        {
                            case '-':
                                break;
                            case 'h':
                                break;
                            case 'm':
                                break;
                            case 'n':
                                break;
                            default:
                                Console.WriteLine("Sorry, (" + Column + "," + Row + ") is a near miss.");
                                Board[Row, Column] = 'n';
                                nearmiss = true;
                                break;

                        }
                    }
                    catch
                    {
                        nearmiss = false;
                    }
                    if (nearmiss) break;
                }
                if (nearmiss) break;
            }
            if (!nearmiss)
            {
                Console.WriteLine("Sorry, (" + Column + "," + Row + ") is a miss.");
                Board[Row, Column] = 'm';
            }


            misses++;
        }
        else if(Board[Row, Column] == 'X')
        {
            GameLost = true;
            Console.WriteLine("Sorry you have lost the game by hitting a mine.\nPress any key to continue.");
            Console.ReadLine();
        }
        else
        {

            Console.WriteLine("Hit at (" + Column + "," + Row + ").");
            switch (Board[Row, Column])
            {
                case 'A':
                    Aleft--;
                    if (Aleft == 0) Console.WriteLine("You have sunk an aircraft carrier.");
                    else Console.WriteLine("You have hit an aircraft carrier.");
                    break;
                case 'B':
                    Bleft--;
                    if (Bleft == 0) Console.WriteLine("You have sunk a battleship.");
                    else Console.WriteLine("You have hit a battleship.");
                    break;
                case 'S':
                    Sleft--;
                    if (Sleft == 0) Console.WriteLine("You have sunk a submarine.");
                    else Console.WriteLine("You have hit a submarine.");
                    break;
                case 'D':
                    Dleft--;
                    if (Dleft == 0) Console.WriteLine("You have sunk a destroyer.");
                    else Console.WriteLine("You have hit a destroyer.");

                    break;
                case 'P':
                    Pleft--;
                    if (Pleft == 0) Console.WriteLine("You have sunk a patrol boat.");
                    else Console.WriteLine("You have hit a patrol boat.");

                    break;
                default:
                    break;
            }
            Board[Row, Column] = 'h';
            hits++;
        }
        double hitrate = Math.Round(((hits / (hits + misses)) * 100), 2);
        Console.WriteLine("Your hitrate is {0}%", hitrate);
    }

    private static void SetUpBoard(ref char[,] Board)
    {
        for (int Row = 0; Row < 10; Row++)
        {
            for (int Column = 0; Column < 10; Column++)
            {
                Board[Row, Column] = '-';
            }
        }
    }

    private static void LoadGame(string TrainingGame, ref char[,] Board)
    {
        string Line = "";
        StreamReader BoardFile = new StreamReader(TrainingGame);
        for (int Row = 0; Row < 10; Row++)
        {
            Line = BoardFile.ReadLine();
            for (int Column = 0; Column < 10; Column++)
            {
                Board[Row, Column] = Line[Column];
            }
        }
        BoardFile.Close();
    }

    private static void PlaceRandomShips(ref char[,] Board, ShipType[] Ships)
    {
        Random RandomNumber = new Random();
        bool Valid;
        char Orientation = ' ';
        int Row = 0;
        int Column = 0;
        int HorV = 0;
        foreach (var Ship in Ships)
        {
            Valid = false;
            while (Valid == false)
            {
                Row = RandomNumber.Next(0, 10);
                Column = RandomNumber.Next(0, 10);
                HorV = RandomNumber.Next(0, 2);
                if (HorV == 0)
                {
                    Orientation = 'v';
                }
                else
                {
                    Orientation = 'h';
                }
                Valid = ValidateBoatPosition(Board, Ship, Row, Column, Orientation);
            }
            Console.WriteLine("Computer placing the " + Ship.Name);
            PlaceShip(ref Board, Ship, Row, Column, Orientation);
        }
    }

    private static void PlaceShip(ref char[,] Board, ShipType Ship, int Row, int Column, char Orientation)
    {
        if (Orientation == 'v')
        {
            for (int Scan = 0; Scan < Ship.Size; Scan++)
            {
                Board[Row + Scan, Column] = Ship.Name[0];
            }
        }
        else if (Orientation == 'h')
        {
            for (int Scan = 0; Scan < Ship.Size; Scan++)
            {
                Board[Row, Column + Scan] = Ship.Name[0];
            }
        }
    }

    private static bool ValidateBoatPosition(char[,] Board, ShipType Ship, int Row, int Column, char Orientation)
    {
        if (Orientation == 'v' && Row + Ship.Size > 10)
        {
            return false;
        }
        else if (Orientation == 'h' && Column + Ship.Size > 10)
        {
            return false;
        }
        else
        {
            if (Orientation == 'v')
            {
                for (int Scan = 0; Scan < Ship.Size; Scan++)
                {
                    if (Board[Row + Scan, Column] != '-')
                    {
                        return false;
                    }
                }
            }
            else if (Orientation == 'h')
            {
                for (int Scan = 0; Scan < Ship.Size; Scan++)
                {
                    if (Board[Row, Column + Scan] != '-')
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    private static bool CheckWin(char[,] Board)
    {
        for (int Row = 0; Row < 10; Row++)
        {
            for (int Column = 0; Column < 10; Column++)
            {
                if (Board[Row, Column] == 'A' || Board[Row, Column] == 'B' || Board[Row, Column] == 'S' || Board[Row, Column] == 'D' || Board[Row, Column] == 'P')
                {
                    return false;
                }
            }
        }
        return true;
    }

    private static void PrintBoard(char[,] Board)
    {
        Console.WriteLine();
        Console.WriteLine("The board looks like this: ");
        Console.WriteLine();
        Console.Write(" ");
        for (int Column = 0; Column < 10; Column++)
        {
            Console.Write(" " + Column + "  ");
        }
        Console.WriteLine();
        for (int Row = 0; Row < 10; Row++)
        {
            Console.Write(Row + " ");
            for (int Column = 0; Column < 10; Column++)
            {
                if (Board[Row, Column] == '-')
                {
                    Console.Write(" ");
                }
                else if (Board[Row, Column] == 'A' || Board[Row, Column] == 'B' || Board[Row, Column] == 'S' || Board[Row, Column] == 'D' || Board[Row, Column] == 'P'|| Board[Row,Column] == 'X')
                {
                    Console.Write(" ");
                }
                else
                {
                    Console.Write(Board[Row, Column]);
                }
                if (Column != 9)
                {
                    Console.Write(" | ");
                }
            }
            Console.WriteLine();
        }
    }

    private static void DisplayMenu()
    {
        Console.WriteLine("MAIN MENU");
        Console.WriteLine("");
        Console.WriteLine("1. Start new game");
        Console.WriteLine("2. Load training game");
        Console.WriteLine("3. Save current game");
        Console.WriteLine("4. Load saved game");
        Console.WriteLine("9. Quit");
        Console.WriteLine();
    }

    private static int GetMainMenuChoice()
    {
        int Choice = 0;
        Console.Write("Please enter your choice: ");
        Choice = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine();
        return Choice;
    }

    static void Bigbomb(ref char[,] Board, ref ShipType[] Ships, ref int Aleft, ref int Bleft, ref int Sleft, ref int Dleft, ref int Pleft, ref double misses, ref double hits, ref bool GameLost)
    {
        int row = 0;
        int column = 0;
        GetRowColumn(ref row, ref column);
        for (int R = -1; R <= 1; R++)
        {
            for (int C = -1; C <= 1; C++)
            {
                int Row = row + R;
                int Column = column + C;
                
                if (Board[Row, Column] == 'm' || Board[Row, Column] == 'h' || Board[Row, Column] == 'n')
                {
                    Console.WriteLine("Sorry, you have already shot at the square (" + Column + "," + Row + "). Please try again.");
                }
                else if (Board[Row, Column] == '-')
                {
                    bool nearmiss = false;
                    for (int r = -1; r <= 1; r++)
                    {
                        for (int c = -1; c <= 1; c++)
                        {
                            if (r == 0 && c == 0) continue;
                            try
                            {
                                switch (Board[Row + r, Column + c])
                                {
                                    case '-':
                                        break;
                                    case 'h':
                                        break;
                                    case 'm':
                                        break;
                                    case 'n':
                                        break;
                                    default:
                                        Console.WriteLine("Sorry, (" + Column + "," + Row + ") is a near miss.");
                                        Board[Row, Column] = 'n';
                                        nearmiss = true;
                                        break;

                                }
                            }
                            catch
                            {
                                nearmiss = false;
                            }
                            if (nearmiss) break;
                        }
                        if (nearmiss) break;
                    }
                    if (!nearmiss)
                    {
                        Console.WriteLine("Sorry, (" + Column + "," + Row + ") is a miss.");
                        Board[Row, Column] = 'm';
                    }


                    misses++;
                }
                else if (Board[Row, Column] == 'X')
                {
                    GameLost = true;
                    Console.WriteLine("Sorry you have lost the game by hitting a mine.\nPress any key to continue.");
                    Console.ReadLine();
                }
                else
                {

                    Console.WriteLine("Hit at (" + Column + "," + Row + ").");
                    switch (Board[Row, Column])
                    {
                        case 'A':
                            Aleft--;
                            if (Aleft == 0) Console.WriteLine("You have sunk an aircraft carrier.");
                            else Console.WriteLine("You have hit an aircraft carrier.");
                            break;
                        case 'B':
                            Bleft--;
                            if (Bleft == 0) Console.WriteLine("You have sunk a battleship.");
                            else Console.WriteLine("You have hit a battleship.");
                            break;
                        case 'S':
                            Sleft--;
                            if (Sleft == 0) Console.WriteLine("You have sunk a submarine.");
                            else Console.WriteLine("You have hit a submarine.");
                            break;
                        case 'D':
                            Dleft--;
                            if (Dleft == 0) Console.WriteLine("You have sunk a destroyer.");
                            else Console.WriteLine("You have hit a destroyer.");

                            break;
                        case 'P':
                            Pleft--;
                            if (Pleft == 0) Console.WriteLine("You have sunk a patrol boat.");
                            else Console.WriteLine("You have hit a patrol boat.");

                            break;
                        default:
                            break;
                    }
                    Board[Row, Column] = 'h';
                    hits++;
                }
                double hitrate = Math.Round(((hits / (hits + misses)) * 100), 2);
                Console.WriteLine("Your hitrate is {0}%", hitrate);
            }
        }
    }




    private static void PlayGame(ref char[,] Board, ref ShipType[] Ships)
    {
        bool GameWon = false;
        bool GameLost = false;
        bool BigBomb = true;
        int Aleft = Ships[0].Size;
        int Bleft = Ships[1].Size;
        int Sleft = Ships[2].Size;
        int Dleft = Ships[3].Size;
        int Pleft = Ships[4].Size;
        double misses = 0;
        double hits = 0;
        string choice = "";
        
        while (GameWon == false)
        {
            PrintBoard(Board);
            if (BigBomb)
            {
                Console.Write("Would you like to use a big bomb? (y/n)");
                choice = Console.ReadLine().ToLower();
                if(choice == "y")
                {
                    BigBomb = false;
                    Bigbomb(ref Board, ref Ships, ref Aleft, ref Bleft, ref Sleft, ref Dleft, ref Pleft, ref misses, ref hits, ref GameLost);
                    Console.Write("Would you like to continue with the game? (y/n): ");
                    string input1 = Console.ReadLine().ToLower();
                    if (input1 == "n") break;

                    continue;
                }
            }
            
            MakePlayerMove(ref Board, ref Ships, ref Aleft, ref Bleft, ref Sleft, ref Dleft, ref Pleft, ref misses, ref hits, ref GameLost);
            if (GameLost)
            {
                break;
            }
            GameWon = CheckWin(Board);
            if (GameWon == true)
            {
                Console.WriteLine("All ships sunk!");
                Console.WriteLine();
            }
            Console.Write("Would you like to continue with the game? (y/n): ");
            string input = Console.ReadLine().ToLower();
            if (input == "n") break;
        }
    }

    private static void SetUpShips(ref ShipType[] Ships)
    {
        Ships[0].Name = "Aircraft Carrier";
        Ships[0].Size = 5;
        Ships[1].Name = "Battleship";
        Ships[1].Size = 4;
        Ships[2].Name = "Submarine";
        Ships[2].Size = 3;
        Ships[3].Name = "Destroyer";
        Ships[3].Size = 3;
        Ships[4].Name = "Patrol Boat";
        Ships[4].Size = 2;
    }

    static void Main(string[] args)
    {
        ShipType[] Ships = new ShipType[5];
        char[,] Board = new char[10, 10];
        int MenuOption = 0;
        while (MenuOption != 9)
        {
            Console.Clear();

            SetUpShips(ref Ships);
            DisplayMenu();
            MenuOption = GetMainMenuChoice();
            if (MenuOption == 1)
            {
                SetUpBoard(ref Board);
                PlaceRandomShips(ref Board, Ships);
                PlaceRandomMines(ref Board);
                PlayGame(ref Board, ref Ships);
            }
            if (MenuOption == 2)
            {
                LoadGame(TrainingGame, ref Board);
                PlayGame(ref Board, ref Ships);
            }
            if (MenuOption == 3)
            {
                SaveGame(Board);
            }
            if (MenuOption == 4)
            {
                LoadSavedGame(ref Board);
                PlayGame(ref Board, ref Ships);
            }
        }
    }

    static void PlaceRandomMines(ref char[,] Board)
    {
        for (int i = 0; i < 5; i++)
        {
            bool Valid = false;
            Random rng = new Random();

            while(Valid == false)
            {
                int randomrow = rng.Next(10);
                int randomcol = rng.Next(10);
                if (Board[randomrow, randomcol] == '-')
                {
                    Board[randomrow, randomcol] = 'X';
                    Valid = true;
                }

            }



        }






    }




    static void SaveGame(char[,] Board)
    {
        Console.Write("Enter the name that you would like to call the saved file: ");
        string fileName = Console.ReadLine() + ".txt";
        string compressed = "";
        string uncompressed = "";
        for (int row = 0; row < 10; row++)
        {
            for (int column = 0; column < 10; column++)
            {
                uncompressed += Board[row, column];
            }
        }

        int n = uncompressed.Length;
        for (int i = 0; i < n; i++)
        {
            int count = 1;
            while (i < n - 1 && uncompressed[i] == uncompressed[i + 1])
            {
                count++;
                i++;
            }
            compressed = compressed + uncompressed[i].ToString() + count.ToString();
        }
        StreamWriter save = new StreamWriter(fileName, false);

        save.Write(compressed);
        save.Close();
        Console.ReadLine();


    }
    static void LoadSavedGame(ref char[,] Board)
    {
        Console.Write("What is the name of the saved file: ");
        string fileName = Console.ReadLine() + ".txt";
        StreamReader load = new StreamReader(fileName);
        string compressed = load.ReadLine();
        load.Close();
        string uncompressed = "";
        for (int i = 0; i < compressed.Length; i += 2)
        {
            int times = int.Parse(compressed[i + 1].ToString());
            char character = compressed[i];
            for (int j = 0; j < times; j++)
            {
                uncompressed += character;
            }
        }
        int pos = 0;
        while (pos < compressed.Length)
        {

            for (int r = 0; r < 10; r++)
            {
                for (int c = 0; c < 10; c++)
                {
                    Board[r, c] = uncompressed[pos];
                    pos++;
                }
            }

        }

    }
}
