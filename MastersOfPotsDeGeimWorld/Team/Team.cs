using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MastersOfPotsDeGeimWorld
{
    public class Team
    {
        public List<Entity> TeamMembers { get; private set; }

        public string Name { get; private set; }
        public int Number { get; private set; }
        public ConsoleColor Color { get; private set; }

        public int DiamondCount { get; private set; }


        public Team(string name, int number, ConsoleColor color)
        {
            Name = name;
            Number = number;
            Color = color;
            TeamMembers = new List<Entity>();

            DiamondCount = 0;
        }

        public Team(string name, int number, ConsoleColor color, List<Entity> teamEntities)
        {
            Name = name;
            Number = number;
            Color = color;
            TeamMembers = teamEntities;
            
            DiamondCount = 0;
        }

        public void AddDiamond()
        {
            DiamondCount++;
        }

        public void TeamMemberDied(Entity teamMember)
        {
            TeamMembers.Remove(teamMember);
        }

        public void AddTeamMember(Entity teamMember)
        {
            TeamMembers.Add(teamMember);
        }

        public virtual void Update(Entity currentEntity)
        {

        }
    }
}
