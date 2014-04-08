using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MastersOfPotsDeGeimWorld
{
    public class Tile
    {
        public enum Type { empty, food, diamond, wall };


        public Type TileType;
        public int Amount,X,Y;
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
