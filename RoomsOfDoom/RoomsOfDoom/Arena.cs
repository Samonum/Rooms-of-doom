using System;
using System.Collections.Generic;
using System.Linq;
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
        Random r;
        char[][] map;
        ITile tiles;
        int topExit, leftExit, rightExit, botExit;
        int width = 37, height = 25;
        public Arena(Exit openExits, Pack enemies, Player player, Exit entrance)
        {
            r = new Random();
            topExit = 10 + r.Next(width - 20);
            leftExit = 10 + r.Next(height - 20);
            rightExit = 10 + r.Next(height - 20);
            botExit = 10 + r.Next(width - 20);
        }

        public void UpdateMap()
        {
            map = new char[height][];
            map[0] = new char[width];
            for (int j = 0; j < map[0].Length; j++)
                if (j < topExit - 2 || j > topExit + 2)
                    map[0][j] = '█';
                else
                    map[0][j] = '▒';
            for (int i = 1; i < map.Length - 1; i++)
            {
                map[i] = new char[width];
                for (int j = 0; j < map[i].Length; j++)
                    if (j == 0)
                    {
                        if (i < leftExit - 2 || i > leftExit + 2)
                            map[i][j] = '█';
                        else
                            map[i][j] = '▒';
                    }
                    else if (j == map[i].Length - 1)
                    {
                        if (i < rightExit - 2 || i > rightExit + 2)
                            map[i][j] = '█';
                        else
                            map[i][j] = '▒';
                    }
                    else
                        map[i][j] = '.';
                map[map.Length - 1] = new char[width];
                for (int j = 0; j < map[0].Length; j++)
                    if (j < botExit - 2 || j > botExit + 2)
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
