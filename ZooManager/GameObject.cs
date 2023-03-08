using System;

namespace ZooManager
{
    using System.Collections.Generic;
    using System.Linq;
    abstract public class GameObject
    {
        public LayerMask species { get; protected set; }
        public string name { get; protected set; } = ""; // only allow subclass to modify
        public int reactionTime { get; protected set; }  // default reaction time for animals (1 - 10)
        public int turnsActivated { get; protected set; }
        public string emoji { get; protected set; } = ""; // only allow subclass to modify
        public Zone zone { get; protected set; } // only allow subclass to modify
        public bool activate { get; protected set; } = true; // true if the game obejct has not been activated this turn

        protected GameObject(Zone zone)
        {
            this.zone = zone;
            this.zone.occupant = this;
        }
        virtual public void ReportLocation()
        {
            Console.WriteLine($"I am at {zone.position.x},{zone.position.y}");
        }

        virtual public void Activate()
        {
            activate = false;
            turnsActivated++;
        }

        virtual public void Deactivate()
        {
            activate = true;
        }

        /// <summary>
        /// Finds the specified gameobejct at a certain distance according to the specified direction.
        /// Multiple targets can be specified using a layer mask.
        /// when ignoreObstacle is True,Search will ignore occlusions from animals and obstacles.
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="d"></param>
        /// <param name="distance"></param>
        /// <param name="target"></param>
        /// <param name="ignoreObstacle"></param>
        /// <returns></returns>
        virtual protected int Seek(Zone zone, Direction d, int distance = 1, int target = 0 << 0, bool ignoreObstacle = false)
        {
            if (distance <= 0) return 0; // if distance less than 0, terminate the search

            Zone? nextZone = zone.FindZone(d);

            if (nextZone is null) return 0; // if reach the boundary, terminate the search
            else if (nextZone.occupant == null) // Search for open spaces free of other gameobjects
            {
                if ((target & (int)LayerMask.Ground) != 0)
                {
                    return 1;
                }
                //continue seeking
                int returnValue = Seek(nextZone, d, distance - 1, target);
                if (returnValue > 0) return 1 + returnValue;
            }
            else
            {   // Search for specified Animals
                if (((int)nextZone.occupant.species & target) != 0)
                {
                    return 1;
                }
                if (ignoreObstacle)
                {
                    //continue seeking
                    int returnValue = Seek(nextZone, d, distance - 1, target);
                    if (returnValue > 0) return 1 + returnValue;
                }
            }

            return 0;
        }


        /// <summary>
        /// Here, BFS is not used like the Move method, and only possible prey in the four adjacent zones are traversed.
        /// Choose the one with the highest probability of surviving after the attack.
        /// now only support attack range with 1
        /// </summary>
        /// <param name="distance"> attack range</param>
        /// <param name="predators"></param>
        /// <param name="preys"></param>
        virtual protected int Attack(int distance = 1, int predators = 0 << 0, int preys = 0 << 0)
        {

            List<Zone> preyZones = new List<Zone>();
            //search all preys in the range
            for (int i = 0; i < distance; i++)
            {
                for (int d = 0; d < 4; d++)
                {
                    if (Seek(zone, (Direction)d, 1, preys) >= 1)
                    {
                        preyZones.Add(zone.FindZone((Direction)d));
                    }
                }
            }


            if (preyZones.Count <= 0) return 0; // no prey found, return

            // Scores were calculated for each endpoint, with higher scores better for the animal.
            preyZones = preyZones.OrderBy(p =>
            {
                int score = 0;
                for (int d = 0; d < 4; d++)
                {
                    if (Seek(p, (Direction)d, 1, predators) >= 1) score -= 10;
                    if (Seek(p, (Direction)d, 1, preys) >= 1) score += 1;
                }
                return score;
            }).ToList();

            Zone targetZone = preyZones.Last(); // choose the best choice

            Console.WriteLine($"{this.name} is attacking {targetZone.occupant.name}");
            targetZone.occupant = null; // Kill the prey

            zone.MoveTo(targetZone); // move to the prey's position
            return 1;
        }


    }
}

