﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using RoomsOfDoom.Items;

namespace RoomsOfDoom
{
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

        public Arena(Node startNode, Player player, Random random)
        {
            this.random = random;
            this.player = player;
            InitRoom(startNode);
        }

        public void InitRoom(Node newNode)
        {
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
                enemies = new Pack(0);
            else
                enemies = node.PackList[0];

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

            foreach (Enemy e in enemies)
            {
                e.Move(player);
            }

            if(enemies.Size == 0)
            {
                node.RemovePack(enemies);
                if (node.PackList.Count > 0)
                {
                    enemies = node.PackList[0];
                    PlaceEnemies(enemies);
                }
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
