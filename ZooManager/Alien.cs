using System;
namespace ZooManager
{


    public class Alien : GameObject, IPredator
    {
        public int Preys { get; set; }
        public int Hungry { get; set; }

        static int toxicity = 1;

        public Alien(Zone zone, string name = "Covid") : base(zone)
        {
            this.emoji = "🦠";
            this.species = LayerMask.Alien;
            this.Preys = ~(int)(LayerMask.Alien | LayerMask.Ground); // enable all layers except Ground layer
            this.Hungry = 10;
            this.name = name;
            this.reactionTime = new Random().Next(toxicity, Math.Min(toxicity, 11)); // reaction time 10
        }

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
        /// Call this method to replace the animal with a skeleton
        /// </summary>
        protected bool Death()
        {
            if (Hungry <= 0)
            {
                return true;
            }
            return false;
        }
    }
}

