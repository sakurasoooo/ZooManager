using System;
namespace ZooManager
{
	abstract public class GameObject
	{
        public LayerMask species { get; protected set; }
        public string name { get; protected set; } = ""; // only allow subclass to modify
        public int reactionTime { get; protected set; }  // default reaction time for animals (1 - 10)
        public int turnsActivated { get; protected set; }
        public string emoji { get; protected set; } = ""; // only allow subclass to modify
        public Zone zone { get; protected set; } // only allow subclass to modify

        protected GameObject(Zone zone)
        {
            this.zone = zone;
            this.zone.occupant = this;
        }
        virtual public void ReportLocation()
        {
            Console.WriteLine($"I am at {zone.position.x},{zone.position.y}");
        }

        virtual public void Activate()
        {
            turnsActivated++;
        }
    }
}

