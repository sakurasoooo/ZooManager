using System;
namespace ZooManager
{
	public class Skull:GameObject
    {
        /// <summary>
        /// Skull's constructor
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="name"></param>
        public Skull(Zone zone, string name = "Captain") : base(zone)
        {
            this.emoji = "☠";
            this.species = LayerMask.Skull;
           
            this.name = name;
            this.reactionTime = new Random().Next(0, 0); // reaction time 1 (fast) to 5 (medium)
        }

        /// <summary>
        /// Used to be called by Game.
        /// </summary>
        public override void Activate()
        {
            base.Activate();
            Console.WriteLine("I am a Skull. Click，Click.");

        }
      
	}
}

