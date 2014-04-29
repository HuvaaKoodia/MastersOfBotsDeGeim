using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MastersOfPotsDeGeimWorld
{
    public class Tile
    {
        public enum Type { empty, food, diamond, wall };

        int _amount=0;

        public Type TileType;
        public int X,Y;

        public int Amount { 
            get { return _amount; } 
            set { 
                _amount = value;
                if (_amount == 0) {
                    TileType = Type.empty;
                }
            }
        }
        public int AmountCharacterCode { get {return Math.Min(9,Math.Max(0,Amount));} }
        public Entity EntityReference;

        public Tile(int x, int y) { X = x; Y = y; }

        public Tile(Type type) { TileType = type; }

        public bool CanMoveTo() {
            return TileType == Type.empty&&EntityReference==null;
        }

        public string GetCharacterCode()
        {
            if (EntityReference == null)
            {
                if (TileType == Type.empty) return "..";
                else if (TileType == Type.food) return "F" + AmountCharacterCode;
                else if (TileType == Type.wall) return "WW";
                else if (TileType == Type.diamond) return "D" + AmountCharacterCode;
            }
            return "E"+EntityReference.TeamNumber;
        }

        public ConsoleColor GetCharacterColor()
        {
            if (EntityReference == null)
            {
                if (TileType == Type.empty) return ConsoleColor.White;
                else if (TileType == Type.food) return ConsoleColor.Green;
                else if (TileType == Type.wall) return ConsoleColor.Black;
                else if (TileType == Type.diamond) return ConsoleColor.Cyan;
            }
            return EntityReference.MyTeam.Color;
        }

        public bool IsType(Type type)
        {
            return TileType == type;
        }

        public bool IsEmpty()
        {
            return IsType(Type.empty);
        }
    }
}
