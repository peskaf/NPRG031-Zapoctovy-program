using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunner
{
    class Map
    {
        public int SideSize { get; } //number of tiles on one side (width=height)
        public int[,] Grid { get; private set; } //grid where info about -> 1 .. wall / 0 .. no wall, is stored
        public int TileSize { get; } //size of one tile in the grid (width=height)

        public Map(int sideSize)
        {
            SideSize = sideSize; //must be odd and > 3
            Grid = new int[SideSize, SideSize];
            TileSize = 64; //good to be even and power of 2
            BuildMaze();
        }
        private void CreateFoundation() //creation of template to build maze in
        {
            //0 = free space, 1 = wall, 2 = foundation
            for (int i = 0; i < SideSize; i++) 
            {
                Grid[i,0] = 1; //first wall
                Grid[i, SideSize - 1] = 1; //last wall
            }
            for (int j = 1; j < SideSize; j += 2)
            {
                Grid[0, j] = 1;
                for (int i = 1; i < SideSize - 1; i++)
                {
                    Grid[i, j] = 0; //free spaces
                }
                Grid[SideSize - 1, j] = 1;
            }
            for (int j = 2; j < SideSize - 1; j += 2)
            {
                Grid[0, j] = 1;
                for (int i = 1; i < SideSize - 1; i++)
                {
                    Grid[i++, j] = 0; //foundations side
                    Grid[i, j] = 2;
                }
                Grid[SideSize - 1, j] = 1;
            }
        }
        private int FoundationsLeft() //how many foundations are left in grid?
        {
            int numOfFoundations = 0;
            for (int i = 1; i < SideSize - 1; i++)
            {
                for (int j = 1; j < SideSize - 1; j++)
                {
                    if (Grid[i,j] == 2)
                    {
                        numOfFoundations++;
                    }
                }
            }
            return numOfFoundations;
        }
        private Tuple<int,int>? ChooseRandomFoundation(Random random) //choose random foundation to build wall from
        {
            int numOfFoundationsMet = 0;
            int indexOfFoundation = random.Next(1,FoundationsLeft()+1);
            for (int i = 1; i < SideSize - 1; i++)
            {
                for (int j = 1; j < SideSize - 1; j++)
                {
                    if (Grid[i, j] == 2)
                    {
                        numOfFoundationsMet++;
                        if (numOfFoundationsMet == indexOfFoundation)
                        {
                            Tuple<int, int> output = new Tuple<int, int>(i,j);
                            return output;
                        }
                    }
                }
            }
            return null; //won't happen
        }
        private void BuildWall(Random random) //build wall from that random foundation
        {
            Tuple<int, int> randomFoundation = ChooseRandomFoundation(random);
            int i = randomFoundation.Item1;
            int j = randomFoundation.Item2;
            int randomOption = random.Next(1, 5); //1..up, 2..down, 3..left, 4..right

            switch (randomOption)
            {
                case 1:
                    while (Grid[i, j] != 1)
                    {
                        Grid[i--, j] = 1;
                    }
                    break;
                case 2:
                    while (Grid[i, j] != 1)
                    {
                        Grid[i++, j] = 1;
                    }  
                    break;
                case 3:
                    while (Grid[i, j] != 1)
                    {
                        Grid[i, j--] = 1;
                    } 
                    break;
                case 4:
                    while (Grid[i, j] != 1)
                    {
                        Grid[i, j++] = 1;
                    }
                    break;
                default:
                    break;   
            }
        }
        private void BuildMaze() //handle all building functions
        {
            Random random = new Random();
            CreateFoundation();
            while (FoundationsLeft() != 0)
            {
                BuildWall(random);
            }

        }
        private void PrintGrid() //For debug only
        {
            for (int i = 0; i < SideSize; i++)
            {
                for (int j = 0; j < SideSize; j++)
                {
                    Console.Write(string.Format("{0} ", Grid[i, j]));
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
        }
    }
}
