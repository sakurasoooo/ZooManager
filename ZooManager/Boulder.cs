using System;
using System.Diagnostics;

namespace ZooManager
{
	public class Boulder:Landscape
	{
		
        /// <summary>
        /// Boulder's constructor
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="name"></param>
        public Boulder(Zone zone, string name = "Stone") : base(zone)
        {
            this.emoji = "🪨";
            this.species = LayerMask.Boulder;

            this.name = name;
            this.reactionTime = new Random().Next(0, 0); // reaction time 1 (fast) to 5 (medium)
        }

        /// <summary>
        /// Used to be called by Game.
        /// </summary>
        public override void Activate()
        {
            if (!activate) return;

            base.Activate();
            Console.WriteLine("I am a Boulder. Bang! Bang!");
        }
    }
}

