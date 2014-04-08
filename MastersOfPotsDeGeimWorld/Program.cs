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
            Console.SetWindowSize(100, 60);

            int seed = 1111;

            map = new Map(30,30);
            map.GenerateMap(seed,10,20,25);


            var test_e = new TestEntity(map, 1);
            test_e.SetPosition(4, 3);

            map.GameEntities.Add(test_e);
            map.DrawMap();
            
            map.GameLoop();

            Console.WriteLine("Program over ( e to exit )");
            while (true)
            {
                var input = Console.ReadLine();

                if (input.StartsWith("e")) break;
            }
        }

        
    }
}
