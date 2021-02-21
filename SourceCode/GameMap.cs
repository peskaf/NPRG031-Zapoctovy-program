using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunner
{
    class GameMap
    {
        public Map map;
        public Player player;
        public Enemy enemy;
        public GameMap(int size)
        {
            map = new Map(size);
            int startLocationX = Convert.ToInt32(1.5 * map.TileSize); //position -> the middle of the first empty square
            int startLocationY = Convert.ToInt32(1.5 * map.TileSize);
            Vector baseDirection = map.Grid[(startLocationX / map.TileSize)+1, startLocationY/ map.TileSize] == 1 ? new Vector(0, -1) : new Vector(1, 0); //make the player face empty space; right drection not guaranteed
            enemy = new Enemy(startLocationX, startLocationY);
            player = new Player(startLocationX, startLocationY, baseDirection); 
        }
        Tuple<double, int, double> Raycast(int x, int y, Vector direction, int max) //cast one ray and return info about it <size, wall it hit, angle it was cast from>
        {
            int maxExtent = map.SideSize * map.TileSize - 1; //gamemap width -1 (to work as range -> [0,maxExtent])

            int x1 = x; //players position - x --- FIRST POINT
            int y1 = y; //players position - y --- FIRST POINT

            double x2 = x + (direction.x * max); //the utmost point the player is able to see at given direction - x --- SECOND POINT
            double y2 = (y - (direction.y * max)); ////the utmost point the player is able to see at given direction - y - "minus" -> negative y goes upwards on the screen --- SECOND POINT

            double raySizeH; //size of the ray - intersection with horizontal line
            double raySizeV;//size of the ray - intersection with vertical line
            int sideH = -1; //where the intersection happened (1..right, 2..up, 3..left, 4..down) - for intersection with horizontal lines
            int sideV = -1; //where the intersection happened (1..right, 2..up, 3..left, 4..down) - for intersection with vertical lines

            //horizontal lines intersection
            double Px, Py; //intersection coordinates
            if (direction.y != 0) //looking down or up
            {
                int startY = direction.y < 0 ? Convert.ToInt32(Math.Ceiling((double)y / map.TileSize) * map.TileSize) : ((y / map.TileSize) * map.TileSize); //choose line to check for intersection with player's look vector (direction)

                int x3 = 0; //start of the line the intersection will be checked with - x --- THIRD POINT
                int y3 = startY; //start of the line the intersection will be checked with - y --- THIRD POINT
                int x4 = maxExtent; //end of the line the intersection will be checked with - x --- FOURTH POINT
                int y4 = startY; //end of the line the intersection will be checked with - y --- FOURTH POINT

                int gridPx; //in which square to check
                while (true)
                {
                    double t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / ((x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4)); //won't be 0 -> wouldn't be looking down/up if it was
                    double u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / ((x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4)); //for more info check https://en.wikipedia.org/wiki/Line%E2%80%93line_intersection


                    if (t >= 0 && t <= 1 && u >= 0 && u <= 1) //potential intersection falls within the first and the second line segment
                    {
                        Px = x1 + t * (x2 - x1); //intersection x coordinate

                        gridPx = Convert.ToInt32(Math.Floor(Px / map.TileSize));
                        if (map.Grid[gridPx, direction.y > 0 ? (y3 / map.TileSize) - 1 : (y3 / map.TileSize)] == 1) //we hit the wall
                        {
                            raySizeH = Math.Sqrt(Math.Pow(Px - x, 2) + Math.Pow(y3 - y, 2)); //size of the (x, y), (Px, y3) line -> Pythagorean theorem
                            sideH = direction.y < 0 ? 4 : 2; //check it the ray hit bottom or top of the wall
                            break;
                        }
                        if (direction.y < 0) //no hit -> check on the next/previous horizontal line
                        {
                            y3 += map.TileSize;
                            y4 += map.TileSize;
                        }
                        else
                        {
                            y3 -= map.TileSize;
                            y4 -= map.TileSize;
                        }
                    }
                    else
                    {
                        raySizeH = Double.PositiveInfinity; //no intersection (max ray size didn't hit the wall)
                        break;
                    }
                }
            }
            else //not neccessary -> just to be compiled successfully
            {
                raySizeH = Double.PositiveInfinity;
            }

            //vertical lines intersection - check for more info in horizontal intersection section (comments)
            if (direction.x != 0) //looking left or right
            {
                int startX = direction.x > 0 ? Convert.ToInt32(Math.Ceiling((double)x / map.TileSize) * map.TileSize) : ((x / map.TileSize) * map.TileSize);

                int x3 = startX;
                int y3 = 0;
                int x4 = startX; 
                int y4 = maxExtent;

                int gridPy; //intersection that is being checked in the grid
                while (true)
                {
                    double t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / ((x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4)); //won't be 0 -> wouldn't be looking left/right if it was
                    double u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / ((x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4));
               
                    if (t >= 0 && t <= 1 && u >= 0 && u <= 1) //potential intersection falls within the first and the second line segment
                    {
                        Py = y1 + t * (y2 - y1); //intersection y coordinate

                        gridPy = Convert.ToInt32(Math.Floor(Py / map.TileSize));
                        if (map.Grid[direction.x < 0 ? (x3 / map.TileSize) - 1 : (x3 / map.TileSize), gridPy] == 1) //we hit the wall
                        {  
                            raySizeV = Math.Sqrt(Math.Pow((Py - y),2) + Math.Pow((x3 - x),2));
                            sideV = direction.x > 0 ? 1 : 3;
                            break;
                        }
                        if (direction.x > 0)
                        {
                            x3 += map.TileSize;
                            x4 += map.TileSize;
                        }
                        else
                        {
                            x3 -= map.TileSize;
                            x4 -= map.TileSize;
                        }
                    }
                    else
                    {
                        raySizeV = Double.PositiveInfinity; //no intersection (max ray size didn't hit the wall)
                        break;
                    }
                }
            }
            else
            {
                raySizeV = Double.PositiveInfinity;
            }

            double raySizeMin;
            int wallHitSide;

            if (raySizeH < raySizeV)
            {
                raySizeMin = raySizeH;
                wallHitSide = sideH;
            }
            else
            {
                raySizeMin = raySizeV;
                wallHitSide = sideV;
            }

            Tuple<double, int, double> rayInfo = new Tuple<double, int, double>(raySizeMin,wallHitSide,direction.GetAngle());
            return rayInfo;

        }
        
        public List<Tuple<double, int, double>> CastRays(int screenWidth) //cast all the rays to fill the screen width
        {
            int maxRaySize = 1500; //how far can player see

            Tuple<double, double> minAngleCoordinates = player.GetDirection().RotateCounterclockwiseSym(player.FOV / 2);
            Vector minAngle = new Vector(minAngleCoordinates.Item1, minAngleCoordinates.Item2); //from where to start casting

            double step = player.FOV / screenWidth; //how much to rotate each step

            List<Tuple<double, int, double>> raysInfo = new List<Tuple<double, int, double>>();
            
            Vector currAngle = minAngle;

            for (int i = 0; i < screenWidth; i++)
            {
                Tuple<double, int, double> rayInfo = Raycast(player.GetPosition().Item1, player.GetPosition().Item2, currAngle, maxRaySize);
                raysInfo.Add(rayInfo);
                currAngle = currAngle.RotateClockwise(step); //rotate a bit
            }

            return raysInfo;
        }
    }
}