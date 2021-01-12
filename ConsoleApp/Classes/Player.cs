using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApp.Classes
{
    class Player
    {
        char icon = '♥';
        public char Icon { get { return icon; } }
        int x, y;
        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }

        string[] map;
        public Player(int x, int y, ref string[] map)
        {
            this.x = x;
            this.y = y;
            this.map = map;
        }
        bool isGravity = true;
        bool isJump = false;
        private void Jump()
        {
            new Thread(delegate ()
            {
                if ((map[y - 1][x] == '0' || map[y - 1][x] == '2') && !isJump && y > 0)
                {
                    isJump = true;
                    y--;
                    Console.Beep(1000, 100);
                    Thread.Sleep(100);
                    if (map[y + 1][x] == '0' && isGravity)
                    {
                        Console.Beep(500, 100);
                        y++;
                    }
                    isJump = false;
                }
            }).Start();
        }
        bool jumped = false;
        bool lastNonVoid = false;
        public void HandleGravity()
        {
            if (isGravity)
                if (!isJump)
                {
                    if (y + 1 < 20)
                        if (map[y + 1][x] == '0')
                        {
                            y++;
                            jumped = true;
                            lastNonVoid = true;
                        }
                        else if (jumped)
                        {
                            new Thread(() => Console.Beep(500, 100)).Start();
                            jumped = false;
                        }
                        else { }
                    else
                    {
                        new Thread(() => {
                            if (lastNonVoid)
                            {
                                lastNonVoid = false;
                                Console.Beep(500, 100);
                            }
                        }).Start();
                    }
                } else
                {
                    if (map[y][x] == '2')
                    {
                        lastNonVoid = false;
                        jumped = false;
                        y--;
                    }
                }
        }
        public void HandleMove()
        {
            HandleGravity();
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                int moveX = 0;
                int moveY = 0;
                switch (keyInfo.Key)
                {
                    case ConsoleKey.W:
                        if (!isGravity)
                            moveY = -1;
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
                    case ConsoleKey.G:
                        isGravity = !isGravity;
                        Console.WriteLine(isGravity ? "Off" : "On " + " gravity ");
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
            }
        }
    }
}