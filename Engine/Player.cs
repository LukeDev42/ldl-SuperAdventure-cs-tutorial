using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Player : LivingCreature
    {
        public int Gold { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level { get; set; }
        public Location CurrentLocation { get; set; }
        public List<InventoryItem> Inventory { get; set; }
        public List<PlayerQuest> Quests { get; set; }

        public Player(int currentHitPoints, int maximumHitPoints, int godl, int experiencePoints, int level) :
            base(currentHitPoints, maximumHitPoints)
        {
            Gold = Gold;
            ExperiencePoints = experiencePoints;
            Level = level;
            Inventory = new List<InventoryItem>();
            Quests = new List<PlayerQuest>();
        }

        public bool HasRequiredItemToEnterThisLocation(Location location)
        {
            if (location.ItemRequiredToEnter == null)
            {
                //There is no required item for this location
                return true;
            }

            //look for required items
            foreach (InventoryItem ii in Inventory)
            {
                if (ii.Details.ID == location.ItemRequiredToEnter.ID)
                {
                    //We found the item
                    return true;
                }
            }

            //Didn't find the item in their inventory
            return false;
        }
    }


}