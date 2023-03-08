using System;

namespace ZooManager
{
    public class Cat : Animal, IPredator, IPrey
    {
        public int Preys { get; set; }
        public int Predators { get; set; }
        public int Hungry { get; set; }
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

        /* Note that our cat is currently not very clever about its hunting.
         * It will always try to attack "up" and will only seek "down" if there
         * is no mouse above it. This does not affect the cat's effectiveness
         * very much, since the overall logic here is "look around for a mouse and
         * attack the first one you see." This logic might be less sound once the
         * cat also has a predator to avoid, since the cat may not want to run in
         * to a square that sets it up to be attacked!
         */
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

