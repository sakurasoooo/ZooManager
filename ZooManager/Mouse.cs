using System;
namespace ZooManager
{
    public class Mouse : Animal, IPrey
    {
        public int Predators { get; set; }
        public Mouse(Zone zone, string name = "Squeaky") : base(zone)
        {
            emoji = "🐭";
            species = LayerMask.Mouse;
            Predators = (int)LayerMask.Cat;
            this.name = name; // "this" to clarify instance vs. method parameter
            reactionTime = new Random().Next(1, 4); // reaction time of 1 (fast) to 3
            /* Note that Mouse reactionTime range is smaller than Cat reactionTime,
             * so mice are more likely to react to their surroundings faster than cats!
             */

        }

        public override void Activate()
        {
            base.Activate();
            Console.WriteLine("I am a mouse. Squeak.");
            Flee();

        }

        /// <summary>
        /// The mouse's moving method does not move one grid at a time. Will move up to two tiles and look for the ideal position.
        /// </summary>
        /// <returns></returns>
        public bool Flee()
        {
            for (int d = 0; d < 4; d++)
            {
                if (Seek(zone, (Direction)d, 1, (int)Predators) > 0)
                {
                    if (Move(2, Predators, 0) > 0) return true;
                }
            }
            return false;
        }


    }
}

