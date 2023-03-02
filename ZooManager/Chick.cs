using System;
namespace ZooManager
{
	public class Chick:Bird, IPrey
    {
        public int Predators { get; set; }
        public Chick(Zone zone, string name = "Coco") :base(zone)
		{
			emoji = "🐥";
            this.species = LayerMask.Chick;
            Predators = (int)LayerMask.Cat;
            this.name = name;
            this.reactionTime = new Random().Next(6, 10); // reaction time 6 (medium) to 10 (slow)
        }

        public override void Activate()
        {
            base.Activate();
            Mature();
            Console.WriteLine("I am a Chick. Cheep.");
            Flee();

        }

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
        /// After three turns, replace yourself with raptor
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

