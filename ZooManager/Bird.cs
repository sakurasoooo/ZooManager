using System;
namespace ZooManager
{
    /// <summary>
    /// The abstract Bird Class is used to realize the polymorphism of Raptor and chick. cannot be instantiated.
    /// </summary>
    abstract public class Bird:Animal
	{
		public Bird(Zone zone):base(zone)
		{
		}
	}
}

