using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunner
{
    class Player
    {
        Vector direction; //current looking direction of the player
        private int positionX; //current position of the player
        private int positionY; //current position of the player
        public int Step { get; } //size of player's step
        public double FOV { get; } //Field of view (in radians)
        public int startLocationX { get; private set; } //where do I start?
        public int startLocationY { get; private set; } //where do I start?
        public readonly double rotationSpeed = 2.2; //degrees/10 ms
        public bool Killed { get; set; } 

        public Player(int px, int py, Vector direction)
        {
            this.direction = direction;
            positionX = px;
            positionY = py;
            startLocationX = px;
            startLocationY = py;
            Step = 3;
            FOV = (50*Math.PI) / 180; //50 degrees (but in radians)
            Killed = false;
        }
        public void SetPosition(int x, int y)
        {
            positionX = x;
            positionY = y;
        }
        public Tuple<int,int> GetPosition()
        {
            Tuple<int, int> position = new Tuple<int, int>(positionX, positionY);
            return position;
        }
        public void SetDirection(Vector newDirection)
        {
            direction = newDirection;
        }
        public Vector GetDirection()
        {
            return direction;
        }

    }
}
