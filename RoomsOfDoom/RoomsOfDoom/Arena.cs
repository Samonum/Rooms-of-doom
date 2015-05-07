using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using RoomsOfDoom.Items;

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
        public const int Width = 37, Height = 25;
        private Exit exits;
        private int topExit, leftExit, rightExit, botExit;

        public Pack enemies;
        private Player player;

        public Arena(Exit openExits, Pack enemies, Player player, Exit entrance, Random random)
        {
            this.random = random;

            exits = openExits;
            topExit = 10 + random.Next(Width - 20);
            leftExit = 10 + random.Next(Height - 20);
            rightExit = 10 + random.Next(Height - 20);
            botExit = 10 + random.Next(Width - 20);

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
                    player.Location = new Point(botExit, Height - 3);
                    break;
                case Exit.Right:
                    player.Location = new Point(Width - 3, rightExit);
                    break;
                case Exit.Left:
                    player.Location = new Point(2, leftExit);
                    break;
                default:
                    player.Location = new Point(random.Next(Width - 8) + 4, random.Next(Height - 8) + 4);
                    for (int i = 0; i < enemies.Size; i++)
                    {
                        if (enemies[i].Location == player.Location)
                        {
                            i = 0;
                            player.Location = new Point(random.Next(Width - 5) + 1, random.Next(Height - 5) + 1);
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
                enemies[i].Location = new Point(random.Next(Width - 8) + 4, random.Next(Height - 8) + 4);
                for (int j = 0; j < i; j++)
                    if (enemies[i].Location == enemies[j].Location)
                    {
                        i--;
                        break;
                    }
            }
        }

        public void HandleCombatRound(char input)
        {
            switch (input)
            {
                case 'w': 
                    player.Move(Direction.Up, enemies);
                    break;
                case 'a': 
                    player.Move(Direction.Left, enemies);
                    break;
                case 's': 
                    player.Move(Direction.Down, enemies);
                    break;
                case 'd': 
                    player.Move(Direction.Right, enemies);
                    break;
                case '1': 
                    player.UseItem(new Potion(), null);
                    break;
                case '2': 
                    player.UseItem(new TimeCrystal(), null);
                    break;
                case '3': 
                    player.UseItem(new MagicScroll(random), null);
                    break;
            }

            foreach (Enemy e in enemies)
            {
                e.Move(player);
            }
        }

        public char[][] GetUpdatedMap()
        {
            CreateBackground();
            foreach (Enemy e in enemies)
                if(e.Alive)
                    map[e.Location.Y][e.Location.X] = e.Glyph;
            map[player.Location.Y][player.Location.X] = player.Glyph;
            return map;
        }

        private void CreateBackground()
        {
            map = new char[Height][];
            map[0] = new char[Width];
            for (int j = 0; j < map[0].Length; j++)
                if ((exits & Exit.Top) != Exit.Top || j < topExit - 2 || j > topExit + 2)
                    map[0][j] = '█';
                else
                    map[0][j] = '▒';
            for (int i = 1; i < map.Length - 1; i++)
            {
                map[i] = new char[Width];
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
                map[map.Length - 1] = new char[Width];
                for (int j = 0; j < map[0].Length; j++)
                    if ((exits & Exit.Bot) != Exit.Bot || j < botExit - 2 || j > botExit + 2)
                        map[map.Length - 1][j] = '█';
                    else
                        map[map.Length - 1][j] = '▒';
            }
        }
    }
}
