using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
namespace ZooManager
{
    // Add *abstract* to prohibit instantiation of Animal Class
    public abstract class Animal : GameObject
    {

        protected Animal(Zone zone) : base(zone)
        {

        }

        override public void Activate()
        {
            base.Activate();
            Console.WriteLine($"Animal {name} at {zone.position.x},{zone.position.y} activated");
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
        /// Move to the zone with the lowest density (least number of adjacent animals)
        /// startZone expects a starting location 
        /// </summary>
        /// <param name="targetZones"></param>
        /// <param name="distance"></param>
        /// <param name="blocks"></param>
        /// <returns></returns>
        virtual protected int Move(int distance = 1, int predators = 0 << 0, int preys = 0 << 0, bool fly = false)
        {
            int movedDistance = 0;
            Queue<Zone> startZones = new Queue<Zone>();
            HashSet<Zone> visitedZones = new HashSet<Zone>();


            startZones.Enqueue(this.zone);// start point
            visitedZones.Add(this.zone);
            // use BFS with a depth in distance
            for (int i = 0; i < distance; i++)
            {
                Queue<Zone> targetZones = new Queue<Zone>();
                Queue<Zone> cacheZones = new Queue<Zone>(startZones.ToList());
                while (startZones.Count > 0)
                {
                    Zone currentZone = startZones.Dequeue();
                    for (int d = 0; d < 4; d++)
                    {
                        // upgrade for reptor
                        // reptors will try to search further zone
                        int range = 0;
                        if (fly)
                        {
                            range = distance;
                        }
                        else
                        {
                            range = 1;
                        }

                        if (Seek(currentZone, (Direction)d, range, (int)LayerMask.Ground, fly) >= 1) // if found a empty ground
                        {
                            Zone nextZone = currentZone.FindZone((Direction)d);
                            if (!(visitedZones.Contains(nextZone)))
                            {
                                targetZones.Enqueue(nextZone);
                                visitedZones.Add(nextZone);
                            }
                        }
                    }
                }
                // go to next depth
                startZones = targetZones;

                if (startZones.Count > 0) movedDistance++;
                //If there is no way to go in expected distance, restore the last result.
                else
                {
                    startZones = cacheZones;
                    break;
                }

            }

            List<Zone> desinations = new List<Zone>(startZones);

           

            //Score the zone, if there are predators around, subtract 10 points, and if there are prey, add 1 point.
            desinations = desinations
                .Where(p => p.occupant == null)
                .OrderBy(p =>
            {
                int score = 0;
                for (int d = 0; d < 4; d++)
                {
                    if (Seek(p, (Direction)d, 1, predators) >= 1) score -= 10;
                    if (Seek(p, (Direction)d, 1, preys) >= 1) score += 1;

                }
                return score;
            }).ToList();

            if (desinations.Count == 0) return 0; // no valid results

            zone.MoveTo(desinations.Last()); // move the zone with the highest score

            return movedDistance;
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
