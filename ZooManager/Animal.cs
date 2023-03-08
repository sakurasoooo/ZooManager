using System;
using System.Collections.Generic;
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
            desinations = desinations.OrderBy(p =>
            {
                int score = 0;
                for (int d = 0; d < 4; d++)
                {
                    if (Seek(p, (Direction)d, 1, predators) >= 1) score -= 10;
                    if (Seek(p, (Direction)d, 1, preys) >= 1) score += 1;

                }
                return score;
            }).ToList();

            zone.MoveTo(desinations.Last()); // move the zone with the highest score

            return movedDistance;
        }
    }
}
