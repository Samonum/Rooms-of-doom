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
        private const int doorsize = 3;
        Random random;

        private char[][] map;
        public const int Width = 37, Height = 25;
        private Exit exits;
        private int topExit, leftExit, rightExit, botExit;
        private Node node;

        public Pack enemies;
        int curPack;
        private Player player;

        bool inCombat;

        List<IItem> items;
        ItemGenerator itemGenerator;

        public Arena(Node startNode, Player player, Random random, ItemGenerator generator)
        {
            this.random = random;
            this.player = player;
            this.itemGenerator = generator;
            InitRoom(startNode);
        }

        public void InitRoom(Node newNode)
        {
            items = new List<IItem>(2);

            Exit entrance = 0;
            foreach (KeyValuePair<Exit, Node> n in newNode.AdjacencyList)
                if (n.Value == node)
                    entrance = n.Key;

            this.node = newNode;
            player.ScoreMultiplier = node.Multiplier;

            exits = 0;
            foreach (KeyValuePair<Exit, Node> exit in node.AdjacencyList)
                exits |= exit.Key;
            topExit = 10 + random.Next(Width - 20);
            leftExit = 10 + random.Next(Height - 20);
            rightExit = 10 + random.Next(Height - 20);
            botExit = 10 + random.Next(Width - 20);

            if (node.PackList.Count == 0)
            {
                inCombat = false;
                enemies = new Pack(0);
            }
            else
            {
                inCombat = true;
                enemies = node.PackList[0];
            }

            PlaceEnemies(enemies);
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
                    player.Location = GetRandomLocation(4);
                    for (int i = 0; i < enemies.Size; i++)
                    {
                        if (enemies[i].Location == player.Location)
                        {
                            i = 0;
                            player.Location = GetRandomLocation(3);
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
                enemies[i].Location = GetRandomLocation(4);
                for (int j = 0; j < i; j++)
                    if (enemies[i].Location == enemies[j].Location)
                    {
                        i--;
                        break;
                    }
                    else if (enemies[i].Location == player.Location)
                        i--;
            }
        }

        public void HandleCombatRound(char input)
        {
            switch (input)
            {
                case 'w':
                    if (!player.Move(Direction.Up, enemies))
                        if ((exits & Exit.Top) == Exit.Top)
                            if (player.Location.X > topExit - doorsize && player.Location.X < topExit + doorsize)
                                InitRoom(node.AdjacencyList[Exit.Top]);
                    ;
                    break;
                case 'a': 
                    if (!player.Move(Direction.Left, enemies))
                        if ((exits & Exit.Left) == Exit.Left)
                            if (player.Location.Y > leftExit - doorsize && player.Location.Y < leftExit + doorsize)
                                InitRoom(node.AdjacencyList[Exit.Left]);
                    break;
                case 's': 
                    if (!player.Move(Direction.Down, enemies))
                        if ((exits & Exit.Bot) == Exit.Bot)
                            if (player.Location.X > botExit - doorsize && player.Location.X < botExit + doorsize)
                                InitRoom(node.AdjacencyList[Exit.Bot]);
                    break;
                case 'd': 
                    if (!player.Move(Direction.Right, enemies))
                        if ((exits & Exit.Right) == Exit.Right)
                            if (player.Location.Y > rightExit - doorsize && player.Location.Y < rightExit + doorsize)
                                InitRoom(node.AdjacencyList[Exit.Right]);
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
            TryPickUpLoot();
            UpdateEnemies();
        }

        public void TryPickUpLoot()
        {
            for(int i = 0; i < items.Count; i++)
                if(player.Location == items[i].Location)
                {
                    player.AddItem(items[i]);
                    items.RemoveAt(i);
                }
        }

        public void UpdateEnemies()
        {
            foreach (Enemy e in enemies)
            {
                e.Move(player);
            }

            if (enemies.Size == 0)
            {
                node.RemovePack(enemies);
                if (inCombat)
                {
                    IItem loot = itemGenerator.GetItem(node.Multiplier);
                    if (loot != null)
                    {
                        loot.Location = GetRandomLocation(4);
                        items.Add(loot);
                    }
                }
                inCombat = false;

                if (node.PackList.Count > 0)
                {
                    enemies = node.PackList[0];
                    inCombat = true;
                    PlaceEnemies(enemies);
                }
            }
        }

        public char[][] GetUpdatedMap()
        {
            CreateBackground();
            foreach (ITile i in items)
                map[i.Location.Y][i.Location.X] = i.Glyph;

            foreach (Enemy e in enemies)
                if(e.Alive)
                    map[e.Location.Y][e.Location.X] = e.Glyph;
            map[player.Location.Y][player.Location.X] = player.Glyph;
            return map;
        }

        public Point GetRandomLocation(int distFromWall)
        {
            return new Point(random.Next(Width - distFromWall - 2) + 1 + distFromWall / 2,
                random.Next(Height - distFromWall - 2) + 1 + distFromWall / 2);
        }

        private void CreateBackground()
        {
            map = new char[Height][];
            map[0] = new char[Width];
            for (int j = 0; j < map[0].Length; j++)
                if ((exits & Exit.Top) != Exit.Top || j <= topExit - doorsize || j >= topExit + doorsize)
                    map[0][j] = '█';
                else
                    map[0][j] = '▒';
            for (int i = 1; i < map.Length - 1; i++)
            {
                map[i] = new char[Width];
                for (int j = 0; j < map[i].Length; j++)
                    if (j == 0)
                    {
                        if ((exits & Exit.Left) != Exit.Left || i <= leftExit - doorsize || i >= leftExit + doorsize)
                            map[i][j] = '█';
                        else
                            map[i][j] = '▒';
                    }
                    else if (j == map[i].Length - 1)
                    {
                        if ((exits & Exit.Right) != Exit.Right || i <= rightExit - doorsize || i >= rightExit + doorsize)
                            map[i][j] = '█';
                        else
                            map[i][j] = '▒';
                    }
                    else
                        map[i][j] = '.';
                map[map.Length - 1] = new char[Width];
                for (int j = 0; j < map[0].Length; j++)
                    if ((exits & Exit.Bot) != Exit.Bot || j <= botExit - doorsize || j >= botExit + doorsize)
                        map[map.Length - 1][j] = '█';
                    else
                        map[map.Length - 1][j] = '▒';
            }
        }
    }
}