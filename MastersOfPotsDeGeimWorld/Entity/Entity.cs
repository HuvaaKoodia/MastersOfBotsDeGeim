using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MastersOfPotsDeGeimWorld
{
    public class Entity
    {
        public enum Direction{Right=0,Up,Left,Down};
        public Team MyTeam { get; private set; }
        public int TeamNumber { get; private set;}
        public bool Dead { get;private set;}

        protected Map MapReference;
        protected int energy=10;

        private int _x, _y;
        private Tile _currentTile;

        public int X { get { return _x; } }
        public int Y { get { return _y; } }

        public Entity(Map mapref, Team team)
        {
            MyTeam = team;
            MyTeam.AddTeamMember(this);
            TeamNumber = team.Number;
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

        protected void MoveTo(Direction d)
        {
            int x=0,y=0;
            if (d == Direction.Right) x = 1;
            else if (d == Direction.Left) x = -1;
            else if (d == Direction.Up) y = 1;
            else if (d == Direction.Down) y = -1;
            SetPosition(_x + x, _y + y);
        }

        protected bool IsDirectionFree(int x, int y)
        {
            return MapReference.GetTile(_x + x, _y + y).IsEmpty();
        }

        /// <summary>
        /// Move a relative amount.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected void Move(int x,int  y)
        {
            SetPosition(_x + x, _y + y);
        }

        public void SetPosition(int x, int y) {
            if (_currentTile != null)
            {
                _currentTile.EntityReference = null;
            }
            _x = x; _y = y;
            _currentTile = MapReference.GetTile(x, y);
            _currentTile.EntityReference = this;
        }

        public virtual void Update(){}

        public void LateUpdate()
        {
            energy -= 1;
            if (energy <= 0)
            {
                Console.WriteLine("too bad is DEAD (starvation)!");
                Die();
            }
        }

        void Die()
        {
            Dead = true;
            _currentTile.EntityReference = null;

            MapReference.GameEntities.Remove(this);
            MyTeam.TeamMemberDied(this);
        }
    }
}
