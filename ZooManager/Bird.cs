using System;
namespace ZooManager
{

    /// <summary>
    /// The abstract class for Chick and Raptor
    /// Should not be instantiate
    /// </summary>
    abstract public class Bird : Animal
    {
        public Bird(Zone zone) : base(zone)
        {
        }
    }
}

