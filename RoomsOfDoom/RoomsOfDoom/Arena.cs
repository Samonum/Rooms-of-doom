using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    [Flags]
    public enum Exit : byte
    {
        Top = 1,
        Bot = 2,
        Left = 4,
        Right = 8
    }

    public class Arena
    {
        Random random;

        private char[][] map;
        private int width = 37, height = 25;
        private Exit exits;
        private int topExit, leftExit, rightExit, botExit;

        private Pack enemies;
        private Player player;

        public Arena(Exit openExits, Pack enemies, Player player, Exit entrance, Random random)
        {
            this.random = random;

            exits = openExits;
            topExit = 10 + random.Next(width - 20);
            leftExit = 10 + random.Next(height - 20);
            rightExit = 10 + random.Next(height - 20);
            botExit = 10 + random.Next(width - 20);

            this.enemies = enemies;
            PlaceEnemies(enemies);

            this.player = player;
            PlacePlayer(entrance);
        }

        public void PlacePlayer(Exit entrance)
        {
            switch (entrance)
            {
                case Exit.Top:
                    player.Location = new Point(topExit, 2);
                    break;
                case Exit.Bot:
                    player.Location = new Point(botExit, height - 3);
                    break;
                case Exit.Right:
                    player.Location = new Point(width - 3, rightExit);
                    break;
                case Exit.Left:
                    player.Location = new Point(2, leftExit);
                    break;
                default:
                    player.Location = new Point(random.Next(width - 8) + 4, random.Next(height - 8) + 4);
                    for (int i = 0; i < enemies.Size; i++)
                    {
                        if (enemies[i].Location == player.Location)
                        {
                            i = 0;
                            player.Location = new Point(random.Next(width - 5) + 1, random.Next(height - 5) + 1);
                            break;
                        }
                    }
                    break;
            }
        }

        public void PlaceEnemies(Pack enemies)
        {
            for (int i = 0; i < enemies.Size; i++)
            {
                enemies[i].Location = new Point(random.Next(width - 8) + 4, random.Next(height - 8) + 4);
                for (int j = 0; j < i; j++)
                    if (enemies[i].Location == enemies[j].Location)
                    {
                        i--;
                        break;
                    }
            }
        }

        public void UpdateMap()
        {
            CreateBackground();
            foreach (Enemy e in enemies)
                map[e.Location.Y][e.Location.X] = e.Glyph;
            map[player.Location.Y][player.Location.X] = player.Glyph;
        }

        private void CreateBackground()
        {
            map = new char[height][];
            map[0] = new char[width];
            for (int j = 0; j < map[0].Length; j++)
                if ((exits & Exit.Top) != Exit.Top || j < topExit - 2 || j > topExit + 2)
                    map[0][j] = '█';
                else
                    map[0][j] = '▒';
            for (int i = 1; i < map.Length - 1; i++)
            {
                map[i] = new char[width];
                for (int j = 0; j < map[i].Length; j++)
                    if (j == 0)
                    {
                        if ((exits & Exit.Left) != Exit.Left || i < leftExit - 2 || i > leftExit + 2)
                            map[i][j] = '█';
                        else
                            map[i][j] = '▒';
                    }
                    else if (j == map[i].Length - 1)
                    {
                        if ((exits & Exit.Right) != Exit.Right || i < rightExit - 2 || i > rightExit + 2)
                            map[i][j] = '█';
                        else
                            map[i][j] = '▒';
                    }
                    else
                        map[i][j] = '.';
                map[map.Length - 1] = new char[width];
                for (int j = 0; j < map[0].Length; j++)
                    if ((exits & Exit.Bot) != Exit.Bot || j < botExit - 2 || j > botExit + 2)
                        map[map.Length - 1][j] = '█';
                    else
                        map[map.Length - 1][j] = '▒';
            }
        }

        public void Draw()
        {
            for (int i = 0; i < map.Length; i++)
                Console.WriteLine(map[i]);
        }
    }
}
