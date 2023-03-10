using System;
namespace ZooManager
{
    /// <summary>
    /// Prey contract. Every Prey should implement Predators' layermask and Flee methods.
    /// </summary>
	public interface IPrey
    {
        public int Predators { get; set; }
        public bool Flee();
    }
}

