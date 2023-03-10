using System;
namespace ZooManager
{


    public class Alien : GameObject, IPredator
    {
        public int Preys { get; set; }  // Layermask, used to record the layer where the food is located
        public int Hungry { get; set; } // A value of 0 means the animal is very hungry.

        static int toxicity = 1; // Shared by alien populations. Affects the reaction time of new aliens

        /// <summary>
        /// The Alien's constructor, the constructor defaults set the Alien'food as any objects except ground and other alien
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="name"></param>
        public Alien(Zone zone, string name = "Covid") : base(zone)
        {
            this.emoji = "🦠";
            this.species = LayerMask.Alien;
            this.Preys = ~(int)(LayerMask.Alien | LayerMask.Ground); // enable all layers except Ground layer
            this.Hungry = 10;
            this.name = name;
            this.reactionTime = new Random().Next(toxicity, Math.Min(toxicity, 11)); // reaction time 10
        }

        /// <summary>
        /// Used to be called by Game.
        /// When the hunger value is 0, the Alien will die.
        /// It attempts to attack its prey nearby
        /// </summary>
        public override void Activate()
        {
            if (!activate) return;

            base.Activate();
            Console.WriteLine("I am an alien. Beep, beep.");
            Hungry--;
            if (Death())
            {
                return;
            }
            if (Hunt())
            {
                Hungry = 10;
                //Alien's aggressiveness gradually weakens
                toxicity = Math.Clamp(toxicity + 1, 1, 10);
            }
        }

        /// <summary>
        /// Used to be called by Activate.
        /// If there are prey in the four adjacent zones, it will try to attack the most favorable position.
        /// </summary>
        /// <returns></returns>
        public bool Hunt()
        {
            for (int d = 0; d < 4; d++)
            {
                if (Seek(zone, (Direction)d, 1, (int)Preys) > 0)
                {
                    if (Attack(1, 0, Preys) > 0) return true;
                }
            }
            return false;

        }
        /// <summary>
        /// Used to be called by Activate.
        /// Call this method to replace the animal with a skeleton
        /// </summary>
        public bool Death()
        {
            if (Hungry <= 0)
            {
                return true;
            }
            return false;
        }


    }
}

