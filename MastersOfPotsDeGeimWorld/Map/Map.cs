using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.IO;

namespace MastersOfPotsDeGeimWorld
{
    /// <summary>
    /// Has both the map and all game entities for convenience.
    /// </summary>
    public class Map
    {
        public Tile[,] map_tiles { get; private set; }
        public List<Entity> GameEntities { get; private set; }
        public List<Team> Teams { get; private set; }

        int diamond_amount = 0;

        public int W { get { return map_tiles.GetLength(0); } }
        public int H { get { return map_tiles.GetLength(1); } }
        
        public Map(int w,int h){
            map_tiles= new Tile[w,h];
            GameEntities = new List<Entity>();
            Teams=new List<Team>();
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

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            waiting_for_timer = false;
        }

        Timer timer = new Timer();
        bool waiting_for_timer = false;
        public int Turn { get; private set; }

        public void GameLoop() {

            Console.WriteLine("Game loop start:");
            Turn = 0;
            bool gameOn = true, allowInput=true;
            int autoRun = 0;
            DrawMap();

            timer.Interval = 250;
            timer.Elapsed += TimerElapsed;

            var s_out = Console.Out;
            StringWriter s_wrt = new StringWriter();

            Team winner=null;

            while (gameOn)
            {
                for (int e = GameEntities.Count-1; e >= 0; --e)
                {
                    if (waiting_for_timer) {
                        ++e;
                      continue;
                    }
                    timer.Stop();

                    ++Turn;
                    var entity = GameEntities[e];

                    //auto run logic
                    if (autoRun != 0) {
                        allowInput = false;

                        if (autoRun > 0) --autoRun;
                        if (autoRun == 0) {
                            allowInput = true;
                        }
                    }

                    //input
                    if (allowInput)
                    {
                        Console.WriteLine("Input:\n- \"exit\" to end program\n- \"auto n\" to autorun simulation.\n(n = amount of turns, no parameter runs the rest of the simulation)\n anykey to continue");
                        
                        while (true)
                        {
                            var input = Console.ReadLine();

                            if (input == ("exit"))
                            {
                                gameOn = false;
                                break;
                            }
                            else if (input.StartsWith("auto"))
                            {
                                try
                                {
                                    if (input.Length > 4)
                                    {
                                        int steps = int.Parse(input.Substring(5));
                                        autoRun = steps;
                                    }
                                    else
                                    {
                                        autoRun = -1;
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine("Faulty syntax. Try: \"auto 100\"");
                                    continue;
                                }                            
                            }
                            else
                            {
    #if DEBUG
                                entity.GetInput(input);
    #endif
                            }
                            break;
                        }
                    }
                    timer.Start();
                    waiting_for_timer=true;
                    Console.Clear();
                    Console.SetOut(s_wrt);

                    //updates
                    entity.MyTeam.Update(entity);
                    entity.Update();
                    entity.LateUpdate();

                    Console.SetOut(s_out);
                    //drawing
                    
                    Console.WriteLine("Turn " + Turn);
                    Console.WriteLine(entity.MyTeam);
                    Console.WriteLine(entity);

                    DrawMap();

                    if (entity.Dead) GameEntities.Remove(entity);

                    Console.Write(s_wrt.ToString());
                    s_wrt.GetStringBuilder().Clear();



                    //game over (lazy polling checks)
                    if (GameEntities.Count == 0){
                        gameOn=false;
                        winner = null;
                        break;
                    }
                    //no enemies
                    bool no_teams = true;
                    Team team = entity.MyTeam;
                    winner = team;
                    foreach (var ent in GameEntities) {
                        
                        if (ent.MyTeam != team) {
                            no_teams = false;
                            break;
                        } 
                    }

                    if (no_teams)
                    {
                        gameOn = false;
                        break;
                    }

                    //no diamonds
                    bool no_diamonds=true;
                    foreach (var t in map_tiles) {
                        if (t.IsType(Tile.Type.diamond)){
                            no_diamonds=false;
                            break;
                        }
                    }

                    if (no_diamonds)
                    {
                        int max = 0;
                        foreach (var t in Teams)
                        {
                            if (t.DiamondCount > max) {
                                winner = t;
                                max = t.DiamondCount;
                            }
                        }
                        gameOn = false;
                        break;
                    }
                }
                
            }


            //gameover report
            Console.WriteLine("Gameover!");

            if (winner == null)
            {
                Console.WriteLine("It's a tie!!!");
            }
            else {
                Console.WriteLine(winner+" wins!");
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
