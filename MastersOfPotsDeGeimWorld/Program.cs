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

            Team testTeam = new Team("Testi", 1, ConsoleColor.Yellow);
            Team anotherTeam = new Team("Lol", 2, ConsoleColor.Magenta);

            var test_e = new TestEntity(map, testTeam);
            test_e.SetPosition(4, 3);

            var test_2 = new TestEntity(map, anotherTeam);
            test_2.SetPosition(10, 10);
            
            map.GameLoop();

            Console.WriteLine("Program over ( anykey to exit )");
            Console.ReadLine();
        }
    }
}
