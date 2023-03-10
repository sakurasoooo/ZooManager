using System;
namespace ZooManager
{
    public class Raptor : Bird, IPredator
    {
        public int Preys { get; set; }  // Layermask, used to record the layer where the food is located
        public int Hungry { get; set; } // A value of 0 means the animal is very hungry.

        /// <summary>
        /// The raptor's constructor, the constructor defaults to the food is mouse and Cat
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="name"></param>
        public Raptor(Zone zone, string name = "Screechy") : base(zone)
        {
            emoji = "🦅";
            this.species = LayerMask.Raptor;
            this.Preys = (int)(LayerMask.Cat | LayerMask.Mouse);
            this.name = name;
            this.reactionTime = new Random().Next(1, 1); // reaction time 1(very fast)

            this.Hungry = 4;
        }

        /// <summary>
        /// Used to be called by Game.
        /// When the hunger value is 0, the Raptor will die.
        /// If a Raptor attempts to attack its prey and fails, it will try to patrol
        /// </summary>
        public override void Activate()
        {
            base.Activate();
            Hungry--;
            if (Death())
            {
                return;
            }
            Console.WriteLine("I am a Raptor. Screech.");
            if (!Hunt()) Flee();
            else Hungry = 4;

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
        /// The raptor's own movement method will move to a maximum of distance 2, and ignore any obstacles blocking the way
        /// </summary>
        /// <returns></returns>
        public bool Flee()
        {

            if (Move(2, 0, Preys, true) > 0) return true;

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
                var skull = new Skull(zone);
                return true;
            }
            return false;
        }
    }
}

