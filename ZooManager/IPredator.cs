using System;
namespace ZooManager
{
    /// <summary>
    /// Predator contract. Every Predator should implement Preys' layermask and Hunt methods.
    /// </summary>
	public interface IPredator
	{
        public int Preys { get; set; }
        public int Hungry { get;  set; }

        public bool Hunt();
    }

}

