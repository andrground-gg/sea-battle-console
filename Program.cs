using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        static public bool gameOver = false;
        static public Random rand = new Random();
        public const int ROWS = 10, COLS = 10;
        static public int[][] enemy = new int[ROWS][];
        static public int[][] player = new int[ROWS][];
        static public int[][] hidden = new int[ROWS][];
        static public bool additional = false;
        static int fourdeck = 1, threedeck = 2, twodeck = 3, onedeck = 4;
        static public List<Ship> shipsEnemy = new List<Ship>();
        static public List<Ship> shipsPlayer = new List<Ship>();
        static public ConsoleKeyInfo key;
        static void Main(string[] args)
        {          
            for(int i = 0; i < ROWS; i++) {
                enemy[i] = new int[COLS];
                player[i] = new int[COLS];
                hidden[i] = new int[COLS];
                Array.Fill(enemy[i], 0);
                Array.Fill(player[i], 0);
                Array.Fill(hidden[i], 0);
            }

            automaticPlacing(shipsEnemy, fourdeck, threedeck, twodeck, onedeck, ref enemy);

            do
            {
                Console.Clear();
                Console.WriteLine("1 - Automatic ships placing\n2 - Manual ships placing");
                key = Console.ReadKey();
                if (key.Key == ConsoleKey.D1)
                    automaticPlacing(shipsPlayer, fourdeck, threedeck, twodeck, onedeck, ref player);   
                else if (key.Key == ConsoleKey.D2)
                    manualPlacing(shipsPlayer, fourdeck, threedeck, twodeck, onedeck, ref player);
            } while (key.Key != ConsoleKey.D2 && key.Key != ConsoleKey.D1);

            do {
                Console.Clear();

                if (!additional)
                {
                    playerTurn(shipsEnemy, ref enemy, ref additional);

                    if (shipsEnemy.Count == 0){
                        gameOver = true;
                        continue;
                    }
                }
                else {
                    additional = false;
                }

                Console.Clear();
                Console.WriteLine("Enemy field\n");
                printField(hidden);
                Console.WriteLine("------------------------------------------------------------------\nYour field\n");
                printField(player);

                if (!additional)
                {
                    Thread.Sleep(500);
                    enemyTurn(shipsPlayer, ref player, ref additional);
                    if (shipsPlayer.Count == 0){
                        gameOver = true;
                        continue;
                    }
                }
                else {
                    additional = false;
                }
            } while (!gameOver);

            Console.Clear();
            Console.WriteLine("Enemy field\n");
            printField(hidden);
            Console.WriteLine("------------------------------------------------------------------\nYour field\n");
            printField(player);
            Console.ForegroundColor = (shipsPlayer.Count == 0 ? ConsoleColor.Red : ConsoleColor.Green);
            Console.WriteLine(shipsPlayer.Count == 0 ? "You lost!" : "You won!");

            Console.ReadKey();
        }
        static public void printField(int[][] field){
            Console.Write("   ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            for (int i = 65; i < 65 + COLS; i++)
                Console.Write(Convert.ToChar(i) + " ");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;

            for (int i = 0; i < ROWS; i++) {
                for(int j = 0; j < COLS; j++) {
                    if (j == 0) {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("{0,3}", i + 1 + " ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    if (field[i][j] == 1)
                        Console.ForegroundColor = ConsoleColor.Blue;
                    else if (field[i][j] == 3)
                        Console.ForegroundColor = ConsoleColor.Red;
                    else if (field[i][j] == 4)
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    else if (field[i][j] == 5)
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(field[i][j] + " ");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine();
            }
        }
        static public void manualPlacing(List<Ship> ships, int fourdeck, int threedeck, int twodeck, int onedeck, ref int[][] field) {
            int decks = 4;

            for (int i = 0; i < fourdeck + threedeck + twodeck + onedeck; i++)
            {
                int stX, stY;
                Direction? dir;
                bool err = false;

                do {
                    stX = -1;
                    stY = -1;
                    dir = null;

                    Console.Clear();
                    printField(field);

                    if(err){
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Wrong input");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    err = true;

                    if (decks != 1)
                    {
                        Console.WriteLine($"\nEnter {decks}-decked ship direction(arrow keys)");
                        switch (Convert.ToString(Console.ReadKey().Key))
                        {
                            case "LeftArrow": dir = Direction.Left; break;
                            case "RightArrow": dir = Direction.Right; break;
                            case "UpArrow": dir = Direction.Up; break;
                            case "DownArrow": dir = Direction.Down; break;
                        }
                        Console.Write(dir);
                        if (dir == null) {
                            err = true;
                            continue;
                        }
                    }
                    else
                        dir = Direction.Left;

                    Console.WriteLine($"\nEnter {decks}-decked ship starting number: ");
                    string stYstr = Convert.ToString(Console.ReadKey().KeyChar);
                    if (Int32.TryParse(stYstr, out stY)) {
                        if (stY == 0)
                            stY = 9;
                        else
                            stY--;
                    }
                    else {
                        err = true;
                        continue;
                    }
                    Console.WriteLine($"\nEnter {decks}-decked ship starting letter: ");
                    string stXstr = Convert.ToString(Console.ReadKey().Key);
                    
                    Console.WriteLine("\n" + stYstr);
                    switch (stXstr) {
                        case "A": stX = 0; break;
                        case "B": stX = 1; break;
                        case "C": stX = 2; break;
                        case "D": stX = 3; break;
                        case "E": stX = 4; break;
                        case "F": stX = 5; break;
                        case "G": stX = 6; break;
                        case "H": stX = 7; break;
                        case "I": stX = 8; break;
                        case "J": stX = 9; break;
                        default: stX = -1; break;
                    }
                    if (stX == -1) {
                        err = true;
                        continue;
                    }
                   
                } while (!Ship.checkPosition(decks, stY, stX, dir, field));
                
                ships.Add(new Ship(decks, stY, stX, dir, ref field));
                if (ships.Count == fourdeck || ships.Count == fourdeck + threedeck || ships.Count == fourdeck + threedeck + twodeck || ships.Count == fourdeck + threedeck + twodeck + onedeck)
                    decks--;
            }
        }
        static public void automaticPlacing(List<Ship> ships, int fourdeck, int threedeck, int twodeck, int onedeck, ref int[][] field) {
            int decks = 4;
            Array values = Enum.GetValues(typeof(Direction));
            for (int i = 0; i < fourdeck + threedeck + twodeck + onedeck; i++) {
                Direction dir = (Direction)values.GetValue(rand.Next(values.Length));
                int stY = rand.Next(0, ROWS);
                int stX = rand.Next(0, COLS);
                while(!Ship.checkPosition(decks, stY, stX, dir, field)) {
                    stY = rand.Next(0, ROWS);
                    stX = rand.Next(0, COLS);
                }
                ships.Add(new Ship(decks, stY,stX, dir, ref field));
                if (ships.Count == fourdeck || ships.Count == fourdeck + threedeck || ships.Count == fourdeck + threedeck + twodeck || ships.Count == fourdeck + threedeck + twodeck + onedeck) 
                    decks--;
            }           
        }
        static public void playerTurn(List<Ship> ships, ref int[][] field, ref bool additional) {
            int stX = -1, stY = -1;
            bool err = false;
            do
            {
                stX = -1;
                stY = -1;

                Console.Clear();
                Console.WriteLine("Enemy field\n");
                printField(hidden);
                Console.WriteLine("------------------------------------------------------------------\nYour field\n");
                printField(player);

                if (err){
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Wrong input");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                err = true;

                Console.WriteLine($"\nEnter number: ");
                string stYstr = Convert.ToString(Console.ReadKey().KeyChar);
                if (Int32.TryParse(stYstr, out stY))
                {
                    if (stY == 0)
                        stY = 9;
                    else
                        stY--;
                }
                else{
                    err = true;
                    continue;
                }
                Console.WriteLine($"\nEnter letter: ");
                string stXstr = Convert.ToString(Console.ReadKey().Key);

                Console.WriteLine("\n" + stYstr);
                switch (stXstr)
                {
                    case "A": stX = 0; break;
                    case "B": stX = 1; break;
                    case "C": stX = 2; break;
                    case "D": stX = 3; break;
                    case "E": stX = 4; break;
                    case "F": stX = 5; break;
                    case "G": stX = 6; break;
                    case "H": stX = 7; break;
                    case "I": stX = 8; break;
                    case "J": stX = 9; break;
                    default: stX = -1; break;
                }
                if (stX == -1){
                    err = true;
                    continue;
                }
                if(field[stY][stX] == 3 || field[stY][stX] == 5 || field[stY][stX] == 4){
                    stY = -1;
                    stX = -1;
                    err = true;
                    continue;
                }
                for(int i = 0; i < ships.Count; i++) {
                    for (int j = 0; j < ships[i].YCoords.Count; j++) {
                        if (ships[i].YCoords[j] == stY && ships[i].XCoords[j] == stX && field[stY][stX] != 5 && field[stY][stX] != 3)
                        {
                            field[stY][stX] = 3;
                            additional = true;
                            if (ships[i].checkIfDestroyed(ref field))
                                ships.RemoveAt(i);
                            break;
                        }                       
                        if (field[stY][stX] == 3 || field[stY][stX] == 5)
                            break;
                        field[stY][stX] = 4;
                    }
                    if (field[stY][stX] == 3 || field[stY][stX] == 5)
                        break;
                }
                for (int i = 0; i < field.Length; i++)
                    for (int j = 0; j < field[0].Length; j++)
                        if (field[i][j] != 0 && field[i][j] != 1) 
                            hidden[i][j] = field[i][j];
            } while (stY == -1 || stX == -1);
        }
        static public void enemyTurn(List<Ship> ships, ref int[][] field, ref bool additional) {
            int stY = -1, stX = -1, damagedY = -1, damagedX = -1, shipInd = -1;
            for (int i = 0; i < ships.Count; i++) {
                for (int j = 0; j < ships[i].YCoords.Count; j++) {
                    if (field[ships[i].YCoords[j]][ships[i].XCoords[j]] == 3) {
                        damagedY = ships[i].YCoords[j];
                        damagedX = ships[i].XCoords[j];
                        shipInd = i;
                        if((damagedY - 1 >= 0 && (field[damagedY - 1][damagedX] == 1 || field[damagedY - 1][damagedX] == 0)) || (damagedY + 1 < field.Length && (field[damagedY + 1][damagedX] == 1 || field[damagedY + 1][damagedX] == 0))
                            || (damagedX - 1 >= 0 && (field[damagedY][damagedX - 1] == 1 || field[damagedY][damagedX - 1] == 0)) || (damagedX + 1 < field[0].Length && (field[damagedY][damagedX + 1] == 1 || field[damagedY][damagedX + 1] == 0)))
                            break;
                    }
                }
                if (shipInd != -1 && ((damagedY - 1 >= 0 && (field[damagedY - 1][damagedX] == 1 || field[damagedY - 1][damagedX] == 0)) || (damagedY + 1 < field.Length && (field[damagedY + 1][damagedX] == 1 || field[damagedY + 1][damagedX] == 0))
                            || (damagedX - 1 >= 0 && (field[damagedY][damagedX - 1] == 1 || field[damagedY][damagedX - 1] == 0)) || (damagedX + 1 < field[0].Length && (field[damagedY][damagedX + 1] == 1 || field[damagedY][damagedX + 1] == 0))))
                    break;
            }

            if (shipInd == -1) {
                do {
                    stY = rand.Next(0, field.Length);
                    stX = rand.Next(0, field[0].Length);
                } while (field[stY][stX] == 3 || field[stY][stX] == 5 || field[stY][stX] == 4);
                for (int i = 0; i < ships.Count; i++){
                    for (int j = 0; j < ships[i].YCoords.Count; j++){
                        if (ships[i].YCoords[j] == stY && ships[i].XCoords[j] == stX && field[stY][stX] != 5 && field[stY][stX] != 3){
                            field[stY][stX] = 3;
                            additional = true;
                            if (ships[i].checkIfDestroyed(ref field))
                                ships.RemoveAt(i);
                            break;
                        }
                        if (field[stY][stX] == 3 || field[stY][stX] == 5)
                            break;
                        field[stY][stX] = 4;
                    }
                    if (field[stY][stX] == 3 || field[stY][stX] == 5)
                        break;
                }
            }
            else {
                do {
                    switch (rand.Next(0, 4)) {
                        case 0: stY = damagedY + 1; stX = damagedX; break;
                        case 1: stY = damagedY - 1; stX = damagedX; break;
                        case 2: stY = damagedY; stX = damagedX + 1; break;
                        case 3: stY = damagedY; stX = damagedX - 1; break;
                    }
                } while (stY >= field.Length || stX >= field[0].Length || stY < 0 || stX < 0 || field[stY][stX] == 3 || field[stY][stX] == 5 || field[stY][stX] == 4);
                if (field[stY][stX] == 0)
                    field[stY][stX] = 4;
                else if (field[stY][stX] == 1) {
                    field[stY][stX] = 3;
                    additional = true;
                    if (ships[shipInd].checkIfDestroyed(ref field))
                        ships.RemoveAt(shipInd);
                }

            }
            
        }
    }
}
