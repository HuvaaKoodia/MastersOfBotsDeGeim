using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MastersOfPotsDeGeimWorld
{
    /// <summary>
    /// Has both the map and all game entities for convenience.
    /// </summary>
    public class Map
    {
        public Tile[,] map_tiles { get; private set; }
        public List<Entity> GameEntities { get; private set; }

        int diamond_amount = 0;

        public int W { get { return map_tiles.GetLength(0); } }
        public int H { get { return map_tiles.GetLength(1); } }
        
        public Map(int w,int h){
            map_tiles= new Tile[w,h];
            GameEntities = new List<Entity>();
        }

        public void GenerateMap(int seed,int diamonds, int foods,int walls) {
            diamond_amount = diamonds;

            var rand = new Random(seed);

            for (int i = 0; i < W; ++i)
            {
                for (int j = 0; j < H; ++j)
                {
                    map_tiles[i, j] = new Tile(i,j);
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

        //logic

        public int Turn { get; private set; }

        public void GameLoop() {
            Turn = 0;
            
            bool gameOn = true;

            while (gameOn)
            {
                
                
                for (int e = GameEntities.Count-1; e >= 0; --e)
                {
                    var entity = GameEntities[e];

                    Console.WriteLine("Input:\n- e to exit\n- anykey to continue");
                    var input = Console.ReadLine();

                    if (input.StartsWith("e"))
                    {
                        gameOn = false;
                        break;
                    }
                    Console.WriteLine("Turn " + Turn);

                    //updates
                    entity.MyTeam.Update(entity);
                    entity.Update();
                    entity.LateUpdate();

                    if (entity.Dead) GameEntities.Remove(entity);

                    DrawMap();
                }

                if (GameEntities.Count == 0) break;
            }
        }

        public void DrawMap()
        {
            for (int j = 0; j < H; ++j)
            {
                for (int i = 0; i < W; ++i)
                {
                    var t = GetTile(i, j);

                    Console.ForegroundColor = t.GetCharacterColor();
                    Console.Write(t.GetCharacterCode() + "");
                    Console.Write(" ");
                }
                Console.WriteLine();
            }

            Console.ResetColor();
        }
    }
}
