using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MastersOfPotsDeGeimWorld
{
    public class Map
    {
        Tile[,] map_tiles;

        int diamond_amount = 0;

        public int W { get { return map_tiles.GetLength(0); } }
        public int H { get { return map_tiles.GetLength(1); } }
        
        public Map(int w,int h) {
            map_tiles= new Tile[w,h];
        }

        public void GenerateMap(int seed,int diamonds, int foods,int walls) {
            diamond_amount = diamonds;

            var rand = new Random(seed);

            for (int i = 0; i < W; ++i)
            {
                for (int j = 0; j < H; ++j)
                {
                    map_tiles[i, j] = new Tile();
                }
            }

            int fail_safe = 10000;
            int rx,ry;
            while(diamonds>0){
                --fail_safe;
                if (fail_safe == 0) break;

                rx=rand.Next(W);
                ry=rand.Next(H);

                var t = GetTile(rx, ry);

                if (t.IsEmpty())
                {
                    t.TileType = Tile.Type.diamond;
                    t.Amount = 1+rand.Next(14);
                }
                else continue;

                --diamonds;
            }
            fail_safe = 10000;

            while (foods > 0)
            {
                --fail_safe;
                if (fail_safe == 0) break;

                rx = rand.Next(W);
                ry = rand.Next(H);

                var t = GetTile(rx, ry);

                if (t.IsEmpty())
                {
                    t.TileType = Tile.Type.food;
                    t.Amount = 5 + rand.Next(25);
                }
                else continue;

                --foods;
            }
            fail_safe = 10000;

            while (walls > 0)
            {
                --fail_safe;
                if (fail_safe == 0) break;

                rx = rand.Next(W);
                ry = rand.Next(H);

                var t = GetTile(rx, ry);

                if (ry == 0 || ry == H - 1 || ry % 3 == 0) continue;

                if (t.IsEmpty())
                {
                    t.TileType = Tile.Type.wall;
                }
                else continue;

                --walls;  
            }
        }

        public void SetTile(int x, int y, Tile.Type t)
        {
            var tile = map_tiles[x, y];
            tile.TileType = t;
        }

        public void SetTile(int x, int y, int a)
        {
            var tile = map_tiles[x, y];
            tile.Amount = a;
        }

        public void SetTile(int x, int y, Entity e)
        {
            var tile = map_tiles[x, y];
            tile.EntityReference=e;
        }

        private Tile wall_tile = new Tile(Tile.Type.wall);

        public Tile GetTile(int x, int y)
        {
            if (x < 0 || y < 0 || x > W - 1 || y > H - 1) return wall_tile;
            return map_tiles[x, y];
        }
    }
}
