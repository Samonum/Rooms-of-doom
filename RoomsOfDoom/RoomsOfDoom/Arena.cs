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

        public Node fleeNode;
        public Point fleeLocation;

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

        #region Check if within a gate

        public bool WithinTopGate(int x)
        {
            if (locked)
                return false;
            if ((exits & Exit.Top) != Exit.Top)
                return false;
            if (topLocation - topSize > x || topLocation + topSize < x)
                return false;
            return true;
        }

        public bool WithinBotGate(int x)
        {
            if (locked)
                return false;
            if ((exits & Exit.Bot) != Exit.Bot)
                return false;
            if (botLocation - botSize > x || botLocation + botSize < x)
                return false;
            return true;
        }

        public bool WithinLeftGate(int x)
        {
            if (locked)
                return false;
            if ((exits & Exit.Left) != Exit.Left)
                return false;
            if (leftLocation - leftSize > x || leftLocation + leftSize < x)
                return false;
            return true;
        }
        
        public bool WithinRightGate(int x)
        {
            if (locked)
                return false;
            if ((exits & Exit.Right) != Exit.Right)
                return false;
            if (rightLocation - rightSize > x || rightLocation + rightSize < x)
                return false;
            return true;
        }

        #endregion

        #region Get gate locations

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

        #endregion

        public Pack CurrentPack
        {
            get 
            {
                if (packs.Count == 0)
                    return null;
                return packs[0];
            }
        }

        protected Point GetRandomLocation(int distFromWall)
        {
            return new Point(random.Next(Width - distFromWall - 2) + 1 + distFromWall / 2,
                random.Next(Height - distFromWall - 2) + 1 + distFromWall / 2);
        }

        #region Visualization of the current node

        private char[][] CreateBackground()
        {
            char[][] map = new char[Height][];
            map[0] = new char[Width];
            for (int j = 0; j < map[0].Length; j++)
                if (!WithinTopGate(j) || locked)
                    map[0][j] = '█';
                else
                    map[0][j] = '▒';
            for (int i = 1; i < map.Length - 1; i++)
            {
                map[i] = new char[Width];
                for (int j = 0; j < map[i].Length; j++)
                    if (j == 0)
                    {
                        if (!WithinLeftGate(i) || locked)
                            map[i][j] = '█';
                        else
                            map[i][j] = '▒';
                    }
                    else if (j == map[i].Length - 1)
                    {
                        if (!WithinRightGate(i) || locked)
                            map[i][j] = '█';
                        else
                            map[i][j] = '▒';
                    }
                    else
                        map[i][j] = '.';

                map[map.Length - 1] = new char[Width];
                for (int j = 0; j < map[0].Length; j++)
                    if (!WithinBotGate(j) || locked)
                        map[map.Length - 1][j] = '█';
                    else
                        map[map.Length - 1][j] = '▒';
            }

            return map;
        }

        private char[][] GetUpdatedMap(Point playerLocation, char playerGlyph)
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

        public string[] CreateEnemyOverview()
        {
            char[][] map = GetUpdatedMap(Player.Location, Player.Glyph);
            string[] drawMap = new string[map.Length];
            int i = 0;
            drawMap[0] = new string(map[0]);
            if (CurrentPack != null)
                for (i = 0; i < CurrentPack.Size; i++)
                {
                    Enemy e = CurrentPack[i];
                    drawMap[i * 2 + 1] = string.Format("{0} {1}", new string(map[i * 2 + 1]), e.name.Substring(0, Math.Min(20, e.name.Length)));
                    drawMap[i * 2 + 2] = string.Format("{0} {1} HP: {2}", new string(map[i * 2 + 2]), e.Glyph, e.CurrentHP);
                }
            for (i = i * 2 + 1; i < map.Length; i++)
                drawMap[i] = new string(map[i]);
            return drawMap;
        }

        #endregion

        public void PlaceEnemies()
        {
            if (CurrentPack == null)
                return;

            Order.HuntOrder.Target = this;
            
            Exit rn;

            if (AdjacencyList.Count > 0)
            {
                rn = (Exit)Math.Pow(2, random.Next(4));
                while (!AdjacencyList.ContainsKey(rn))
                    rn = (Exit)Math.Pow(2, random.Next(4));
                
                fleeNode = adjacencyList[rn];
            }

            else
            {
                rn = 0;
                fleeNode = this;
            }

            switch (rn)
            {
                case Exit.Top:
                    fleeLocation = new Point(TopExit, 1);
                    break;
                case Exit.Bot:
                    fleeLocation = new Point(BotExit, Height - 2);
                    break;
                case Exit.Left:
                    fleeLocation = new Point(1, LeftExit);
                    break;
                case Exit.Right:
                    fleeLocation = new Point(Width - 2, RightExit);
                    break;
                default:
                    fleeLocation = new Point(Width / 2, Height / 2);
                    break;
            }

            for (int i = 0; i < CurrentPack.Size; i++)
            {
                CurrentPack[i].Location = GetRandomLocation(4);
                for (int j = 0; j < i; j++)
                    if (CurrentPack[i].Location == CurrentPack[j].Location)
                    {
                        i--;
                        break;
                    }
            }
        }

        public virtual void MicroUpdates()
        {
            if (CurrentPack == null)
            {
                if (locked)
                    locked = false;
                return;
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

                if (CurrentPack == null)
                {
                    if (locked)
                        locked = false;
                    return;
                }
                PlaceEnemies();
            }

            // TODO: THis if staement shouldnt be here
            if (Player == null)
                return;

            if (CurrentPack.order != null || !CurrentPack.WillFlee() || locked)
            {
                foreach (Enemy e in CurrentPack)
                    if (Move(e, Player.Location))
                        e.KillTheHeretic(Player);
            }

            else
                MoveFlee();
        }

        private void MoveFlee()
        {
            int counter = 0;
            foreach (Enemy e in CurrentPack)
                if (Move(e, fleeLocation))
                    counter++;        

            if (counter == CurrentPack.Size)
                if (fleeNode.AddPack(CurrentPack))
                {
                    RemovePack(CurrentPack);
                    PlaceEnemies();
                }
        }

        public bool Move(Enemy e, Point target)
        {
            int x = target.X - e.Location.X;
            int y = target.Y - e.Location.Y;
        
            Point loc = Math.Abs(x) > Math.Abs(y) ?
                new Point(e.Location.X + Math.Sign(x), e.Location.Y) :
                new Point(e.Location.X, e.Location.Y + Math.Sign(y));

            if (CurrentPack != null)
                foreach (Enemy teammate in CurrentPack)
                {
                    if (teammate.Location == loc)
                    {
                        loc = Math.Abs(x) <= Math.Abs(y) ?
                            new Point(e.Location.X + Math.Sign(x), e.Location.Y) :
                            new Point(e.Location.X, e.Location.Y + Math.Sign(y));
                        
                        foreach (Enemy teamy in CurrentPack)
                            if (teamy.Location == loc)
                                return false;
                        
                        break;
                    }
                }

            if (loc == Player.Location && target != Player.Location)
                return false;

            if (loc == target)
                return true;

            e.Location = loc;
            return false;
        }
    }
}
