﻿using System;
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

            var Entities=new List<Entity>();

            Entities.Add(test_e);

            map.DrawMap();
            
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
                
                map.DrawMap();

                var input = Console.ReadLine();
                var d=Entity.Direction.Right;
                input=input.ToLower();
                if (input.StartsWith("exit")) break;
            }

            Console.WriteLine();
        }

        
    }
}
