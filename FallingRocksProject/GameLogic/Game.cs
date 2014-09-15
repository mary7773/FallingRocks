using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game
{
    public struct Object
    {
        public int x;
        public int y;
        public char symbol;
        public ConsoleColor color;

    }
    class Game
    {

        static int playfieldSize = 0;
        static char[] c = { '#', '@', '#', '*', '<', '>', '(', ')', '$', '%', '&', '^','+', '!' };
        static ConsoleColor[] colors = {ConsoleColor.Black, ConsoleColor.Blue, ConsoleColor.Cyan, ConsoleColor.Magenta, ConsoleColor.Green };
        static Object dwarf = new Object();
        static List<Object> rocks = new List<Object>();
        static Object rock = new Object();
        static int livesCount = 3;
        static bool hitted = false;
        static int speed = 0;
        static int bonus = 0;



        static void SetInitialField()
        {
            Console.BufferHeight = Console.WindowHeight = 30;
            Console.BufferWidth = Console.WindowWidth = 40;

        }

        static void Main()
        {
            SetInitialField();

            playfieldSize = Console.WindowWidth / 2;
           

                dwarf.symbol = 'O';
                dwarf.x = playfieldSize / 2;
                dwarf.y = Console.WindowHeight - 1;
                dwarf.color = ConsoleColor.DarkMagenta;

                Random rand = new Random();



                while (true)
                {
                    speed++;
                    if (speed % 4 == 0)
                    {
                        speed = 0;
                        if (bonus == 5)
                        {
                            livesCount++;
                            bonus = 0;
                        }
                    }

                    rock.symbol = c[rand.Next(0, c.Length)];
                    rock.x = rand.Next(0, playfieldSize);
                    rock.y = 0;
                    rock.color = colors[rand.Next(0, colors.Length)];
                    rocks.Add(rock);


                    //Move our dwarf(pressed key)
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                        while (Console.KeyAvailable)
                        {
                            Console.ReadKey(true);
                        }
                        if (pressedKey.Key == ConsoleKey.LeftArrow)
                        {
                            if (dwarf.x - 1 >= 0)
                            {
                                dwarf.x--;
                            }

                        }
                        else if (pressedKey.Key == ConsoleKey.RightArrow)
                        {
                            if (dwarf.x + 1 < playfieldSize)
                            {
                                dwarf.x++;
                            }
                        }
                    }

                    //Move rocks
                    MoveRocks();

                    //Check if the dwarf is hit             
                    //Clear    
                    Console.Clear();
                    //Draw playfield --> our player and rocks
                    DrawDwarf();
                    DrawRock();
                    //Draw info 
                    PrintStringOnPosition(22, 6, "Lives : " + livesCount, ConsoleColor.White);
                    PrintStringOnPosition(22, 10, "Rocks speed : " + speed*10, ConsoleColor.White);
                    PrintStringOnPosition(22, 14, "Bonus : " + bonus, ConsoleColor.White);
                    //Slow down program

                    Thread.Sleep(300 - speed);
                }             

            }
            public static void PrintOnPosition(int x, int y, char c, ConsoleColor color = ConsoleColor.Green)
            {
                Console.CursorVisible = false;
                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = color;
                Console.Write(c);
            }

            public static void PrintStringOnPosition(int x, int y, string str, ConsoleColor color = ConsoleColor.Green)
            {
                Console.CursorVisible = false;
                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = color;
                Console.Write(str);
            }

            public static void MoveRocks()
            {
                List<Object> newList = new List<Object>();
                for (int i = 0; i < rocks.Count; i++)
                {
                    Object newRocks = new Object();
                    newRocks.x = rocks[i].x;
                    newRocks.y = rocks[i].y + 1;
                    newRocks.color = rocks[i].color;
                    newRocks.symbol = rocks[i].symbol;
                    if (dwarf.x == newRocks.x && dwarf.y == newRocks.y && newRocks.symbol == '!')
                    {
                        bonus++;
                    }
                    if (dwarf.x == newRocks.x && dwarf.y == newRocks.y && newRocks.symbol != '!')
                    {
                        livesCount--;
                        bonus = 0;
                        hitted = true;
                        speed += 20;
                        if (livesCount == 0)
                        {
                            PrintStringOnPosition(8, 8, "Game Over!!!", ConsoleColor.DarkRed);
                            PrintStringOnPosition(8, 9, "press [enter] to exit", ConsoleColor.DarkRed);
                            Environment.Exit(0);
                        }
                    }
                    
                    if (newRocks.y < Console.WindowHeight)
                    {
                        newList.Add(newRocks);
                    }
                                    
                }
                rocks = newList;
            } 

            public static void DrawRock()
            {
                foreach (Object newRock in rocks)
                {
                   PrintOnPosition(newRock.x, newRock.y, newRock.symbol, newRock.color);
                }
            }

            public static void DrawDwarf()
            {
                if (hitted)
                {                  
                    PrintOnPosition(dwarf.x, dwarf.y, 'X', ConsoleColor.Red);
                    rocks.Clear();
                    hitted = false;
                }
                else
                {
                    PrintOnPosition(dwarf.x, dwarf.y, dwarf.symbol, dwarf.color);
                }
                
            }
        }

    }




