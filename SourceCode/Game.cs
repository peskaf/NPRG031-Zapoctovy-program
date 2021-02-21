using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MazeRunner
{
    public partial class MazeRunner : Form
    {
        private GameMap gameMap;  
        bool keyW = false;
        bool keyS = false;
        bool keyA = false;
        bool keyD = false;
        int newX, newY;
        long timeElapsed;

        public MazeRunner()
        {
            InitializeComponent();
            GameOver.Visible = false;
            timer1.Enabled = false;
            Victory.Visible = false;
        }
        private void Render(GameMap gameMap)
        {
            BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
            BufferedGraphics myBuffer = currentContext.Allocate(CreateGraphics(), DisplayRectangle);

            //sky and floor
            Rectangle sky = new Rectangle(0, 0, DisplayRectangle.Width, DisplayRectangle.Height / 2);
            Rectangle floor = new Rectangle(0, DisplayRectangle.Height / 2, DisplayRectangle.Width, DisplayRectangle.Height / 2);
            myBuffer.Graphics.FillRectangle(Brushes.Gray, sky);
            myBuffer.Graphics.FillRectangle(Brushes.Green, floor);

            //walls
            List<Tuple<double, int, double>> rays = gameMap.CastRays(DisplayRectangle.Width); //in Tuple -> ray size, index of direction the ray hit the wall, angle the ray was casted in
            int x = 0;
            
            foreach (Tuple<double, int, double> ray in rays)
            {
                float ca = Convert.ToSingle(gameMap.player.GetDirection().GetAngle() - ray.Item3);
                if (ca < 0)
                {
                    ca += Convert.ToSingle(2 * Math.PI);
                }
                else if (ca > 0)
                {
                    ca -= Convert.ToSingle(2 * Math.PI);
                }
                float disT = Convert.ToSingle(ray.Item1 * Math.Cos(ca));
                float wallHeight = Convert.ToSingle((gameMap.map.TileSize * DisplayRectangle.Height) / (disT));

                if (wallHeight > DisplayRectangle.Height)
                {
                    wallHeight = DisplayRectangle.Height;
                }

                float startY = (DisplayRectangle.Height - Convert.ToSingle(wallHeight)) / 2;
                var color = ray.Item2 switch
                {
                    1 => Pens.Blue,
                    2 => Pens.LightBlue,
                    3 => Pens.DodgerBlue,
                    4 => Pens.DarkBlue,
                    _ => Pens.Red, //default -> won't happen
                };
                myBuffer.Graphics.DrawLine(color, x, startY, x, Convert.ToSingle(startY + wallHeight));

                x++;
            }

            //enemy - billboarding
            if (gameMap.enemy.Spawned == true)
            {
                Vector normalVectorToPlayersDirection = new Vector(gameMap.player.GetDirection().y * Math.Tan(gameMap.player.FOV / 2), -gameMap.player.GetDirection().x * Math.Tan(gameMap.player.FOV / 2));

                int enemysRelativePositionX = gameMap.enemy.GetPosition().Item1 - gameMap.player.GetPosition().Item1;
                int enemysRelativePositionY = -(gameMap.enemy.GetPosition().Item2 - gameMap.player.GetPosition().Item2);

                double invDet = 1.0 / (normalVectorToPlayersDirection.x * gameMap.player.GetDirection().y - gameMap.player.GetDirection().x * normalVectorToPlayersDirection.y); //just for matrix multiplication (to transform X)

                double transformX = invDet * (gameMap.player.GetDirection().y * enemysRelativePositionX - gameMap.player.GetDirection().x * enemysRelativePositionY);
                double transformY = enemysRelativePositionX * gameMap.player.GetDirection().x + enemysRelativePositionY * gameMap.player.GetDirection().y;

                int spriteScreenX = (int)((DisplayRectangle.Width / 2) * (1 + transformX / transformY)); //sprite screen coordinates

                //calculate size of the sprite on screen
                float spriteSize = Convert.ToSingle((gameMap.map.TileSize * DisplayRectangle.Height) / transformY); //sprite screen size (width=height)

                //calculate highest pixel (even if not on screen)
                int drawStartY = (int)(-spriteSize / 2 + DisplayRectangle.Height / 2);

                //calculate leftmost pixel (even if not on screen)
                int drawStartX = (int)(-spriteSize / 2 + spriteScreenX);

                if (transformY > 30) //if sprite more than 30 units away -> draw it
                {
                    for (int stripe = drawStartX; stripe < drawStartX + spriteSize + 1; stripe++)
                    {
                        if (stripe > 0 && stripe < DisplayRectangle.Width && rays[stripe].Item1 * Math.Cos(gameMap.player.GetDirection().GetAngle() - rays[stripe].Item3) > transformY)
                        {
                            int texX = (int)((stripe - (-spriteSize / 2 + spriteScreenX)) * gameMap.enemy.texture.Width / spriteSize); //x coordinate in sprite texture
                            Rectangle destRect = new Rectangle(stripe, drawStartY, 1, (int)spriteSize); //where to display current stripe of sprite texture
                            myBuffer.Graphics.DrawImage(gameMap.enemy.texture, destRect, texX, 0, 1, gameMap.enemy.texture.Height, GraphicsUnit.Pixel);
                        }
                    }
                }
            }
            
            //minimap
            int squareSize = 7; //size of one tile on minimap
            int offset = 10;
            x = DisplayRectangle.Width - gameMap.map.SideSize*squareSize - offset; //last number is the offset from the edge of the screen
            int y = offset;
            for (int i = 0; i < gameMap.map.SideSize; i++)
            {
                for (int j = 0; j < gameMap.map.SideSize; j++)
                {
                    Rectangle r = new Rectangle(x, y, squareSize, squareSize);
                    if (gameMap.map.Grid[j, i] == 1)
                    {
                        myBuffer.Graphics.FillRectangle(Brushes.Black, r);
                    }
                    else
                    {
                        myBuffer.Graphics.FillRectangle(Brushes.Yellow, r);
                    }
                    x += squareSize;
                }
                y += squareSize;
                x = DisplayRectangle.Width - gameMap.map.SideSize * squareSize - offset;
            }

            //player on minimap
            Rectangle player = new Rectangle(DisplayRectangle.Width - gameMap.map.SideSize * squareSize - offset + (gameMap.player.GetPosition().Item1 / gameMap.map.TileSize) * squareSize + 1, offset + (gameMap.player.GetPosition().Item2 / gameMap.map.TileSize) * squareSize + 1, squareSize-2, squareSize-2); //works if squareSize is odd and >1
            myBuffer.Graphics.FillRectangle(Brushes.Blue, player);

            //enemy on minimap
            if (gameMap.enemy.Spawned == true)
            {
                Rectangle enemy = new Rectangle(DisplayRectangle.Width - gameMap.map.SideSize * squareSize - offset + (gameMap.enemy.GetPosition().Item1 / gameMap.map.TileSize) * squareSize + 1, offset + (gameMap.enemy.GetPosition().Item2 / gameMap.map.TileSize) * squareSize + 1, squareSize - 2, squareSize - 2); //works if squareSize is odd and >1
                myBuffer.Graphics.FillRectangle(Brushes.Red, enemy);
            }
            
            //finish on minimap
            Rectangle finish = new Rectangle(DisplayRectangle.Width - gameMap.map.SideSize * squareSize - offset + (gameMap.map.SideSize - 2) * squareSize + 1, offset + (gameMap.map.SideSize - 2) * squareSize + 1, squareSize - 2, squareSize - 2); //works if squareSize is odd and >1
            myBuffer.Graphics.FillRectangle(Brushes.LimeGreen, finish);

            //render everything
            myBuffer.Render();
            myBuffer.Dispose();
        }
        private void timer1_Tick(object sender, EventArgs e) //game loop
        {
            //render
            Render(gameMap);

            //keyboard input handling
            if (keyW)
            {
                newX = Convert.ToInt32(gameMap.player.GetPosition().Item1 + gameMap.player.GetDirection().x * gameMap.player.Step);
                newY = Convert.ToInt32(gameMap.player.GetPosition().Item2 - gameMap.player.GetDirection().y * gameMap.player.Step);
                if (Math.Abs((newX / gameMap.map.TileSize)-gameMap.player.GetPosition().Item1) != 1 || Math.Abs((newY / gameMap.map.TileSize) - gameMap.player.GetPosition().Item2) != 1) //check if the player would skip the corner
                {
                    if (gameMap.map.Grid[(newX / gameMap.map.TileSize), (newY / gameMap.map.TileSize)] == 0)
                    {
                        gameMap.player.SetPosition(newX, newY);
                    }
                    else if (gameMap.map.Grid[(newX / gameMap.map.TileSize), (gameMap.player.GetPosition().Item2 / gameMap.map.TileSize)] == 0) //slide to x side
                    {
                        gameMap.player.SetPosition(newX, gameMap.player.GetPosition().Item2);
                    }
                    else if (gameMap.map.Grid[(gameMap.player.GetPosition().Item1 / gameMap.map.TileSize), (newY / gameMap.map.TileSize)] == 0) //slide to y side
                    {
                        gameMap.player.SetPosition(gameMap.player.GetPosition().Item1, newY);
                    }
                }
            }
            if (keyS)
            {
                newX = Convert.ToInt32(gameMap.player.GetPosition().Item1 - gameMap.player.GetDirection().x * gameMap.player.Step);
                newY = Convert.ToInt32(gameMap.player.GetPosition().Item2 + gameMap.player.GetDirection().y * gameMap.player.Step);
                if (Math.Abs((newX / gameMap.map.TileSize) - gameMap.player.GetPosition().Item1) != 1 || Math.Abs((newY / gameMap.map.TileSize) - gameMap.player.GetPosition().Item2) != 1) //check if the player would skip the corner
                {
                    if (gameMap.map.Grid[(newX / gameMap.map.TileSize), (newY / gameMap.map.TileSize)] == 0)
                    {
                        gameMap.player.SetPosition(newX, newY);
                    }
                    else if (gameMap.map.Grid[(newX / gameMap.map.TileSize), (gameMap.player.GetPosition().Item2 / gameMap.map.TileSize)] == 0) //slide to x side
                    {
                        gameMap.player.SetPosition(newX, gameMap.player.GetPosition().Item2);
                    }
                    else if (gameMap.map.Grid[(gameMap.player.GetPosition().Item1 / gameMap.map.TileSize), (newY / gameMap.map.TileSize)] == 0) //slide to y side
                    {
                        gameMap.player.SetPosition(gameMap.player.GetPosition().Item1, newY);
                    }
                }
            }
            if (keyA)
            {
                gameMap.player.SetDirection(gameMap.player.GetDirection().RotateCounterclockwise((gameMap.player.rotationSpeed * Math.PI) / 180));
            }
            if (keyD)
            {
                gameMap.player.SetDirection(gameMap.player.GetDirection().RotateClockwise((gameMap.player.rotationSpeed * Math.PI) / 180));
            }

            //game over/victory conditions check
            if ((gameMap.player.GetPosition().Item1 > ((gameMap.map.SideSize- 2)*gameMap.map.TileSize) && gameMap.player.GetPosition().Item2 > ((gameMap.map.SideSize - 2) * gameMap.map.TileSize)) || gameMap.player.Killed) //end of the game
            {
                timer1.Stop();
                Menu.Visible = true;
                NewGame.Visible = true;
                Exit.Visible = true;
                if (gameMap.player.Killed)
                {
                    GameOver.Visible = true;
                }
                else
                {
                    Victory.Visible = true;
                }
            }

            //killed condition check
            if ((gameMap.enemy.GetPosition().Item1/gameMap.map.TileSize) == (gameMap.player.GetPosition().Item1 / gameMap.map.TileSize) && (gameMap.enemy.GetPosition().Item2 / gameMap.map.TileSize) == (gameMap.player.GetPosition().Item2 / gameMap.map.TileSize))
            {
                gameMap.player.Killed = true;
            }

            //enemy spawntime check and spawn handle
            if (gameMap.enemy.Spawned == false && timeElapsed >= gameMap.enemy.SpawnTime)
            {
                gameMap.enemy.Spawn();
            }
            else if (timeElapsed < gameMap.enemy.SpawnTime)
            {
                timeElapsed += timer1.Interval;
            }

            //enemy movement handle
            if (gameMap.enemy.Spawned)
            {
                gameMap.enemy.Move(gameMap);
            }
        }
        private void MazeRunner_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
                keyW = false;
            else if (e.KeyCode == Keys.S)
                keyS = false;
            else if (e.KeyCode == Keys.A)
                keyA = false;
            else if (e.KeyCode == Keys.D)
                keyD = false;
        }
        private void NewGame_Click(object sender, EventArgs e)
        {
            gameMap = new GameMap(21); //use odd number > 3!!
            Menu.Visible = false;
            NewGame.Visible = false;
            GameOver.Visible = false;
            Victory.Visible = false;
            Exit.Visible = false;
            timeElapsed = 0;
            timer1.Start();
        }
        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void MazeRunner_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
                keyW = true;
            else if (e.KeyCode == Keys.S)
                keyS = true;
            else if (e.KeyCode == Keys.A)
                keyA = true;
            else if (e.KeyCode == Keys.D)
                keyD = true;
        }
    }
}
