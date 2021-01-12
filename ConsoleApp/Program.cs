using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Windows.Input;

namespace ConsoleApp
{
    class Player
    {
        char icon = '♥';
        public char Icon { get { return icon; } }
        int x, y;
        public int X { get { return x; } }
        public int Y { get { return y; } }

        string[] map;
        public Player(int x, int y, ref string[] map)
        {
            this.x = x;
            this.y = y;
            this.map = map;
            moveThread = new Thread(delegate ()
            {
                
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                int moveX = 0;
                int moveY = 0;
                switch (keyInfo.Key)
                {
                    case ConsoleKey.W:
                        if (isGravity)
                        {

                        }
                        else
                        {
                            moveY = 1;
                        }
                        break;
                    case ConsoleKey.A:
                        moveX = -1;
                        break;
                    case ConsoleKey.S:
                        moveY = 1;
                        break;
                    case ConsoleKey.D:
                        moveX = 1;
                        break;
                    case ConsoleKey.Spacebar:
                        Jump();
                        break;
                    case ConsoleKey.Escape:
                        break;
                    default:
                        break;
                }
                if (x + moveX >= 0 && x + moveX < 20)
                    if (this.map[y][x + moveX] == '0')
                        x += moveX;

                if (y + moveY >= 0 && y + moveY < 20)
                    if (this.map[y + moveY][x] == '0')
                        y += moveY;

            });
            moveThread.Start();
        }
        bool isGravity = true;
        bool isJump = false;
        Thread moveThread;
        bool threadEnded = true;
        private void Jump()
        {
            new Thread(delegate ()
            {
                if (map[y - 1][x] == '0' && !isJump)
                {
                    isJump = true;
                    y--;
                    Thread.Sleep(200);
                    if (map[y + 1][x] == '0')
                        y++;
                    isJump = false;
                }
            }).Start();
        }
        public void HandleGravity()
        {
            if (!isJump)
            {
                if (y + 1 < 20)
                    if (map[y + 1][x] == '0')
                        y++;
            }
        }
        public void HandleMove()
        {
            HandleGravity();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
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
                        else if (pixel.R == 255 && pixel.G == 0 && pixel.B == 0)
                        {
                            row += "2";
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

            while (true)
            {
                Console.SetCursorPosition(0, 0);
                for (int i = 0; i < bitmap.Width; i++)
                {
                    for (int j = 0; j < bitmap.Height; j++)
                    {
                        ConsoleColor color = ConsoleColor.White;
                        if (map[i][j] == '1') color = ConsoleColor.Black;
                        else if (map[i][j] == '2') color = ConsoleColor.Red;
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
                    Console.WriteLine();
                }
                Console.Write("WASD - Move, G - Toggle Gravity, Space - Jump");
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
