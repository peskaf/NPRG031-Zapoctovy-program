using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunner
{
    class Enemy
    {
        private int positionX; //where currently is enemy
        private int positionY; //where currently is enemy
        private int startLocationX; //where does enemy begin
        private int startLocationY; //where does enemy begin
        public int Step { get; } //size of enemy's step
        public Bitmap texture; //enemy's texture -> uploaded to Resources.resx
        public long SpawnTime { get; } //millis to elapse before enemy spawns
        public bool Spawned { get; private set; } //did it spawn?
        public int StepsToMake { get; set; } //how much steps to make before computing next steps (made to reach the middle of each square it needs to visit)
        private Tuple<int, int> nextStepDirection; //which way do I go next?

        public Enemy(int ex, int ey)
        {
            startLocationX = ex;
            startLocationY = ey;
            SpawnTime = 3000;
            Step = 2;
            texture = new Bitmap(Properties.Resources.protector);
            Spawned = false;
            StepsToMake = 0;
        }
        private void SetPosition(int x, int y)
        {
            positionX = x;
            positionY = y;
        }
        public Tuple<int, int> GetPosition()
        {
            Tuple<int, int> position = new Tuple<int, int>(positionX, positionY);
            return position;
        }
        public void Spawn()
        {
            positionX = startLocationX;
            positionY = startLocationY;
            Spawned = true;
        }
        public Tuple<int,int> FindWay(GameMap gameMap) //Find shortest path to reach player's location, uses BFS and remembers predecessors tiles
        {
            List<Tuple<int, int>> nodeList = new List<Tuple<int, int>>();
            Dictionary<Tuple<int, int>, Tuple<int, int>> nodeListPredecessors = new Dictionary<Tuple<int, int>, Tuple<int, int>>();

            Tuple<int, int> playerTilePosition = new Tuple<int, int>(gameMap.player.GetPosition().Item1 / gameMap.map.TileSize, gameMap.player.GetPosition().Item2 / gameMap.map.TileSize);
            Tuple<int, int> enemyTilePosition = new Tuple<int, int>(this.GetPosition().Item1 / gameMap.map.TileSize, this.GetPosition().Item2 / gameMap.map.TileSize);

            void BFS() //bfs to find player
            {
                Queue<Tuple<int, int>> q = new Queue<Tuple<int, int>>();
                nodeList.Add(enemyTilePosition);
                nodeListPredecessors[enemyTilePosition] = null;
                q.Enqueue(enemyTilePosition);

                while (q.Count != 0)
                {
                    Tuple<int, int> v = q.Dequeue();

                    if (gameMap.map.Grid[v.Item1, v.Item2-1] == 0 && nodeList.Contains(new Tuple<int, int>(v.Item1, v.Item2 - 1)) == false) //up, not wall, not found yet
                    {
                        Tuple<int, int> newNode = new Tuple<int, int>(v.Item1,v.Item2 - 1);
                        if (newNode == playerTilePosition) //we found our player
                        {
                            nodeListPredecessors[newNode] = v;
                            break;
                        }
                        nodeList.Add(newNode);
                        nodeListPredecessors[newNode] = v;
                        q.Enqueue(newNode);
                    }
                    if (gameMap.map.Grid[v.Item1, v.Item2 + 1] == 0 && nodeList.Contains(new Tuple<int, int>(v.Item1, v.Item2 + 1)) == false) //down, not wall, not found yet
                    {
                        Tuple<int, int> newNode = new Tuple<int, int>(v.Item1, v.Item2 + 1);
                        if (newNode == playerTilePosition) //we found our player
                        {
                            nodeListPredecessors[newNode] = v;
                            break;
                        }
                        nodeList.Add(newNode);
                        nodeListPredecessors[newNode] = v;
                        q.Enqueue(newNode);
                    }
                    if (gameMap.map.Grid[v.Item1 - 1, v.Item2] == 0 && nodeList.Contains(new Tuple<int, int>(v.Item1 - 1, v.Item2 )) == false) //left, not wall, not found yet
                    {
                        Tuple<int, int> newNode = new Tuple<int, int>(v.Item1 - 1, v.Item2);
                        if (newNode == playerTilePosition) //we found our player
                        {
                            nodeListPredecessors[newNode] = v;
                            break;
                        }
                        nodeList.Add(newNode);
                        nodeListPredecessors[newNode] = v;
                        q.Enqueue(newNode);
                    }
                    if (gameMap.map.Grid[v.Item1 + 1, v.Item2] == 0 && nodeList.Contains(new Tuple<int, int>(v.Item1 + 1, v.Item2)) == false) //right, not wall, not found yet
                    {
                        Tuple<int, int> newNode = new Tuple<int, int>(v.Item1 + 1, v.Item2);
                        if (newNode == playerTilePosition) //we found our player
                        {
                            nodeListPredecessors[newNode] = v;
                            break;
                        }
                        nodeList.Add(newNode);
                        nodeListPredecessors[newNode] = v;
                        q.Enqueue(newNode);
                    }
                }
            }

            Tuple<int,int> NextMove() //reconstruct path to find which way to go next
            {
                Tuple<int, int> lastButOneTile = playerTilePosition;
                
                while (nodeListPredecessors[lastButOneTile] != enemyTilePosition) //find what tile is next to move to
                {
                    lastButOneTile = nodeListPredecessors[lastButOneTile];
                    if (lastButOneTile == null)
                    {
                        return null;
                    }
                }
                return lastButOneTile;
            }

            BFS();
            return NextMove();
        }
        public void Move(GameMap gameMap) //handle moving -> move it if it has steps to make, if not, compute it's way and move that way
        {
            if (StepsToMake > 0)
            {
                SetPosition((GetPosition().Item1 + nextStepDirection.Item1 * Step), (GetPosition().Item2 + nextStepDirection.Item2 * Step));
                StepsToMake--;
            }
            else
            {
                Tuple <int,int> nextTile = FindWay(gameMap);
                if (nextTile == null)
                {
                    nextStepDirection = new Tuple<int, int>(0, 0);
                    return;
                }
                nextStepDirection = new Tuple<int, int>((nextTile.Item1 - positionX / gameMap.map.TileSize), (nextTile.Item2 - positionY / gameMap.map.TileSize));
                StepsToMake = gameMap.map.TileSize / Step;
                Move(gameMap); //will go to fisrt branch now
            }
        }
    }
}
