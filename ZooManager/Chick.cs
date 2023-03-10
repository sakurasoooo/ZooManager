using System;
namespace ZooManager
{
	public class Chick:Bird, IPrey
    {
        public int Predators { get; set; } // Layermask, used to record the layer where the enemies is located

        /// <summary>
        /// The Chick's constructor, the constructor defaults to the chick's natural enemy as Cat
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="name"></param>
        public Chick(Zone zone, string name = "Coco") :base(zone)
		{
			emoji = "🐥";
            this.species = LayerMask.Chick;
            Predators = (int)LayerMask.Cat;
            this.name = name;
            this.reactionTime = new Random().Next(6, 10); // reaction time 6 (medium) to 10 (slow)
        }

        /// <summary>
        /// Used to be called by Game.
        /// When the chick lives 3 turns, it will mature to Raptor
        /// Then it will try to run away from the predator.
        /// </summary>
        public override void Activate()
        {
            base.Activate();
            Mature();
            Console.WriteLine("I am a Chick. Cheep.");
            Flee();

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
                    if (Move(1, Predators, 0) > 0) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Used to be called by Activate.
        /// After three turns, replace self  with raptor
        /// </summary>
        private void Mature()
        {
            if (turnsActivated > 3)
            {
                var mature = new Raptor(zone);
                mature.Activate();
            }

        }
    }
}

