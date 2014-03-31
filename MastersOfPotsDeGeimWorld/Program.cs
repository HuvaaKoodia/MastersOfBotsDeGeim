using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MastersOfPotsDeGeimWorld
{
    class Program
    {
        static Map map;
        static void Main(string[] args)
        {
            int seed = 1111;

            map = new Map(30,30);
            map.GenerateMap(seed,10,20,25);


            var test_e = new TestEntity(map, 1);
            test_e.SetPosition(4, 3);

            var Entities=new List<Entity>();

            Entities.Add(test_e);

            DrawMap();
            
            int turn = 1;
            while (true) {
                Console.WriteLine("Turn: " + turn);
                ++turn;

                for (int i = 0; i < Entities.Count;++i)
                {
                    var e= Entities[i];
                    e.Update();
                    e.LateUpdate();

                    if (e.Dead) {
                        Entities.Remove(e);
                        --i;
                    }
                }

                DrawMap();

                var input = Console.ReadLine();
                var d=Entity.Direction.Right;
                input=input.ToLower();
                if (input.StartsWith("exit")) break;
            }

            Console.WriteLine();
        }

        private static void DrawMap()
        {
            for (int j = 0; j < map.H; ++j)
            {
                for (int i = 0; i < map.W; ++i)
                {
                    var t = map.GetTile(i, j);
                    Console.Write(t.GetCharacterCode() + "");
                }
                Console.WriteLine();
            }
        }
    }
}
