using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MastersOfPotsDeGeimWorld
{
    public class TestEntity : Entity
    {
        Random r = new Random();
        public TestEntity(Map mapref, Team team) : base(mapref, team) {}

        public override void Update()
        {
            var rn=r.Next(100);
            var d=Direction.Left;
            if (rn < 25) {
                d=Direction.Left;
            }
            else if (rn < 50)
            {
                d = Direction.Right;
            }
            else if (rn < 75)
            {
                d = Direction.Up;
            }
            else {
                d = Direction.Down;
            }

            if (IsDirectionFree(d)) MoveTo(d);
        }
    }
}
