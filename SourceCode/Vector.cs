using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunner
{
    class Vector
    {
        public double x;
        public double y;

        public Vector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public Vector RotateCounterclockwise(double angle) //original vector will be rotated and saved (new vector overwrites the original one)
        {
            double newX = x * Math.Cos(angle) + y * (-Math.Sin(angle));
            double newY = x * Math.Sin(angle) + y * Math.Cos(angle);
            x = newX;
            y = newY;
            return this;
        }

        public Vector RotateClockwise(double angle) //original vector will be rotated and saved (new vector overwrites the original one)
        {
            double newX = x * Math.Cos(-angle) + y * (-Math.Sin(-angle));
            double newY = x * Math.Sin(-angle) + y * Math.Cos(-angle);
            x = newX;
            y = newY;
            return this;
        }
        public Tuple<double, double> RotateCounterclockwiseSym(double angle) //symbolic rotation; won't be saved to the original object
        {
            Tuple<double, double> xy = new Tuple<double, double>(x * Math.Cos(angle) + y * (-Math.Sin(angle)), x * Math.Sin(angle) + y * Math.Cos(angle));
            return xy;
        }
        public Tuple<double, double> RotateClockwiseSym(double angle) //symbolic rotation; won't be saved to the original object
        {
            Tuple<double, double> xy = new Tuple<double, double>(x * Math.Cos(-angle) + y * (-Math.Sin(-angle)), x * Math.Sin(-angle) + y * Math.Cos(-angle));
            return xy;
        }
        public double GetAngle() //get angle in radians; angle that vector forms with (1,0)^T vector
        {
            return Math.Atan2(y, x);
        }
    }
}
