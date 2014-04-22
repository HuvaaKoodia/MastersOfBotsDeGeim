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
        public int TeamNumber { get { return MyTeam.Number; } }
        public bool Dead { get;private set;}

        protected Map MapReference;
        protected int energy=30;

        private int _x, _y;
        private Tile _currentTile;

        public Tile CurrentTile { get { return _currentTile; } }
        public int X { get { return _x; } }
        public int Y { get { return _y; } }

        bool acted;

        public Entity(Map mapref, Team team)
        {
            MyTeam = team;
            MyTeam.AddTeamMember(this);
            MapReference = mapref;

            mapref.GameEntities.Add(this);

            acted = false;
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

            acted = true;
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
            acted = true;
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
            if (acted)
            {
                energy -= 1;
                acted = false;
            }

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

        //action functions
        public bool EatFrom(Tile tile) 
        {
            if (tile.IsType(Tile.Type.food)&&tile.Amount>0)
            {
                energy += 20;//DEV. to a constant
                --tile.Amount;
                acted = true;
                return true;
            }
            return false;
        }

        public bool DigFrom(Tile tile)
        {
            if (tile.IsType(Tile.Type.diamond) && tile.Amount > 0)
            {
                MyTeam.AddDiamond();
                --tile.Amount;
                acted = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Starts checking from upper left tile of calling entity, checks every "row" of tiles, 
        /// and places clone to first free tile
        /// Returns true if a tile was found, otherwise false
        /// </summary>
        /// <param name="clone"></param>
        /// <returns></returns>
        public bool PlaceCloneToFirstOpenNeigbourTile(Entity clone)
        {
            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    if (MapReference.GetTile(_x + x, _y + y).CanMoveTo())
                    {
                        PlaceCloneToTile(clone, MapReference.GetTile(_x + x, _y + y));
                        return true;
                    }
                }
            }

            return false;
        }

        public void PlaceCloneToTile(Entity clone, Tile tile)
        {
            clone.SetPosition(tile.X, tile.Y);
        }
    }
}
