using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Windows.Input;
using ConsoleApp.Classes;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            List<MovingLine> movingLines = new List<MovingLine>();



            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string[] map = new string[20];

            int playerX = 0;
            int playerY = 0;

            Bitmap bitmap = new Bitmap("./map.bmp");
            if (bitmap.Width == bitmap.Height && bitmap.Width == 20)
            {
                for (int i = 0; i < bitmap.Width; i++)
                {
                    string row = "";
                    for (int j = 0; j < bitmap.Height; j++)
                    {
                        Color pixel = bitmap.GetPixel(j, i);
                        if (pixel.R == 255 && pixel.G == 255 && pixel.B == 255)
                        {
                            row += "0";
                        }
                        else if(pixel.R == 0 && pixel.G == 0 && pixel.B == 0)
                        {
                            row += "1";
                        }
                        else if (pixel.R == 0 && pixel.G == 0 && pixel.B == 255)
                        {
                            row += "2";
                        }
                        else if (pixel.R == 255)
                        {
                            row += "0";
                            if (pixel.G >= bitmap.Width || pixel.B >= bitmap.Height) { Console.WriteLine($"Found incorrect moving pixel at X: {i}, Y: {j}! It cannot go to X: {pixel.G}, Y: {pixel.G}!"); while (true) { } }
                            movingLines.Add(new MovingLine(j, i, pixel.G, pixel.B));
                        }
                        else if (pixel.R == 0 && pixel.G == 255 && pixel.B == 0)
                        {
                            row += "0";
                            playerX = j;
                            playerY = i;
                        }
                        else
                        {
                            row += "?";
                            Console.WriteLine($"Found incorrect pixel at X: {i}, Y: {j}! Please redraw");
                            while (true) { }
                        }
                    }
                    map[i] = row;
                }
            } else
            {
                Console.WriteLine($"Incorrect Map Size ({bitmap.Width}x{bitmap.Height}, expected 20x20)!");
                while (true) { }
            }

            Console.Title = "VladikRunner";
            Console.CursorVisible = false;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Player player = new Player(playerX, playerY, ref map);
            bool firstTime = true;
            Timer t = new Timer(x =>
            {
                foreach (var line in movingLines)
                {
                    StringBuilder sbPrev = new StringBuilder(map[line.PrevY]);
                    sbPrev[line.PrevX] = '0';
                    map[line.PrevY] = sbPrev.ToString();

                    StringBuilder sb = new StringBuilder(map[line.CurrentY]);
                    sb[line.CurrentX] = '3';
                    map[line.CurrentY] = sb.ToString();

                    if ((player.X == line.PrevX && player.Y + 1 == line.PrevY))
                    {
                        player.X = line.CurrentX;
                        player.Y = line.CurrentY - 1;
                    }

                    line.Step();

                }
            }, null, 0, 200);
            while (true)
            {
                Console.SetCursorPosition(0, 0);
                for (int i = 0; i < bitmap.Width; i++)
                {
                    for (int j = 0; j < bitmap.Height; j++)
                    {
                        ConsoleColor color = ConsoleColor.Black;
                        if (map[i][j] == '1') color = ConsoleColor.White;
                        else if (map[i][j] == '2') color = ConsoleColor.Blue;
                        else if (map[i][j] == '3') color = ConsoleColor.Red;
                        Console.BackgroundColor = color;
                        if (j == player.X && i == player.Y)
                        {
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.Write($"{player.Icon}{player.Icon}");
                        }
                        else
                        {
                            Console.Write("  ");
                        }
                    }
                    Console.ResetColor();
                    Console.Write("|");
                    Console.WriteLine();
                }
                Console.ResetColor();
                for (int i = 0; i < 41; i++)
                {
                    Console.Write("¯");
                }
                Console.WriteLine();
                Console.Write("WASD - Move, Space - Jump, G - ");
                if (firstTime)
                {
                    firstTime = false;
                    Console.WriteLine("Off gravity ");
                }
                player.HandleMove();
            }
        }


        static void ex1()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.Yellow;

            Console.WindowHeight = 10;
            Console.WindowWidth = 20;
            Console.WindowLeft = 0;
            Console.WindowTop = 5;




            Console.Title = "GAME";

            Console.CursorLeft = 5;
            Console.CursorTop = 5;

            if (Console.CapsLock)
            {
                Console.WriteLine("CAPS");
            }
            Console.CursorVisible = false;

            Console.ReadLine();
            Console.Clear();
        }
    }
}
