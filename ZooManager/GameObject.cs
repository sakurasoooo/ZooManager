using System;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace ZooManager
{
	abstract public class GameObject
	{
        public LayerMask species { get; protected set; } // Record the layer where the game object is located for detection by other game objects.
        public string name { get; protected set; } = ""; // only allow subclass to modify
        public int reactionTime { get; protected set; }  // default reaction time for animals (1 - 10)
        public int turnsActivated { get; protected set; } // The number of rounds the game object survived.
        public string emoji { get; protected set; } = ""; // only allow subclass to modify
        public Zone zone { get; protected set; } // only allow subclass to modify

        /// <summary>
        /// Constructor for GameObject. A reference to a Zone is required. Let GameObject and Zone refer to each other.
        /// </summary>
        /// <param name="zone"></param>
        protected GameObject(Zone zone)
        {
            this.zone = zone;
            this.zone.occupant = this;
        }
        virtual public void ReportLocation()
        {
            Console.WriteLine($"I am at {zone.position.x},{zone.position.y}");
        }
        /// <summary>
        /// Used to be called by Game
        /// Each call increments turnsActivated by 1. It is only expected to be called once per Turn or Round.
        /// </summary>
        virtual public void Activate()
        {
            turnsActivated++;
        }
    }
}

