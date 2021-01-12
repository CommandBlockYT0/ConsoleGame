using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ConsoleApp.Classes
{
    class MovingLine
    {
        // Source: http://ericw.ca/notes/bresenhams-line-algorithm-in-csharp.html
        public static IEnumerable<Point> LineTrajectory(int x0, int y0, int x1, int y1)
        {
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                int t;
                t = x0; // swap x0 and y0
                x0 = y0;
                y0 = t;
                t = x1; // swap x1 and y1
                x1 = y1;
                y1 = t;
            }
            if (x0 > x1)
            {
                int t;
                t = x0; // swap x0 and x1
                x0 = x1;
                x1 = t;
                t = y0; // swap y0 and y1
                y0 = y1;
                y1 = t;
            }
            int dx = x1 - x0;
            int dy = Math.Abs(y1 - y0);
            int error = dx / 2;
            int ystep = (y0 < y1) ? 1 : -1;
            int y = y0;
            for (int x = x0; x <= x1; x++)
            {
                yield return new Point((steep ? y : x), (steep ? x : y));
                error = error - dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
            yield break;
        }

        // Дальше писал сам
        IEnumerable<Point> trajectory;
        int trajectoryStep = -1;
        int side = 1;

        int cx = 0;
        int cy = 0;
        public int CurrentX { get { return cx; } }
        public int CurrentY { get { return cy; } }
        int px = 0;
        int py = 0;
        public int PrevX { get { return px; } }
        public int PrevY { get { return py; } }

        public MovingLine(int x0, int y0, int x1, int y1)
        {
            trajectory = LineTrajectory(x0, y0, x1, y1);
            cx = x0;
            cy = y0;
            px = cx;
            py = cy;
        }
        
        public void Step()
        {
            int c = trajectory.Count();
            if (c > 1)
            {
                if (trajectoryStep >= trajectory.Count() - 1) side = -side;
                if (trajectoryStep > -1)
                {
                    Point _p = trajectory.ElementAt(trajectoryStep);
                    px = (int)_p.X;
                    py = (int)_p.Y;
                }
                trajectoryStep += side;
                if (trajectoryStep < 0) { side = -side; trajectoryStep = 1; }
                Point p = trajectory.ElementAt(trajectoryStep);
                cx = (int)p.X;
                cy = (int)p.Y;
            }
        }
    }
}
