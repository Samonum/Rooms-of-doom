using RoomsOfDoom.Items;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RoomsOfDoom
{
    public partial class Node
    {
        private const int minDoorSize = 1;
        private const int maxDoorSize = 3;

        private const int Width = 37, Height = 25;

        public Exit exits;

        public List<Loot> lootList;

        public int topLocation, botLocation, leftLocation, rightLocation;
        public int topSize, botSize, leftSize, rightSize;

        public void InitSizes()
        {
            topSize = random.Next(minDoorSize, maxDoorSize + 1);
            botSize = random.Next(minDoorSize, maxDoorSize + 1);
            leftSize = random.Next(minDoorSize, maxDoorSize + 1);
            rightSize = random.Next(minDoorSize, maxDoorSize + 1);

            int offset = 2 + topSize;
            topLocation = offset + random.Next(Width - offset * 2);

            offset = 2 + botSize;
            botLocation = offset + random.Next(Width - offset * 2);

            offset = 2 + leftSize;
            leftLocation = offset + random.Next(Height - offset * 2);

            offset = 2 + rightSize;
            rightLocation = offset + random.Next(Height - offset * 2);

            lootList = new List<Loot>();

            if (IsExit)
            {
                Loot key = new Loot(3, '>');
                key.Location = GetRandomLocation(8);
                lootList.Add(key);
            }
        }

        public bool WithinTopGate(int x)
        {
            if ((exits & Exit.Top) != Exit.Top)
                return false;
            if (topLocation - topSize > x || topLocation + topSize < x)
                return false;
            return true;
        }

        public bool WithinBotGate(int x)
        {
            if ((exits & Exit.Bot) != Exit.Bot)
                return false;
            if (botLocation - botSize > x || botLocation + botSize < x)
                return false;
            return true;
        }

        public bool WithinLeftGate(int x)
        {
            if ((exits & Exit.Left) != Exit.Left)
                return false;
            if (leftLocation - leftSize > x || leftLocation + leftSize < x)
                return false;
            return true;
        }
        
        public bool WithinRightGate(int x)
        {
            if ((exits & Exit.Right) != Exit.Right)
                return false;
            if (rightLocation - rightSize > x || rightLocation + rightSize < x)
                return false;
            return true;
        }

        public int TopExit
        {
            get { return topLocation; }
        }

        public int BotExit
        {
            get { return botLocation; }
        }

        public int LeftExit
        {
            get { return leftLocation; }
        }

        public int RightExit
        {
            get { return rightLocation; }
        }

        public Pack CurrentPack
        {
            get 
            {
                if (packs.Count == 0)
                    return null;
                return packs[0];
            }
        }

        private Point GetRandomLocation(int distFromWall)
        {
            return new Point(random.Next(Width - distFromWall - 2) + 1 + distFromWall / 2,
                random.Next(Height - distFromWall - 2) + 1 + distFromWall / 2);
        }

        private char[][] CreateBackground()
        {

            char[][] map = new char[Height][];
            map[0] = new char[Width];
            for (int j = 0; j < map[0].Length; j++)
                if (!WithinTopGate(j))
                    map[0][j] = '█';
                else
                    map[0][j] = '▒';
            for (int i = 1; i < map.Length - 1; i++)
            {
                map[i] = new char[Width];
                for (int j = 0; j < map[i].Length; j++)
                    if (j == 0)
                    {
                        if (!WithinLeftGate(i))
                            map[i][j] = '█';
                        else
                            map[i][j] = '▒';
                    }
                    else if (j == map[i].Length - 1)
                    {
                        if (!WithinRightGate(i))
                            map[i][j] = '█';
                        else
                            map[i][j] = '▒';
                    }
                    else
                        map[i][j] = '.';

                map[map.Length - 1] = new char[Width];
                for (int j = 0; j < map[0].Length; j++)
                    if (!WithinBotGate(j))
                        map[map.Length - 1][j] = '█';
                    else
                        map[map.Length - 1][j] = '▒';
            }

            return map;
        }

        public char[][] GetUpdatedMap(Point playerLocation, char playerGlyph)
        {
            char[][] map = CreateBackground();
            foreach (Loot l in lootList)
                map[l.Location.Y][l.Location.X] = l.Glyph;
            if (CurrentPack != null)
                foreach (Enemy e in CurrentPack)
                    if (e.Alive)
                        map[e.Location.Y][e.Location.X] = e.Glyph;
            map[playerLocation.Y][playerLocation.X] = playerGlyph;
            return map;
        }

        public void PlaceEnemies()
        {
            if (CurrentPack == null)
                return;

            for (int i = 0; i < CurrentPack.Size; i++)
            {
                CurrentPack[i].Location = GetRandomLocation(4);
                for (int j = 0; j < i; j++)
                    if (CurrentPack[i].Location == CurrentPack[j].Location)
                    {
                        i--;
                        break;
                    }
                // Check for dem enemies on player locationd
                    /*
                    else if (CurrentPack[i].Location == player.Location)
                        i--;
                     * */
            }
        }

        // TODO: Adjust move, no longer give player
        public void MicroUpdates(Player player)
        { 
            if (CurrentPack == null)
                return;

            if (CurrentPack.CurrentPackHP >= (0.3 * CurrentPack.MaxPackHP))
            {
                foreach (Enemy e in CurrentPack)
                {
                    // TODO: Plz move
                    e.AggressiveMove(player);
                }
            }
            else
            {
                //get random door to flee to
                // TODO: Change the 5
                Point doorLocation = new Point(5, 2);
                foreach (Enemy e in CurrentPack)
                {
                    //FLY YOU FOOLS!
                    e.NeutralMove(doorLocation);
                }
            }


            if (CurrentPack.Size == 0)
            {
                RemovePack(CurrentPack);

                Loot loot = ItemGenerator.GetItem(Multiplier);
                if (loot != null)
                {
                    loot.Location = GetRandomLocation(4);
                    lootList.Add(loot);
                }

                if (CurrentPack != null)
                    PlaceEnemies();
            }
        }
    }
}
