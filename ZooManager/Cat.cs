using System;

namespace ZooManager
{
    public class Cat : Animal, IPredator, IPrey
    {
        public int Preys { get; set; } // Layermask, used to record the layer where the food is located
        public int Predators { get; set; } // Layermask, used to record the layer where the enemies is located
        public int Hungry { get; set; } // A value of 0 means the animal is very hungry.

        /// <summary>
        /// The cat's constructor, the constructor defaults to the cat's natural enemy as Raptor, and the food is mouse and chick
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="name"></param>
        public Cat(Zone zone, string name = "Fluffy") : base(zone)
        {
            this.emoji = "🐱";
            this.species = LayerMask.Cat;
            this.Preys = (int)(LayerMask.Mouse|LayerMask.Chick);
            this.Predators = (int)LayerMask.Raptor;
            this.name = name;
            this.reactionTime = new Random().Next(1, 6); // reaction time 1 (fast) to 5 (medium)

            this.Hungry = 4;

        }

        /// <summary>
        /// Used to be called by Game.
        ///When the hunger value is 0, the cat will die.
        /// If a cat attempts to attack its prey and fails, it will try to run away from the predator.
        /// </summary>
        public override void Activate()
        {
            base.Activate();
            Hungry--;
            if (Death())
            {
                return;
            }
            Console.WriteLine("I am a cat. Meow.");
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
                    if (Attack(1, Predators, Preys) > 0) return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Used to be called by Activate.
        /// If there are natural enemies in the four adjacent zones, it will try to move to the most favorable position within two distances.
        /// </summary>
        /// <returns></returns>
        public bool Flee()
        {
            for (int d = 0; d < 4; d++)
            {
                if (Seek(zone, (Direction)d, 1, (int)Predators) > 0)
                {
                    if (Move(2, Predators, Preys) > 0) return true;
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
                var skull = new Skull(zone);
                return true;
            }
            return false;
        }
    }
}

