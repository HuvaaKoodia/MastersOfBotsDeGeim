using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MastersOfPotsDeGeimWorld
{
    public class Entity
    {
        public enum Direction{Right=0,Up,Left,Down};
        public int TeamNumber { get; private set;}
        public bool Dead { get;private set;}

        protected Map MapReference;
        protected int energy=10;

        private int _x, _y;
        private Tile _currentTile;

        public Entity(Map mapref,int team) {
            TeamNumber = team;
            MapReference = mapref;
        }

        protected bool IsDirectionFree(Direction d)
        {
            int x = 0, y = 0;
            if (d == Direction.Right) x = 1;
            else if (d == Direction.Left) x = -1;
            else if (d == Direction.Up) y = 1;
            else if (d == Direction.Down) y = -1;
            return MapReference.GetTile(_x + x, _y + y).IsEmpty();
        }

        protected void MoveTo(Direction d) {
            int x=0,y=0;
            if (d == Direction.Right) x = 1;
            else if (d == Direction.Left) x = -1;
            else if (d == Direction.Up) y = 1;
            else if (d == Direction.Down) y = -1;
            SetPosition(_x + x, _y + y);
        }

        public void SetPosition(int x, int y) {
            if (_currentTile != null) {
                _currentTile.EntityReference = null;
            }
            _x = x; _y = y;
            _currentTile = MapReference.GetTile(x, y);
            _currentTile.EntityReference = this;
        }

        public virtual void Update(){}

        public void LateUpdate() {
            energy -= 1;
            if (energy <= 0)
            {
                Console.WriteLine("too bad is DEAD (starvation)!");
                Dead = true;
                _currentTile.EntityReference = null;
            }
        }
    }
}
