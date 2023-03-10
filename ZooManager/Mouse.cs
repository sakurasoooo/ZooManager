using System;
namespace ZooManager
{
    public class Mouse : Animal, IPrey
    {
        public int Predators { get; set; } // Layermask, used to record the layer where the enemies is located

        private bool Reproducible = true; // Production status of mice. If it is false, then this mouse cannot produce baby mice again.

        /// <summary>
        /// The mouse's constructor, the constructor defaults to the mouse's enemy as Cat and Raptor
        /// The predator is also added alien as enemy
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="name"></param>
        public Mouse(Zone zone, string name = "Squeaky") : base(zone)
        {
            emoji = "🐭";
            species = LayerMask.Mouse;
            Predators = (int)(LayerMask.Cat | LayerMask.Raptor | LayerMask.Alien);
            this.name = name; // "this" to clarify instance vs. method parameter
            reactionTime = new Random().Next(1, 4); // reaction time of 1 (fast) to 3
            /* Note that Mouse reactionTime range is smaller than Cat reactionTime,
             * so mice are more likely to react to their surroundings faster than cats!
             */

        }

        /// <summary>
        /// It will try to run away from the predator.
        /// Then try to reproduce new mouse
        /// </summary>
        public override void Activate()
        {
            if (!activate) return;

            base.Activate();
            Console.WriteLine("I am a mouse. Squeak.");
            Flee();
            Reproduce();

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

        /// <summary>
        /// If the mouse lasts for 3 rounds and reproducible is true,
        /// it will try to find a vacancy in the adjacent four grids,
        /// then generate a new mouse in its own position, and it goes to the new position
        /// </summary>
        protected void Reproduce()
        {
            if (turnsActivated > 3 && Reproducible)
            {
                for (int d = 0; d < 4; d++)
                {
                    if (Seek(zone, (Direction)d, 1, (int)LayerMask.Ground) > 0)
                    {
                        Zone home = zone.FindZone((Direction)d);
                        var mice = new Mouse(home); //  Reproduce new mouse
                        zone.MoveTo(home); // swap self position with the child
                        Reproducible = false;
                        break;
                    }
                }
            }
        }


    }
}

